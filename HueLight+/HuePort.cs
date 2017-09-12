using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;

namespace HueLightPlus
{
    class HuePort
    {
        public SerialPort serialPort;
        public HueBuffer[] buffers = new HueBuffer[] { new HueBuffer(1), new HueBuffer(2) };
        public double gamma = 1;
        public String port;
        public int baudRate;
        public byte[] gammaTable = new byte[256];

        const byte headerBits = 5;
        const byte colorBits = 3;

        public HuePort(String port, int baudRate, double gamma)
        {
            serialPort = new SerialPort(port, baudRate);
            this.port = port;
            this.baudRate = baudRate;

            SetGamma(gamma);
        }

        /*** SERIAL PORT STATE MANAGEMENT ***/

        public void Open()
        {
            while (!serialPort.IsOpen)
                try
                {
                    serialPort.Open();
                }
                catch (Exception e)
                {
                    Logger.Add("Error at serial.open: " + e);
                }
            Logger.Add("opened serial port: " + port);
        }

        public void Close()
        {
            while (serialPort.IsOpen)
                try
                {
                    serialPort.Close();
                }
                catch (Exception e)
                {
                    Logger.Add("Error at serial.close: " + e);
                }
            Logger.Add("Closed serial port: " + port);
        }

        /*** GAMMA SETUP ***/

        public double SetGamma(double gamma)
        {
            this.gamma = gamma;

            SetupGammaTable(gamma);

            return gamma;
        }

        private void SetupGammaTable(double gamma)
        {
            for (int i = 0; i < 256; i++)
            {
                gammaTable[i] = (byte)(Math.Pow((float)i / 255.0, gamma) * 255 + 0.5);
            }

            Logger.Add("Gamma table created from " + gamma.ToString());
        }

        /*** BUFFER WRITING ***/

        public void WriteBuffers(Action CallBack)
        {
            Stopwatch portWriteTimer = new Stopwatch();
            portWriteTimer.Start();

            buffers[0].WriteAndCallback(serialPort, () =>
            {
                buffers[1].WriteAndCallback(serialPort, () =>
                {
                    portWriteTimer.Stop();
                    Logger.Add("Written port " + serialPort.PortName + " in: " + portWriteTimer.ElapsedMilliseconds);

                    CallBack();
                });
            });
        }

        /*** BUFFER COLOR SETTERS ***/

        public void SetOneLedToColor(int channel, int ledIndex, Color color)
        {
            byte[] buffer = buffers[channel].buffer;

            int bufferIndex = colorBits * ledIndex + headerBits;
            SetBufferColorAt(buffer, bufferIndex, color);
        }

        public void SetAllLedsToColor(int channel, Color color)
        {
            byte[] buffer = buffers[channel].buffer;

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
    }

    struct Device
    {
        public String port;

        public Device(String port)
        {
            this.port = port;
        }
    }

    class HuePorts
    {
        public HuePort[] huePorts;
        public int baudRate = 256000;
        public double gamma;

        public HuePorts(Device[] devices, double gamma)
        {
            this.gamma = gamma;

            huePorts = new HuePort[devices.Length];

            for (int deviceIndex = 0; deviceIndex < devices.Length; deviceIndex++)
            {
                String port = devices[deviceIndex].port;
                huePorts[deviceIndex] = new HuePort(port, baudRate, gamma);
            }
        }

        public double SetGamma(double gamma)
        {
            foreach (HuePort huePort in huePorts)
            {
                huePort.SetGamma(gamma);
            }

            return gamma;
        }

        public void OpenAll()
        {
            foreach (HuePort huePort in huePorts)
            {
                huePort.Open();
            }
        }

        public void StopAll()
        {
            foreach (HuePort huePort in huePorts)
            {
                huePort.Close();
            }
        }

        public void WriteBuffers()
        {
            int threadsDone = 0;

            Action<HuePort> WriteBuffer = (HuePort huePort) =>
            {
                huePort.WriteBuffers(() =>
                {
                    if (++threadsDone >= huePorts.Length)
                    {
                        threadsDone = 0;
                        AmbiLight.waitHandle.Set();
                    }
                });
            };

            if (AmbiLight.multiThreading)
            {
                Parallel.ForEach(huePorts, WriteBuffer);
            }
            else
            {
                foreach (var huePort in huePorts)
                {
                    WriteBuffer(huePort);
                }
            }
        }
    }
}
