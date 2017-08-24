using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

struct Led
{
    public int channel;
    public int ledIndex;

    public Led(int channel, int ledIndex)
    {
        this.channel = channel;
        this.ledIndex = ledIndex;
    }
}

struct ScreenRegion
{
    public Led[] leds;
    public Dictionary<int, Collection<Point>> coordinates;

    public ScreenRegion(Led[] leds)
    {
        this.leds = leds;
        this.coordinates = new Dictionary<int, Collection<Point>>();
    }
}

struct ScreenRegions
{
    public ScreenRegion right;
    public ScreenRegion left;
    public ScreenRegion top;
    public ScreenRegion bottom;

    public ScreenRegions(ScreenRegion right, ScreenRegion top, ScreenRegion left, ScreenRegion bottom)
    {
        this.right = right;
        this.top = top;
        this.left = left;
        this.bottom = bottom;
    }
}

namespace Ambilight_DFMirage
{
    public partial class Form1 : Form
    {
        List<string> logger = new List<string>();
        readonly driver.DesktopMirror _mirror = new driver.DesktopMirror();
        PerformanceCounter total_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        Dictionary<int, SerialPort> ports = new Dictionary<int, SerialPort>();
        Dictionary<int, byte[]> buffers = new Dictionary<int, byte[]>() { { 0, new byte[125] }, { 1, new byte[125] } };
        ScreenRegions screenRegions;
        Func<int, int> leftIterator;
        Func<int, int> rightIterator;
        Func<int, int> topIterator;
        Func<int, int> bottomIterator;
        Stopwatch frameTimer;
        Stopwatch portChannelTimer;
        Stopwatch portWriteTimer;
        Stopwatch sectionTimer;
        Stopwatch closeTimer;
        byte[] screenBuffer;
        SerialPort huePlusPort;
        Thread a;

        int colorIndex = 0;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        byte[] gammaTable = new byte[256];
        const byte ledStripsPerChannel = 4; // Always assume max amount of strips is connected or serial stream fails (for now)
        const byte headerBits = 5;
        const byte magicBit = 75;
        const byte colorBits = 3;
        const byte animationMode = 0;
        const byte animationDirection = 0;
        const byte animationOptions = 0;
        const byte animationGroup = 0;
        const byte animationSpeed = 2;
        const int huePlusBaudRate = 256000;
        byte delay = 0;
        byte fpsCounter = 0;
        double gamma = 1;
        int scanDepth = 100;
        int pixelsToSkipPerCoordinate = 100; // Every LED region has (scanDepth * ScreenBorderPixelsInRegion / pixelsToSkipPerCoordinate) = possible coordinates. E.g. (100 * 144 / 100) = 144 coordinates;

        int totalRed;
        int totalGreen;
        int totalBlue;
        int totalCoordinates;

        bool formIsHidden = false;
        bool isEngineEnabled = true;
        bool isSendingFrame = false;
        bool isReadingScreen = false;

        /*** FORM ON-INIT ***/

        public Form1()
        {
            logger.Add("Started HueLight");
            InitializeComponent();

            LoadConfig();
            SetupBuffer(buffers[0], 1);
            SetupBuffer(buffers[1], 2);
            SetupGammaTable();
            SetupUiLabels();
            SetupPixelIterators();

            Microsoft.Win32.SystemEvents.SessionSwitch += CloseForm;
            logger.Add("Hooked session switch event");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenHuePort();
            SetupCoordinates();
            ConnectMirrorDriver();
            StartEngine();

            logger.Add("Form lLoaded");
        }

        /*** FORM ON-CLOSE ***/

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (isReadingScreen)
            {
                ;
            }

            isEngineEnabled = false;
            logger.Add("Turning off rendering");

            _mirror.Dispose();
            logger.Add("Driver Disposed");

            //TurnOffLEDS(); // Only works in Debug; memory leak or nullPointer?

            CloseHuePort();
            logger.Add("Exit Success");

            Microsoft.Win32.SystemEvents.SessionSwitch -= CloseForm;
            logger.Add("Unhooked session switch event");

            WriteLoggerToFile();
        }

        /*** CONFIG METHODS ***/

        private void LoadConfig()
        {
            dynamic config = JsonConvert.DeserializeObject(File.ReadAllText("config.json"));

            for (int deviceIndex = 0; deviceIndex < config.devices.Count; deviceIndex++)
            {
                String port = (string)config.devices[deviceIndex].port;
                ports.Add(deviceIndex, new SerialPort(port, huePlusBaudRate));
            }

            huePlusPort = ports[0];
            screenRegions = config.screenRegions.ToObject<ScreenRegions>();
            formIsHidden = config.startsHidden;
            gamma = config.gamma;
            delay = config.delay;
            scanDepth = config.scanDepth;
            pixelsToSkipPerCoordinate = config.pixelsToSkipPerCoordinate;
        }

        private void SetupBuffer(byte[] buffer, byte channel)
        {
            buffer[0] = magicBit;
            buffer[1] = channel;
            buffer[2] = animationMode;
            buffer[3] = animationDirection << 4 | animationOptions << 3 | ledStripsPerChannel;
            buffer[4] = 0 << 5 | animationGroup << 3 | animationSpeed;

            logger.Add("Buffers created");
        }

        private void SetupGammaTable()
        {
            for (int i = 0; i < 256; i++)
            {
                gammaTable[i] = (byte)(Math.Pow((float)i / 255.0, gamma) * 255 + 0.5);
            }

            logger.Add("Gamma table created from " + gamma.ToString());
        }

        private void SetupUiLabels()
        {
            label4.Text = "Gamma: " + gamma.ToString();
            label5.Text = "";
            label6.Text = "HUE+ Port: " + huePlusPort.PortName.ToString() + " Baudrate: " + huePlusPort.BaudRate.ToString();
            label7.Text = "ScanDepth: " + scanDepth.ToString() + " Skip: " + pixelsToSkipPerCoordinate.ToString();

            logger.Add("Loaded form labels ");
        }

        private void SetupPixelIterators()
        {
            leftIterator = (i) => (i + screenRegions.right.leds.Length + screenRegions.top.leds.Length);
            rightIterator = (i) => (screenRegions.right.leds.Length - i - 1);
            topIterator = (i) => (screenRegions.top.leds.Length - i + screenRegions.right.leds.Length - 1);
            bottomIterator = (i) => (screenRegions.bottom.leds.Length - i - 1);
        }

        private void WriteLoggerToFile()
        {
            logger.Add(DateTime.Now.ToString());
            logger.Add("Log End ..");
            File.WriteAllLines("log.txt", logger);
        }

        /*** COORDINATE DICTIONARY SETUP ***/

        private void SetupCoordinates()
        {
            SetupCoordinatesWith(ref screenRegions.left, 0, screenHeight, true);
            SetupCoordinatesWith(ref screenRegions.right, screenWidth - (scanDepth + 1), screenHeight, true);
            SetupCoordinatesWith(ref screenRegions.top, 0, screenWidth, false);
            SetupCoordinatesWith(ref screenRegions.bottom, screenHeight - (scanDepth + 1), screenWidth, false);

            logger.Add("Calculcated coordinates");
        }

        private void SetupCoordinatesWith(ref ScreenRegion screenRegion, int xOrigin, int xMax, bool isHorizontal)
        {
            int ratio = xMax / screenRegion.leds.Length;
            int count = 0;

            screenRegion.coordinates = new Dictionary<int, Collection<Point>>();

            for (int ledIndex = 0; ledIndex < screenRegion.leds.Length; ledIndex++)
            {
                {
                    screenRegion.coordinates.Add(ledIndex, new Collection<Point>());
                    for (int x = xOrigin; x < xOrigin + scanDepth; x++)
                    {
                        int yOrigin = ledIndex * ratio;
                        int yMax = yOrigin + ratio;

                        for (int y = yOrigin; y < yMax; y++)
                        {
                            count++;
                            if ((count % pixelsToSkipPerCoordinate) == 0)
                            {
                                if (isHorizontal)
                                {
                                    screenRegion.coordinates[ledIndex].Add(new Point(x, y));
                                }
                                else
                                {
                                    screenRegion.coordinates[ledIndex].Add(new Point(y, x));
                                }
                            }
                        }
                    }
                }
            }
        }

        /*** START ENGINE ***/

        private void StartEngine()
        {
            a = new Thread(AmbiEngine);
            a.Start();
            logger.Add("Thread Started");
        }

        /*** ENGINE ***/

        private void TurnOffLEDS()
        {
            closeTimer = new Stopwatch();
            closeTimer.Start();

            isSendingFrame = true;
            FillBuffersWithColor(Color.FromArgb(0, 0, 0));
            SendBuffersToPort();

            while (isSendingFrame)
            {
                ;
            }

            closeTimer.Stop();
            logger.Add("Turned off LEDS in: " + sectionTimer.ElapsedMilliseconds);
        }

        private void AmbiEngine()
        {
            frameTimer = new Stopwatch();
            frameTimer.Start();

            logger.Add("");
            logger.Add("********************");
            logger.Add("Starting new frame");

            CalculateBuffers();

            while (isEngineEnabled)
            {
                if(!isSendingFrame)
                {
                    SendBuffers();

                    frameTimer.Stop();
                    logger.Add("Finished frame in: " + frameTimer.ElapsedMilliseconds);
                    logger.Add("********************");
                    logger.Add("");

                    frameTimer = new Stopwatch();
                    frameTimer.Start();

                    logger.Add("");
                    logger.Add("********************");
                    logger.Add("Starting new frame");

                    CalculateBuffers();
                }
            }
        }

        private void CalculateBuffers()
        {
            sectionTimer = new Stopwatch();
            sectionTimer.Start();

            try
            {
                FillBuffersFromScreen();
            }
            catch (Exception e)
            {
                logger.Add("Error during calculations: " + e);
            }

            sectionTimer.Stop();
            logger.Add("Calculated buffer in: " + sectionTimer.ElapsedMilliseconds);
        }

        private void SendBuffers()
        {
            sectionTimer = new Stopwatch();
            sectionTimer.Start();

            isSendingFrame = true;
            fpsCounter++;
            SendBuffersToPort();

            sectionTimer.Stop();
            logger.Add("Handed off buffer to serial in: " + sectionTimer.ElapsedMilliseconds);
        }

        private void EnableNextFrame()
        {
            isSendingFrame = false;

            portWriteTimer.Stop();
            logger.Add("--------------------");
            logger.Add("Written both buffers in: " + portWriteTimer.ElapsedMilliseconds);
            logger.Add("--------------------");
        }

        /*** FILL BUFFERS ***/

        private void FillBuffersWithColor(Color color)
        {
            SetAllLedsToColor(buffers[0], color);
            SetAllLedsToColor(buffers[1], color);
        }

        private void FillBuffersFromScreen()
        {
            if (delay > 0)
            {
                Thread.Sleep(delay);
            }

            UpdateScreenShot();

            FillBufferFromScreenWith(screenRegions.left, leftIterator);
            FillBufferFromScreenWith(screenRegions.right, rightIterator);
            FillBufferFromScreenWith(screenRegions.top, topIterator);
            FillBufferFromScreenWith(screenRegions.bottom, bottomIterator);
        }

        private void FillBufferFromScreenWith(ScreenRegion screenRegion, Func<int, int> LedIterator)
        {
            foreach (var currentLedCoordinates in screenRegion.coordinates)
            {
                totalRed = totalGreen = totalBlue = 0;
                totalCoordinates = currentLedCoordinates.Value.Count;

                foreach (Point currentLedCoordinate in currentLedCoordinates.Value)
                {
                    colorIndex = Screen.PrimaryScreen.Bounds.Width * 4 * currentLedCoordinate.Y + currentLedCoordinate.X;

                    totalRed += screenBuffer[colorIndex++];
                    totalGreen += screenBuffer[colorIndex++];
                    totalBlue += screenBuffer[colorIndex++];
                }

                SetOneLedToColor(buffers[screenRegion.leds[currentLedCoordinates.Key].channel - 1], LedIterator(currentLedCoordinates.Key), Color.FromArgb(totalRed / totalCoordinates, totalGreen / totalCoordinates, totalBlue / totalCoordinates));
            }
        }

        /*** BUFFER COLOR SETTERS ***/

        private void SetOneLedToColor(byte[] buffer, int ledIndex, Color color)
        {
            int bufferIndex = colorBits * ledIndex + headerBits;
            SetBufferColorAt(buffer, bufferIndex, color);
        }

        private void SetAllLedsToColor(byte[] buffer, Color color)
        {
            for (int bufferIndex = headerBits; bufferIndex < buffer.Length; bufferIndex += colorBits)
            {
                SetBufferColorAt(buffer, bufferIndex, color);
            }
        }

        private void SetBufferColorAt(byte[] buffer, int bufferIndex, Color color)
        {
            buffer[bufferIndex++] = gammaTable[color.G];
            buffer[bufferIndex++] = gammaTable[color.R];
            buffer[bufferIndex++] = gammaTable[color.B];
        }

        /*** SEND BUFFERS ***/

        private void SendBuffersToPort()
        {
            portWriteTimer = new Stopwatch();
            portWriteTimer.Start();

            try
            {
                WriteAndCallback(buffers[0], () =>
                {
                    WriteAndCallback(buffers[1], EnableNextFrame);
                });
            }
            catch (Exception e)
            {
                logger.Add("sendScreenBuffer error " + e);
            }
        }

        /*** HUE PORT METHODS ***/

        private void OpenHuePort()
        {
            while (!huePlusPort.IsOpen)
                try
                {
                    huePlusPort.Open();
                }
                catch (Exception e)
                {
                    logger.Add("Error at serial.open: " + e);
                }
        }

        private void CloseHuePort()
        {
            while (huePlusPort.IsOpen)
                try
                {
                    huePlusPort.Close();
                }
                catch (Exception)
                {
                }
            logger.Add("closed serial port");
        }

        private void WriteAndCallback(byte[] buffer, Action Callback)
        {
            portChannelTimer = new Stopwatch();
            portChannelTimer.Start();

            huePlusPort.Write(buffer, 0, buffer.Length);
            WaitForBufferReceived(buffer, () =>
            {
                portChannelTimer.Stop();
                //logger.Add("Written channel" + buffer[1] + ": " + portChannelTimer.ElapsedMilliseconds);

                Callback();
            });
        }

        private void WaitForBufferReceived(byte[] buffer, Action Callback)
        {
            Byte[] readBuffer = new byte[buffer.Length];

            if (huePlusPort.IsOpen)
            {
                huePlusPort.BaseStream.BeginRead(readBuffer, 0, readBuffer.Length, (IAsyncResult ar) =>
                {
                    if (huePlusPort.IsOpen)
                    {
                        huePlusPort.BaseStream.EndRead(ar);

                        Callback();
                    }
                }, null);
            }
        }

        /*** MIRROR DRIVER METHODS ***/

        private void ConnectMirrorDriver()
        {
            _mirror.Load();
            logger.Add("Driver Loaded");

            _mirror.Connect();
            logger.Add("Driver Connected");
        }

        /*** SCREENSHOT METHODS ***/
        private byte[] UpdateScreenShot()
        {
            isReadingScreen = true;
            screenBuffer = _mirror.GetScreenBuffer();
            isReadingScreen = false;

            return screenBuffer;
        }

        private void SaveScreenShot()
        {
            _mirror.GetScreen().Save("screenshot.bmp");
        }

        /*** REALTIME UI UPDATES ***/

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            String fps = "FPS: " + fpsCounter.ToString();
            String cpu = "CPU: " + (Math.Round(total_cpu.NextValue())).ToString() + " %";

            notifyIcon1.Text = "HueLight+ - " + fps + " - " + cpu;
            label1.Text = fps;
            label2.Text = cpu;

            fpsCounter = 0;
        }

        /*** FORM INTERACTION ***/

        private void button1_Click(object sender, EventArgs e)
        {
            SaveScreenShot();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            formIsHidden = true;
            logger.Add("Hide Button Clicked");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (formIsHidden)
            {
                Hide();
            }
        }

        private void CloseForm(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            logger.Add("Closing due to session change");
            Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (formIsHidden)
            {
                Show();
                logger.Add("Form Shown");
            }
            else
            {
                Hide();
                logger.Add("Form formIsHidden");
            }
            formIsHidden = !formIsHidden;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.Add("Closing due to notification menu");
            Close();
        }
    }
}
