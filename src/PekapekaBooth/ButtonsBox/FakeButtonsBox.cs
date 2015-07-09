using System;
using System.Timers;

namespace PekapekaBooth.ButtonsBox
{
    public class FakeButtonsBox : IButtonsBox
    {
        public event EventHandler ButtonTakePictureClick;
        public event EventHandler ButtonPrintClick;

        private System.Windows.Forms.Button mButtonTakePicture;
        private System.Windows.Forms.Button mButtonPrint;

        private Timer mTimerTakePicture;
        private Timer mTimerPrint;

        public FakeButtonsBox()
        {
            FakeButtonsBoxForm fakeBoxButtonsForm = new FakeButtonsBoxForm();
            fakeBoxButtonsForm.Show();

            mButtonTakePicture = fakeBoxButtonsForm.GetButtonTakePicture();
            mButtonPrint       = fakeBoxButtonsForm.GetButtonPrint();

            // Setting up buttons clicking event
            mButtonTakePicture.Click += (o, i) =>
            {
            if (ButtonTakePictureClick != null)
            {
                ButtonTakePictureClick(this, EventArgs.Empty);
            }
            };
            mButtonPrint.Click += (o, i) =>
            {
            if (ButtonPrintClick != null)
            {
                ButtonPrintClick(this, EventArgs.Empty);
            }
            };

            // Setting up timers for flashing lights
            mTimerTakePicture = new System.Timers.Timer(500);
            mTimerTakePicture.Enabled = false;
            mTimerTakePicture.Elapsed += (o, i) =>
            {
            if (mButtonTakePicture.BackColor == System.Drawing.Color.Green)
            {
                mButtonTakePicture.BackColor = System.Drawing.Color.White;
            }
            else
            {
                mButtonTakePicture.BackColor = System.Drawing.Color.Green;
            }
            };

            mTimerPrint = new System.Timers.Timer(500);
            mTimerPrint.Enabled = false;
            mTimerPrint.Elapsed += (o, i) =>
            {
            if (mButtonPrint.BackColor == System.Drawing.Color.Blue)
            {
                mButtonPrint.BackColor = System.Drawing.Color.White;
            }
            else
            {
                mButtonPrint.BackColor = System.Drawing.Color.Blue;
            }
            };
        }

        //
        // Controls for TakePicture (green) light
        //
        public void TurnOnTakePictureLight()
        {
            mTimerTakePicture.Enabled = false;
            mButtonTakePicture.BackColor = System.Drawing.Color.Green;
        }
        public void TurnOffTakePictureLight()
        {
            mTimerTakePicture.Enabled = false;
            mButtonTakePicture.BackColor = System.Drawing.Color.White;
        }
        public void FlashTakePictureLight()
        {
            TurnOffTakePictureLight();
            mTimerTakePicture.Enabled = true;
        }

        //
        // Controls for Print (blue) light
        //
        public void TurnOnPrintLight()
        {
            mTimerPrint.Enabled = false;
            mButtonPrint.BackColor = System.Drawing.Color.Blue;
        }
        public void TurnOffPrintLight()
        {
            mTimerPrint.Enabled = false;
            mButtonPrint.BackColor = System.Drawing.Color.White;
        }
        public void FlashPrintLight()
        {
            TurnOffPrintLight();
            mTimerPrint.Enabled = true;
        }
    }
}
