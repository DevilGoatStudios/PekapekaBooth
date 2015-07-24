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
                mDLSRCamera.LiveViewEnabled = true;
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
            if (mDLSRCamera != null)
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
            //NewPictureImage(mCurrentImage);
        }

        public void Shutdown()
        {
            mManager.Shutdown();
        }

        public NikonD5200()
        {
            try
            {
                mManager = new NikonManager("Type0009.md3");
                mManager.DeviceAdded += ManagerDeviceAdded;

                // Setting up timers for faking video stream
                mTimerVideoStream = new System.Timers.Timer(32);
                mTimerVideoStream.Enabled = false;
                mTimerVideoStream.Elapsed += (o, i) =>
                {
                    if (mDLSRCamera != null && IsVideoOn())
                    {
                        NikonLiveViewImage liveViewImage = mDLSRCamera.GetLiveViewImage();

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
                };
            }
            catch (NikonException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //Console.WriteLine(ex.Message);
            }
        }

        private void ManagerDeviceAdded(NikonManager sender, NikonDevice device)
        {
            if (mDLSRCamera == null && device != null)
            {
                mDLSRCamera = device;
                StartVideo();
            }
        }
    }
}
