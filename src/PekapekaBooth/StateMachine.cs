﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using PekapekaBooth.ButtonsBox;
using PekapekaBooth.Camera;
using PekapekaBooth.Printer;
using PekapekaBooth.Screen;

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
        private State mCurrentState;
        private Bitmap mCurrentPicture; // Last saved frame

        private IButtonsBox mButtonsBox;
        private ICamera mCamera;
        private IScreen mScreen;
        private IPrinter mPrinter;

        public StateMachine(IButtonsBox buttonsBox, ICamera camera, IScreen screen)
        {
            mButtonsBox = buttonsBox;
            mCamera = camera;
            mScreen = screen;

            mButtonsBox.ButtonTakePictureClick += PressTakePicture;
            mButtonsBox.ButtonPrintClick += PressPrint;

            mCamera.NewFrame += CameraNewFrame;

            // Default starting state
            SetStateToIdle();
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
        private void CameraNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // When receiving new frame from camera (live stream)
            // We only update screen if we are idle OR waiting to print or re-take
            if (mCurrentState == State.eIdle || mCurrentState == State.eCountdown)
            {
                mCurrentPicture = (Bitmap)eventArgs.Frame.Clone();
                mScreen.SetImage(mCurrentPicture);
            }
            // Else we do nothing
        }

        private void SetStateToIdle()
        {
            mButtonsBox.FlashTakePictureLight();
            mButtonsBox.TurnOffPrintLight();

            mCurrentState = State.eIdle;
        }

        private void SetStateToTakePictureCountdown()
        {
            mButtonsBox.TurnOnTakePictureLight();
            mButtonsBox.TurnOffPrintLight();
            mScreen.ShowCountdown();

            mCurrentState = State.eCountdown;

            System.Timers.Timer toto = new System.Timers.Timer(5000);
            toto.Enabled = true;
            toto.Elapsed += (o, i) =>
            {
                SetStateToPrintOrReTakePicture();
                toto.Enabled = false;
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