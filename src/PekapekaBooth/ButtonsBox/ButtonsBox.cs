using System;
using System.IO.Ports;
using System.Threading;

namespace PekapekaBooth.ButtonsBox
{
    public class ButtonsBox : IButtonsBox
    {
        public event EventHandler ButtonTakePictureClick;
        public event EventHandler ButtonPrintClick;

        private SerialPort mSerialPort;

        public ButtonsBox()
        {
            //string name;
            //string message;
            //StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            //Thread readThread = new Thread(Read);

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
            mSerialPort.ReadTimeout = 500;
            mSerialPort.WriteTimeout = 500;

            mSerialPort.Open();
            //continue  = true;
            //readThread.Start();
        }            

        //
        // Controls for TakePicture (green) light
        //
        public void TurnOnTakePictureLight()
        {
        }
        public void TurnOffTakePictureLight()
        {
            mSerialPort.WriteLine("2");
        }
        public void FlashTakePictureLight()
        {
            Thread.Sleep(2000);
            mSerialPort.WriteLine("3");
        }

        //
        // Controls for Print (blue) light
        //
        public void TurnOnPrintLight()
        {
        }
        public void TurnOffPrintLight()
        {
            mSerialPort.WriteLine("5");
        }
        public void FlashPrintLight()
        {
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
