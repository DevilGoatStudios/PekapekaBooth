using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;

namespace PekapekaBooth.Camera
{
    interface ICamera
    {
        // Event triggered when a new frame from camera is ready
        event NewFrameEventHandler NewFrame;
    }
}
