using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PekapekaBooth
{
    public partial class Screen : Form
    {
        private CamerasManager mCamera;

        public Screen()
        {
            InitializeComponent();
            mCamera = new CamerasManager();
        }

        

        private void CameraFeed(object sender, NewFrameEventArgs eventArgs)
        {
        }
    }
}
