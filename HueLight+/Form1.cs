using HueLightPlus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace Ambilight_DFMirage
{
    public partial class Form1 : Form
    {
        PerformanceCounter total_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        AmbiLight ambiLight;

        /*** FORM ON-INIT ***/

        public Form1()
        {
            InitializeComponent();

            SetupAmbiLight();
            SetupUiLabels();

            Microsoft.Win32.SystemEvents.SessionSwitch += CloseForm;
            Logger.Add("Hooked session switch event");

            ambiLight.Start();
        }

        /*** FORM ON-CLOSE ***/

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ambiLight.Stop();

            Microsoft.Win32.SystemEvents.SessionSwitch -= CloseForm;
            Logger.Add("Unhooked session switch event");
        }

        /*** CONFIG METHODS ***/

        private void SetupAmbiLight()
        {
            dynamic config = JsonConvert.DeserializeObject(File.ReadAllText("config.json"));
            
            bool formIsHidden = config.startsHidden;
            byte delay = config.delay;
            double gamma = config.gamma;

            AmbiLight.multiThreading = config.multiThreading;
            AmbiLight.scanDepth = config.scanDepth;
            AmbiLight.pixelsToSkipPerCoordinate = config.pixelsToSkipPerCoordinate;

            HuePorts huePorts = new HuePorts(config.devices.ToObject<Device[]>(), gamma);
            ScreenSide right = new ScreenSide(config.ambiLight.right.screenRegions.ToObject<ScreenRegion[]>(), Direction.Right);
            ScreenSide left = new ScreenSide(config.ambiLight.left.screenRegions.ToObject<ScreenRegion[]>(), Direction.Left);
            ScreenSide top = new ScreenSide(config.ambiLight.top.screenRegions.ToObject<ScreenRegion[]>(), Direction.Top);
            ScreenSide bottom = new ScreenSide(config.ambiLight.bottom.screenRegions.ToObject<ScreenRegion[]>(), Direction.Bottom);
            ScreenSide[] screenSides = new ScreenSide[] { top, right, bottom, left };

            ambiLight = new AmbiLight(screenSides, formIsHidden, delay, huePorts);
            Logger.Add("Loaded AmbiLight from config.json");
            Logger.Add("----------------------------");
        }

        private void SetupUiLabels()
        {
            multiThreadingCheckBox.Checked = AmbiLight.multiThreading;
            gammaTrackBar.Value = (int)(10 * Math.Round(ambiLight.huePorts.gamma, 1));
            scanDepthTrackBar.Value = AmbiLight.scanDepth;
            SkipTrackbar.Value = AmbiLight.pixelsToSkipPerCoordinate;
            gammaValueLabel.Text = ambiLight.huePorts.gamma.ToString();
            label6.Text = "HUE+ Port: " + ambiLight.huePorts.huePorts[0].serialPort.PortName.ToString() + " Baudrate: " + ambiLight.huePorts.baudRate.ToString();

            Logger.AddLine("FORM CONFIG");
            Logger.Add("Loaded form labels ");
        }

        /*** REALTIME UI UPDATES ***/

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            String fps = "FPS: " + ambiLight.fpsCounter.ToString();
            String cpu = "CPU: " + (Math.Round(total_cpu.NextValue())).ToString() + " %";

            notifyIcon1.Text = "HueLight+ - " + fps + " - " + cpu;
            fpsLabel.Text = fps;
            cpuLabel.Text = cpu;

            ambiLight.fpsCounter = 0;
        }

        /*** FORM INTERACTION ***/

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            ambiLight.formIsHidden = true;
            Logger.Add("Hide Button Clicked");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (ambiLight.formIsHidden)
            {
                Hide();
            }
        }

        private void CloseForm(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            Logger.Add("Closing due to session change");
            Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ambiLight.formIsHidden)
            {
                Show();
                Logger.Add("Form Shown");
            }
            else
            {
                Hide();
                Logger.Add("Form formIsHidden");
            }
            ambiLight.formIsHidden = !ambiLight.formIsHidden;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Add("Closing due to notification menu");
            Close();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            Logger.AddLine("UI - GAMMA");
            gammaValueLabel.Text = ambiLight.huePorts.SetGamma(gammaTrackBar.Value / 10.0).ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Logger.AddLine("UI - MULTI-THREADING");
            AmbiLight.multiThreading = multiThreadingCheckBox.Checked;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            Logger.AddLine("UI - SCANDEPTH");
            AmbiLight.scanDepth = scanDepthTrackBar.Value;
            scanDepthValueLabel.Text = AmbiLight.scanDepth.ToString();
            //TODO recalculate coordinates
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            Logger.AddLine("UI - SKIP");
            AmbiLight.pixelsToSkipPerCoordinate = SkipTrackbar.Value;
            skipValueLabel.Text = AmbiLight.pixelsToSkipPerCoordinate.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
