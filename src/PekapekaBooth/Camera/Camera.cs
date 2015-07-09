using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;

namespace PekapekaBooth.Camera
{
    public class Camera : ICamera
    {

        public event NewFrameEventHandler NewFrame;

        private VideoCaptureDevice mVideoSource;
        private FilterInfoCollection mCameras;
        private Size mSize;
        

        public Camera()
        {
            mCameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        public void Start()
        {
            SelectCamera(mCameras[0].MonikerString);
        }

        public void SetFrameSize(Size size)
        {
            mSize = size;
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
            mVideoSource.NewFrame += new NewFrameEventHandler(NewFrame);
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
