using PekapekaBooth.ButtonsBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PekapekaBooth.Screen
{
    public partial class ScreenForm : Form
    {
        private StateMachine mStateMachine;

        public ScreenForm()
        {
            InitializeComponent();

            // Fake Camera
            mStateMachine = new StateMachine(new FakeButtonsBox(), new Camera.Camera(), new Screen(this));

            // Real Camera
            //mStateMachine = new StateMachine(new WinFormsButtonBox(), new Camera(), new Screen(this));
        }

        public PictureBox GetScreen() { return pictureBox; }

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

    }
}
