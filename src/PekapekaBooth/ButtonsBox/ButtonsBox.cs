using System;
using System.IO.Ports;
using System.Threading;

namespace PekapekaBooth.ButtonsBox
{
    public class ButtonsBox : IButtonsBox
    {
        public event EventHandler ButtonTakePictureClick;
        public event EventHandler ButtonPrintClick;

        private bool mContinue;
        private SerialPort mSerialPort;

        public ButtonsBox()
        {
            //string name;
            //string message;
            //StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            mSerialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            mSerialPort.PortName = SetPortName(mSerialPort.PortName);
            mSerialPort.BaudRate = 9600;
            mSerialPort.Parity = Parity.None;
            mSerialPort.DataBits = 8;
            mSerialPort.StopBits = StopBits.One ;
            mSerialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            mSerialPort.ReadTimeout = 2500;
            mSerialPort.WriteTimeout = 2500;

            mSerialPort.Open();
            Thread.Sleep(2000);
            mContinue = true;
            readThread.Start();
        }            

        //
        // Controls for TakePicture (green) light
        //
        public void TurnOnTakePictureLight()
        {
            mSerialPort.WriteLine("7");
        }
        public void TurnOffTakePictureLight()
        {
            mSerialPort.WriteLine("5");
        }
        public void FlashTakePictureLight()
        {
            mSerialPort.WriteLine("6");
        }

        //
        // Controls for Print (blue) light
        //
        public void TurnOnPrintLight()
        {
            mSerialPort.WriteLine("4");
        }
        public void TurnOffPrintLight()
        {
            mSerialPort.WriteLine("2");
        }
        public void FlashPrintLight()
        {
            mSerialPort.WriteLine("3");
        }


        private void Read()
        {
            while (mContinue)
            {
                try
                {
                    int message = mSerialPort.ReadByte();

                    if (message == 50)
                    {
                        if (ButtonTakePictureClick != null)
                        {
                            ButtonTakePictureClick(this, EventArgs.Empty);
                        }
                    }
                    else if (message == 49)
                    {
                        if (ButtonPrintClick != null)
                        {
                            ButtonPrintClick(this, EventArgs.Empty);
                        }
                    }
                }
                catch (TimeoutException) { }
            }
        }

        private string SetPortName(string defaultPortName)
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                return portName;
            }
            return null;
        }
    }
}
