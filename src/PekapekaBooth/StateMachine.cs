using AForge.Video;
using PekapekaBooth.ButtonsBox;
using PekapekaBooth.Camera;
using PekapekaBooth.Printer;
using PekapekaBooth.Screen;
using System;
using System.Drawing;

namespace PekapekaBooth
{
    public enum State
    {                         //                ----==== Outputs ====----
                                // Button TakePicture | Button Print | Screen
        eIdle,                //    Flashing        |   Off        |  Live video feed
        eCountdown,           //    On              |   Off        |  Live video feed + 5..4..3..2..1..Click!
        ePrintOrReTakePicture,//    Flashing        |   Flashing   |  Still image taken
        ePrinting             //    Off             |   On         |  Still image taken
    };
    class StateMachine
    {
        private State  mCurrentState;
        private Bitmap mCurrentPicture; // Last saved frame

        private IButtonsBox mButtonsBox;
        private ICamera     mCamera;
        private IPrinter    mPrinter;
        private IScreen     mScreen;
        

        public StateMachine(IButtonsBox buttonsBox, ICamera camera, IPrinter printer, IScreen screen)
        {
            mButtonsBox = buttonsBox;
            mCamera     = camera;
            mPrinter    = printer;
            mScreen     = screen;

            mButtonsBox.ButtonTakePictureClick += PressTakePicture;
            mButtonsBox.ButtonPrintClick += PressPrint;
             
            mCamera.NewVideoFrame += CameraNewVideoFrame;
            mCamera.NewPictureImage += CameraPictureImage;
            mScreen.Closing += Closing;

            // Default starting state
            SetStateToIdle();
        }

        private void Closing(object sender, EventArgs e)
        {
            mButtonsBox.TurnOffTakePictureLight();
            mButtonsBox.TurnOffPrintLight();
            mCamera.Shutdown();
        }

        // Event handler
        private void PressTakePicture(object sender, EventArgs e)
        {
            // When pressing "TakePicture" (green button)
            // And we are idle OR waiting to print or re-take, we will switch to countdown state
            if (mCurrentState == State.eIdle || mCurrentState == State.ePrintOrReTakePicture)
            {
                SetStateToTakePictureCountdown();
            }
            // Else we do nothing
        }

        // Event handler
        private void PressPrint(object sender, EventArgs e)
        {
            // When pressing "Print" (blue button)
            // And we are waiting to print or re-take, we will switch to print state
            if (mCurrentState == State.ePrintOrReTakePicture)
            {
                SetStateToPrinting();
            }
            // Else we do nothing
        }

        // Event handler
        private void CameraNewVideoFrame(Image image)
        {
            // When receiving new frame from camera (live stream)
            // We only update screen if we are idle OR Countdown
            if (mCurrentState == State.eIdle || mCurrentState == State.eCountdown)
            {
                mCurrentPicture = (Bitmap)image.Clone();
                mCurrentPicture.RotateFlip(RotateFlipType.Rotate180FlipY);
                mScreen.SetImage(mCurrentPicture);
            }
            // Else we do nothing
        }

        // Event handler
        private void CameraPictureImage(Image image)
        {
            // When receiving new picture from camera (still image)
            // We only update screen if we are in Countdown
            if (mCurrentState == State.eCountdown)
            {
                mCurrentPicture = (Bitmap)image.Clone();
                mCurrentPicture.RotateFlip(RotateFlipType.Rotate180FlipY);
                mScreen.SetImage(mCurrentPicture);

                SetStateToPrintOrReTakePicture();
            }
            // Else we do nothing
        }


        private void SetStateToIdle()
        {
            mButtonsBox.FlashTakePictureLight();
            mButtonsBox.TurnOffPrintLight();

            mCamera.StartVideo();

            mCurrentState = State.eIdle;
        }

        private void SetStateToTakePictureCountdown()
        {
            mButtonsBox.TurnOnTakePictureLight();
            mButtonsBox.TurnOffPrintLight();
            mScreen.ShowCountdown();

            if (!mCamera.IsVideoOn())
            {
                mCamera.StartVideo();
            }

            mCurrentState = State.eCountdown;

            System.Timers.Timer countdown = new System.Timers.Timer(5000);
            countdown.Enabled = true;
            countdown.Elapsed += (o, i) =>
            {
                mCamera.StopVideo();
                mCamera.TakePicture();
                countdown.Enabled = false;
            };
        }

        private void SetStateToPrintOrReTakePicture()
        {
            mButtonsBox.FlashTakePictureLight();
            mButtonsBox.FlashPrintLight();

            mCurrentState = State.ePrintOrReTakePicture;
        }

        private void SetStateToPrinting()
        {
            mButtonsBox.TurnOffTakePictureLight();
            mButtonsBox.TurnOnPrintLight();

            mCurrentState = State.ePrinting;

            mPrinter.Print(mCurrentPicture);

            System.Timers.Timer toto = new System.Timers.Timer(5000);
            toto.Enabled = true;
            toto.Elapsed += (o, i) =>
            {
                SetStateToIdle();
                toto.Enabled = false;
            };
        }
    }
}
