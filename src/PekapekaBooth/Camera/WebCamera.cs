using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Timers;

namespace PekapekaBooth.Camera
{
    public class WebCamera : ICamera
    {
        public event NewImageEventHandler NewVideoFrame;
        public event NewImageEventHandler NewPictureImage;

        private VideoCaptureDevice mVideoSource;
        private FilterInfoCollection mCameras;
        private Size mSize;
        private Image mCurrentImage; // Last saved frame
        private Timer mTimerVideoStream;

        public WebCamera()
        {
            mCameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            // Setting up timers for faking video stream
            mTimerVideoStream = new System.Timers.Timer(32);
            mTimerVideoStream.Enabled = false;
            mTimerVideoStream.Elapsed += (o, i) =>
            {
                if (mCurrentImage != null)
                {
                    NewVideoFrame(mCurrentImage);
                }
            };
        }

        public void StartVideo()
        {
            SelectCamera(mCameras[0].MonikerString);
            mTimerVideoStream.Enabled = true;
        }

        public void StopVideo()
        {
            mTimerVideoStream.Enabled = false;
        }

        public bool IsVideoOn()
        {
            return mVideoSource != null && mVideoSource.IsRunning && mTimerVideoStream.Enabled;
        }

        public void TakePicture()
        {
            NewPictureImage(mCurrentImage);
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
            mVideoSource.NewFrame += CameraNewFrame;
            mVideoSource.Start();
        }

        public void Shutdown()
        {
            CloseVideoSource();
        }

        // Event handler
        private void CameraNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            mCurrentImage = (Bitmap)eventArgs.Frame.Clone();
        }

        private void CloseVideoSource()
        {
            if (!(mVideoSource == null))
            {
                if (mVideoSource.IsRunning)
                {
                    mVideoSource.SignalToStop();
                    mVideoSource = null;
                }
            }
        }
    }
}
