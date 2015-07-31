using Nikon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Imaging;

namespace PekapekaBooth.Camera
{
    public class NikonD5200 : ICamera
    {
        public event NewImageEventHandler NewVideoFrame;
        public event NewImageEventHandler NewPictureImage;

        private NikonManager mManager;
        private NikonDevice  mDLSRCamera;

        private NikonLiveViewImage mCurrentImage; // Last saved frame
        private Timer mTimerVideoStream;

        public void SetFrameSize(Size size)
        {
            //mSize = size;
        }

        public void StartVideo()
        {
            if (mDLSRCamera != null)
            {
                try
                {
                    mDLSRCamera.LiveViewEnabled = true;
                }
                catch (NikonException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    mManager.Shutdown();
                    mManager = null;
                }
            }
            mTimerVideoStream.Enabled = true;
        }

        public void StopVideo()
        {
            if (mDLSRCamera != null)
            {
                mDLSRCamera.LiveViewEnabled = false;
            }
            mTimerVideoStream.Enabled = false;
        }

        public bool IsVideoOn()
        {
            if (mManager != null && mDLSRCamera != null)
            {
                return mDLSRCamera.LiveViewEnabled && mTimerVideoStream.Enabled;
            }
            else
            {
                return false;
            }
        }

        public void TakePicture()
        {
            if (mManager != null && mDLSRCamera != null)
            {
                mDLSRCamera.Capture();
            }
        }

        public void Shutdown()
        {
            mManager.Shutdown();
            mManager = null;
            mDLSRCamera = null;
            mTimerVideoStream.Enabled = false;
        }

        public NikonD5200()
        {
            try
            {
                mManager = new NikonManager("Type0009.md3");
                mManager.DeviceAdded   += ManagerDeviceAdded;
                mManager.DeviceRemoved += ManagerDeviceRemoved;

                // Setting up timers for video stream
                // Every 60 ms, we will poke the Nikon for a new video frame
                mTimerVideoStream = new System.Timers.Timer(60);
                mTimerVideoStream.Enabled = false;
                mTimerVideoStream.Elapsed += TimerVideoStreamElapsed;
            }
            catch (NikonException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        // Event handler
        private void ManagerDeviceAdded(NikonManager sender, NikonDevice device)
        {
            if (mManager != null && device != null)
            {
                mDLSRCamera = device;
                device.ImageReady += device_ImageReady;
                StartVideo();
            }
        }

        // Event handler
        private void ManagerDeviceRemoved(NikonManager sender, NikonDevice device)
        {
            if (mManager != null && device != null)
            {
                StopVideo();
                mDLSRCamera = null;
            }
        }

        // Event handler
        private void TimerVideoStreamElapsed(object sender, ElapsedEventArgs e)
        {
            if (mManager != null && mDLSRCamera != null && IsVideoOn())
            {
                NikonLiveViewImage liveViewImage = null;

                try
                {
                    liveViewImage = mDLSRCamera.GetLiveViewImage();
                }
                catch (NikonException ex)
                {
                    System.Windows.Forms.MessageBox.Show("Failed to get live view image: " + ex.ToString());
                }

                if (liveViewImage != null)
                {
                    JpegBitmapDecoder decoder = new JpegBitmapDecoder(new MemoryStream(liveViewImage.JpegBuffer), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

                    BitmapFrame frame = decoder.Frames[0];

                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();

                        enc.Frames.Add(BitmapFrame.Create(frame));
                        enc.Save(outStream);
                        Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                        NewVideoFrame(bitmap);
                    }
                }
            }
        }
        // Event handler
        private void device_ImageReady(NikonDevice sender, NikonImage image)
        {
            if (mManager != null && mDLSRCamera != null && image  != null && image.Type == NikonImageType.Jpeg)
            {
                JpegBitmapDecoder decoder = new JpegBitmapDecoder(new MemoryStream(image.Buffer), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

                BitmapFrame frame = decoder.Frames[0];

                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();

                    enc.Frames.Add(BitmapFrame.Create(frame));
                    enc.Save(outStream);
                    Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                    NewPictureImage(bitmap);
                }
            }
        }
    }
}
