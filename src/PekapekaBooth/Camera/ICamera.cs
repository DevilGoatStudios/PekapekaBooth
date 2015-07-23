using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using System.Drawing;

namespace PekapekaBooth.Camera
{
    public delegate void NewImageEventHandler(Image image);

    public interface ICamera
    {
        event NewImageEventHandler NewVideoFrame;   // Triggered when a new frame from the video camera feed is ready
        event NewImageEventHandler NewPictureImage; // Triggered when a still picture from the camera is taken

        void SetFrameSize(Size size);

        void StartVideo();
        void StopVideo();
        bool IsVideoOn();

        void TakePicture();

        void Shutdown();
    }
}
