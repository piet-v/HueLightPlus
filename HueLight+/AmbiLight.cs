using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HueLightPlus
{
    enum Direction
    {
        Left,
        Top,
        Right,
        Bottom
    };

    struct Led
    {
        public int channel;
        public int ledIndex;
        public int device;

        public Led(int device, int channel, int ledIndex)
        {
            this.device = device;
            this.channel = channel;
            this.ledIndex = ledIndex;
        }
    }

    struct ScreenRegion
    {
        public Led[] leds;
        public Collection<Point> coordinates;

        public ScreenRegion(Led[] leds)
        {
            this.leds = leds;
            this.coordinates = new Collection<Point>();
        }
    }

    class ScreenSide
    {
        public ScreenRegion[] screenRegions;
        public int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        private int xOrigin;
        private int xMax;
        private bool isHorizontal;
        public Direction direction;

        public ScreenSide(ScreenRegion[] screenRegions, Direction direction)
        {
            this.screenRegions = screenRegions;
            this.direction = direction;

            InitCoordinates();
        }

        public void InitCoordinates()
        {
            switch (direction)
            {
                case Direction.Right:
                    xOrigin = screenWidth - (AmbiLight.scanDepth + 1);
                    xMax = screenHeight;
                    isHorizontal = true;
                    break;
                case Direction.Left:
                    xOrigin = 0;
                    xMax = screenHeight;
                    isHorizontal = true;
                    break;
                case Direction.Top:
                    xOrigin = 0;
                    xMax = screenWidth;
                    isHorizontal = false;
                    break;
                default:
                    xOrigin = screenHeight - (AmbiLight.scanDepth + 1);
                    xMax = screenWidth;
                    isHorizontal = false;
                    break;
            }

            SetupCoordinates();
        }

        /*** COORDINATE DICTIONARY SETUP ***/
        public void SetupCoordinates()
        {
            int screenRegionAmount = screenRegions.Length;

            if (screenRegionAmount > 0)
            {
                int ratio = xMax / screenRegionAmount;
                int count = 0;

                for (int regionIndex = 0; regionIndex < screenRegionAmount; regionIndex++)
                {
                    ref ScreenRegion screenRegion = ref screenRegions[regionIndex];

                    screenRegion.coordinates = new Collection<Point>();

                    for (int x = xOrigin; x < xOrigin + AmbiLight.scanDepth; x++)
                    {
                        int yOrigin = regionIndex * ratio;
                        int yMax = yOrigin + ratio;

                        for (int y = yOrigin; y < yMax; y++)
                        {
                            count++;
                            if ((count % AmbiLight.pixelsToSkipPerCoordinate) == 0)
                            {
                                if (isHorizontal)
                                {
                                    screenRegion.coordinates.Add(new Point(x, y));
                                }
                                else
                                {
                                    screenRegion.coordinates.Add(new Point(y, x));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    class AmbiLight
    {
        public static bool multiThreading = false;
        public static readonly EventWaitHandle waitHandle = new AutoResetEvent(true);
        public static int scanDepth = 100;
        public static int pixelsToSkipPerCoordinate = 100; // Every LED region has (scanDepth * ScreenBorderPixelsInRegion / pixelsToSkipPerCoordinate) = possible coordinates. E.g. (100 * 144 / 100) = 144 coordinates;
        public static int delay = 0;
        public static int frameTimeout = 2000;
        public static bool previewMode = false;

        Stopwatch frameTimer = new Stopwatch();
        Stopwatch sectionTimer = new Stopwatch();
        public readonly driver.DesktopMirror _mirror = new driver.DesktopMirror();

        public HuePorts huePorts;
        public ScreenSide[] screenSides;
        public bool formIsHidden = false;
        public byte fpsCounter = 0;
        byte[] screenBuffer;
        bool isEngineEnabled = true;
        bool isReadingScreen = false;
        private Bitmap colorPreview;
        private PictureBox pictureBox;
        private int pictureBoxX;
        private int pictureBoxY;

        public AmbiLight(ScreenSide[] screenSides, bool formIsHidden, HuePorts huePorts, PictureBox pictureBox)
        {
            this.formIsHidden = formIsHidden;
            this.huePorts = huePorts;
            this.screenSides = screenSides;
            this.pictureBox = pictureBox;

            setupPictureBox();
        }

        private void setupPictureBox()
        {
            int left = 0;
            int right = 0;
            int top = 0;
            int bottom = 0;

            foreach (var screenSide in screenSides)
            {
                switch (screenSide.direction)
                {
                    case Direction.Right:
                        right = screenSide.screenRegions.Length;
                        break;
                    case Direction.Left:
                        left = screenSide.screenRegions.Length;
                        break;
                    case Direction.Top:
                        top = screenSide.screenRegions.Length;
                        break;
                    default:
                        bottom = screenSide.screenRegions.Length;
                        break;
                }
            }
            pictureBoxX = Math.Max(top, bottom);
            pictureBoxY = Math.Max(left, right) + 2;

            colorPreview = new Bitmap(pictureBoxX, pictureBoxY);
        }

        /*** AMBILIGHT STATE MANAGEMENT ***/
        public void Start()
        {
            huePorts.OpenAll();
            ConnectMirrorDriver();

            Thread a = new Thread(AmbiEngine);
            a.Start();
            Logger.Add("Thread Started");
        }

        public void Stop()
        {
            while (isReadingScreen)
            {
                ;
            }

            isEngineEnabled = false;
            Logger.Add("Turning off rendering");

            huePorts.StopAll();

            DisconnectMirrorDriver();
            Logger.Add("Exit Success");
            Logger.Finish();
        }

        /*** MIRROR DRIVER METHODS ***/
        private void ConnectMirrorDriver()
        {
            _mirror.Load();
            Logger.Add("Driver Loaded");

            _mirror.Connect();
            Logger.Add("Driver Connected");
        }

        private void DisconnectMirrorDriver()
        {
            _mirror.Dispose();
            Logger.Add("Driver Disposed");
        }

        /*** ENGINE ***/
        private void AmbiEngine()
        {
            frameTimer.Start();

            Logger.AddLine("ENGINE - START");

            CalculateBuffers();

            while (isEngineEnabled)
            {
                SendBuffers();
                CalculateBuffers();
            }
        }

        private void CalculateBuffers()
        {
            sectionTimer.Restart();

            try
            {
                FillBuffersFromScreen();
            }
            catch (Exception e)
            {
                Logger.Add("Error during calculations: " + e);
            }

            long bufferSeconds = sectionTimer.ElapsedMilliseconds;
            sectionTimer.Restart();

            waitHandle.WaitOne(frameTimeout);
            waitHandle.Reset();

            Logger.Add("Finished frame in: " + frameTimer.ElapsedMilliseconds);
            frameTimer.Restart();

            Logger.AddLine("FRAME");
            Logger.Add("Calculated buffer in: " + bufferSeconds);
            Logger.Add("Waiting for ports: " + sectionTimer.ElapsedMilliseconds);
        }

        /*** SEND BUFFERS ***/
        private void SendBuffers()
        {
            sectionTimer.Restart();

            huePorts.WriteBuffers();
            fpsCounter++;

            Logger.Add("Handed off buffer to serial in: " + sectionTimer.ElapsedMilliseconds);
        }

        /*** FILL BUFFERS ***/
        private void FillBuffersFromScreen()
        {
            if (delay > 0)
            {
                Thread.Sleep(delay);
            }

            UpdateScreenShot();

            if (multiThreading)
            {
                Parallel.ForEach(screenSides, FillBufferFromScreenWith);
                updatePreview();
            }
            else
            {
                foreach (var screenSide in screenSides)
                {
                    FillBufferFromScreenWith(screenSide);
                    updatePreview();
                }
            }
        }

        private void updatePreview()
        {
            if (previewMode)
            {
                pictureBox.Image = new Bitmap(colorPreview, pictureBoxX, pictureBoxY);
            }
        }

        private void FillBufferFromScreenWith(ScreenSide screenSide)
        {
            ScreenRegion currentScreenRegion;
            Collection<Point> currentLedCoordinates;
            int totalRed;
            int totalGreen;
            int totalBlue;
            int totalCoordinates;
            int colorIndex = 0;

            for (int regionIndex = 0; regionIndex < screenSide.screenRegions.Length; regionIndex++)
            {
                currentScreenRegion = screenSide.screenRegions[regionIndex];
                currentLedCoordinates = currentScreenRegion.coordinates;

                totalRed = totalGreen = totalBlue = 0;
                totalCoordinates = currentLedCoordinates.Count;

                foreach (Point currentLedCoordinate in currentLedCoordinates)
                {
                    colorIndex = Screen.PrimaryScreen.Bounds.Width * 4 * currentLedCoordinate.Y + currentLedCoordinate.X * 4;

                    totalBlue += screenBuffer[colorIndex++];
                    totalGreen += screenBuffer[colorIndex++];
                    totalRed += screenBuffer[colorIndex++];
                }

                Color color = Color.FromArgb(totalRed / totalCoordinates, totalGreen / totalCoordinates, totalBlue / totalCoordinates);

                foreach (Led currentLed in currentScreenRegion.leds)
                {
                    if (huePorts.huePorts[currentLed.device].isEnabled)
                    {
                        huePorts.huePorts[currentLed.device].SetOneLedToColor(currentLed.channel, currentLed.ledIndex, color);
                    }   
                }

                if (previewMode)
                {
                    switch (screenSide.direction)
                    {
                        case Direction.Right:
                            colorPreview.SetPixel(19, regionIndex + 1, color);
                            break;
                        case Direction.Left:
                            colorPreview.SetPixel(0, regionIndex + 1, color);
                            break;
                        case Direction.Top:
                            colorPreview.SetPixel(regionIndex, 1, color);
                            break;
                        default:
                            colorPreview.SetPixel(regionIndex, 11, color);
                            break;
                    }
                }
            }
        }

        /*** SCREENSHOT METHODS ***/
        private byte[] UpdateScreenShot()
        {
            isReadingScreen = true;
            try
            {
                screenBuffer = _mirror.GetScreenBuffer();
            }
            catch (Exception e)
            {
                Logger.Add("Something went wrong while getting the screen buffer:" + e);
            }
            isReadingScreen = false;

            return screenBuffer;
        }

        /*** CONFIG ***/

        public int SetScanDepth(int scanDepth)
        {
            AmbiLight.scanDepth = scanDepth;

            foreach (ScreenSide screenSide in screenSides)
            {
                screenSide.SetupCoordinates();
            }

            return scanDepth;
        }

        public int SetPixelsToSkipPerCoordinate(int pixelsToSkipPerCoordinate)
        {
            AmbiLight.pixelsToSkipPerCoordinate = pixelsToSkipPerCoordinate;

            foreach (ScreenSide screenSide in screenSides)
            {
                screenSide.SetupCoordinates();
            }

            return pixelsToSkipPerCoordinate;
        }
    }
}
