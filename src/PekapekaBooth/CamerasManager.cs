using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekapekaBooth
{
    public class CamerasManager
    {
        private VideoCaptureDevice mVideoSource;
        private FilterInfoCollection mCameras;
        private NewFrameEventHandler mFeedHandler; //Wrong!!! Need to be the screen feed handler

        public CamerasManager()
        {
            mCameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            SelectCamera(mCameras[0].MonikerString);
        }

        public FilterInfoCollection getCamList()
        {
            return mCameras;
        }

        public void RefreshCameras()
        {
            mCameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        public void SelectCamera(string camera)
        {
            //Verify if the camera is still valid
            CloseVideoSource();
            mVideoSource = new VideoCaptureDevice(camera);
            mVideoSource.NewFrame += new NewFrameEventHandler(mFeedHandler);
            mVideoSource.DesiredFrameSize = new Size(320, 240); //To be changed
            mVideoSource.Start();
        }

        private void CloseVideoSource()
        {
            if (!(mVideoSource == null))
                if (mVideoSource.IsRunning)
                {
                    mVideoSource.SignalToStop();
                    mVideoSource = null;
                }
        }
    }
}
