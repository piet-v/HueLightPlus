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

namespace Ambilight_DFMirage
{
    public partial class Form1 : Form
    {
        List<string> logger = new List<string>();
        readonly driver.DesktopMirror _mirror = new driver.DesktopMirror();
        PerformanceCounter total_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        Dictionary<int, Collection<Point>> leftCoordinates = new Dictionary<int, Collection<Point>>();
        Dictionary<int, Collection<Point>> topCoordinates = new Dictionary<int, Collection<Point>>();
        Dictionary<int, Collection<Point>> bottomCoordinates = new Dictionary<int, Collection<Point>>();
        Dictionary<int, Collection<Point>> rightCoordinates = new Dictionary<int, Collection<Point>>();
        Func<int, int> leftIterator;
        Func<int, int> rightIterator;
        Func<int, int> topIterator;
        Func<int, int> bottomIterator;
        Stopwatch frameTimer;
        Stopwatch portWriteTimer;
        Stopwatch sectionTimer;
        Stopwatch closeTimer;
        Bitmap bmpScreenshot;
        SerialPort huePlusPort;
        Thread a;

        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        byte[] gammaTable = new byte[256];
        byte[] bufferScreen = new byte[125];
        byte[] bufferScreen2 = new byte[125];
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
        int rightLedCount = 10;
        int leftLedCount = 10;
        int topLedCount = 20;
        int bottomLedCount = 20;
        int totalRed;
        int totalGreen;
        int totalBlue;
        int totalCoordinates;
        Color currentColor;

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
            SetupBuffer(bufferScreen, 1);
            SetupBuffer(bufferScreen2, 2);
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
            Dictionary<String, String> config = JsonConvert.DeserializeObject<Dictionary<String, String>>(File.ReadAllText("config.json"));

            formIsHidden = Boolean.Parse(config["startsHidden"]);
            gamma = double.Parse(config["gamma"]);
            rightLedCount = int.Parse(config["rightLedCount"]);
            leftLedCount = int.Parse(config["leftLedCount"]);
            topLedCount = int.Parse(config["topLedCount"]);
            bottomLedCount = int.Parse(config["bottomLedCount"]);
            huePlusPort = new SerialPort(config["huePlusPort"], huePlusBaudRate);
            delay = byte.Parse(config["delay"]);
            scanDepth = int.Parse(config["scanDepth"]);
            pixelsToSkipPerCoordinate = int.Parse(config["pixelsToSkipPerCoordinate"]);
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
            leftIterator = (i) => (i + rightLedCount + topLedCount);
            rightIterator = (i) => (rightLedCount - i - 1);
            topIterator = (i) => (topLedCount - i + rightLedCount - 1);
            bottomIterator = (i) => (bottomLedCount - i - 1);
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
            SetupCoordinatesWith(leftCoordinates, leftLedCount, 0, screenHeight, true);
            SetupCoordinatesWith(rightCoordinates, rightLedCount, screenWidth - (scanDepth + 1), screenHeight, true);
            SetupCoordinatesWith(topCoordinates, topLedCount, 0, screenWidth, false);
            SetupCoordinatesWith(bottomCoordinates, bottomLedCount, screenHeight - (scanDepth + 1), screenWidth, false);

            logger.Add("Calculcated coordinates");
        }

        private void SetupCoordinatesWith(Dictionary<int, Collection<Point>> coordinates, int ledCount, int xOrigin, int xMax, bool isHorizontal)
        {
            int ratio = xMax / ledCount;
            int count = 0;

            for (int ledIndex = 0; ledIndex < ledCount; ledIndex++)
            {
                {
                    coordinates.Add(ledIndex, new Collection<Point>());
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
                                    coordinates[ledIndex].Add(new Point(x, y));
                                }
                                else
                                {
                                    coordinates[ledIndex].Add(new Point(y, x));
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
            while (isEngineEnabled)
            {
                logger.Add("");
                logger.Add("********************");
                logger.Add("Starting new frame");
                frameTimer = new Stopwatch();
                frameTimer.Start();

                CalculateBuffers();
                SendBuffers();

                frameTimer.Stop();
                logger.Add("Finished frame in: " + frameTimer.ElapsedMilliseconds);
                logger.Add("********************");
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

            if (!isSendingFrame)
            {
                isSendingFrame = true;
                fpsCounter++;
                SendBuffersToPort();
            }

            sectionTimer.Stop();
            logger.Add("Handed off buffer to serial in: " + sectionTimer.ElapsedMilliseconds);
        }

        private void EnableNextFrame()
        {
            isSendingFrame = false;
        }

        /*** FILL BUFFERS ***/

        private void FillBuffersWithColor(Color color)
        {
            SetAllLedsToColor(bufferScreen, color);
            SetAllLedsToColor(bufferScreen2, color);
        }

        private void FillBuffersFromScreen()
        {
            if (delay > 0)
            {
                Thread.Sleep(delay);
            }

            UpdateScreenShot();

            FillBufferFromScreenWith(bufferScreen, leftCoordinates, leftIterator, leftLedCount);
            FillBufferFromScreenWith(bufferScreen, rightCoordinates, rightIterator, rightLedCount);
            FillBufferFromScreenWith(bufferScreen, topCoordinates, topIterator, topLedCount);
            FillBufferFromScreenWith(bufferScreen2, bottomCoordinates, bottomIterator, bottomLedCount);

            DisposeScreenShot();
        }

        private void FillBufferFromScreenWith(byte[] buffer, Dictionary<int, Collection<Point>> coordinates, Func<int, int> LedIterator, int ledCount)
        {
            foreach (var currentLedCoordinates in coordinates)
            {
                totalRed = totalGreen = totalBlue = 0;
                totalCoordinates = currentLedCoordinates.Value.Count;

                foreach(Point currentLedCoordinate in currentLedCoordinates.Value)
                {
                    currentColor = bmpScreenshot.GetPixel(currentLedCoordinate.X, currentLedCoordinate.Y);
                    totalRed += currentColor.R;
                    totalGreen += currentColor.G;
                    totalBlue += currentColor.B;
                }

                SetOneLedToColor(buffer, LedIterator(currentLedCoordinates.Key), Color.FromArgb(totalRed / totalCoordinates, totalGreen / totalCoordinates, totalBlue / totalCoordinates));
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
            try
            {
                WriteAndCallback(bufferScreen, () =>
                {
                    WriteAndCallback(bufferScreen2, EnableNextFrame);
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
            portWriteTimer = new Stopwatch();
            portWriteTimer.Start();

            huePlusPort.Write(buffer, 0, buffer.Length);
            WaitForBufferReceived(buffer, () =>
            {
                portWriteTimer.Stop();
                logger.Add("Written channel" + buffer[1] + ": " + portWriteTimer.ElapsedMilliseconds);

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
        private Bitmap UpdateScreenShot()
        {
            isReadingScreen = true;
            bmpScreenshot = _mirror.GetScreen();
            isReadingScreen = false;

            return bmpScreenshot;
        }

        private void DisposeScreenShot()
        {
            bmpScreenshot.Dispose();
            bmpScreenshot = null;
        }

        private void SaveScreenShot()
        {
            UpdateScreenShot().Save("screenshot.bmp");
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
