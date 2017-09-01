using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLightPlus
{
    class HueBuffer
    {
        public byte[] buffer = new byte[125];
        private byte[] readBuffer = new byte[125];
        private const byte magicBit = 75;
        private const byte ledStripsPerChannel = 4; // Always assume max amount of strips is connected or serial stream fails (for now)
        private const byte animationMode = 0;
        private const byte animationDirection = 0;
        private const byte animationOptions = 0;
        private const byte animationGroup = 0;
        private const byte animationSpeed = 2;

        public HueBuffer(byte channel)
        {
            buffer[0] = magicBit;
            buffer[1] = channel;
            buffer[2] = animationMode;
            buffer[3] = animationDirection << 4 | animationOptions << 3 | ledStripsPerChannel;
            buffer[4] = 0 << 5 | animationGroup << 3 | animationSpeed;
        }

        public void WriteAndCallback(SerialPort serialPort, Action Callback)
        {
            try
            {
                serialPort.Write(buffer, 0, buffer.Length);
            }
            catch(Exception e)
            {
                Logger.Add("Tried writing to closed port: " + e);
            }
            
            WaitForBufferReceived(serialPort, Callback);
        }

        private void WaitForBufferReceived(SerialPort serialPort, Action Callback)
        {
            try
            {
                serialPort.BaseStream.BeginRead(readBuffer, 0, readBuffer.Length, (IAsyncResult ar) =>
                {
                    try
                    {
                        serialPort.BaseStream.EndRead(ar);
                    }
                    catch (Exception e)
                    {
                        Logger.Add("Tried reading from closed port: " + e);
                    }
                    finally
                    {
                        Callback();
                    }

                }, null);
            }
            catch (Exception e)
            {
                Logger.Add("Tried reading from closed port: " + e);
                Callback();
            }
        }
    }
}
