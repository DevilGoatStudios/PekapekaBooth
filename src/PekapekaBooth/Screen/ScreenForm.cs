using PekapekaBooth.ButtonsBox;
using PekapekaBooth.Camera;
using PekapekaBooth.Printer;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PekapekaBooth.Screen
{
    public partial class ScreenForm : Form,  IScreen
    {
        private StateMachine mStateMachine;

        public event EventHandler Closing;

        public ScreenForm(IButtonsBox buttonsBox, ICamera camera, IPrinter printer)
        {
            InitializeComponent();

            mStateMachine = new StateMachine(buttonsBox, camera, printer, this);
        }

        public void SetImage(Image image)
        {
            //float ratio = (float)Size.Height / (float)image.Size.Height;
            //Size newSize = new Size((int)(ratio * image.Size.Width), (int)(ratio * image.Size.Height));

            pictureBox.Image = (Bitmap)image.Clone();// (Image)(new Bitmap(image, newSize));
        }

        // Show a 5..4..3..2..1..Click! countdown on-screen (5 sec in total)
        public void ShowCountdown()
        {
            // Doing nothing for now
        }

        private void TriggerFullScreen()
        {
            if (FormBorderStyle == FormBorderStyle.None)
            {
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
                WindowState = FormWindowState.Normal;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                TriggerFullScreen();
            }
        }

        private void ClosingEvent(object sender, FormClosingEventArgs e)
        {
            Closing(sender, e);
        }

    }
}
