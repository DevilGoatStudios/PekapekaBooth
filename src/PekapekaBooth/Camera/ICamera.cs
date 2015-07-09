using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using System.Drawing;

namespace PekapekaBooth.Camera
{
    public interface ICamera
    {
        // Event triggered when a new frame from camera is ready
        event NewFrameEventHandler NewFrame;

        void SetFrameSize(Size size);
        void Start();
        void CloseVideoSource();
    }
}
