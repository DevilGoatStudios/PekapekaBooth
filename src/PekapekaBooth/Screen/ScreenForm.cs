using PekapekaBooth.ButtonsBox;
using PekapekaBooth.Camera;
using PekapekaBooth.Printer;
using System.Drawing;
using System.Windows.Forms;

namespace PekapekaBooth.Screen
{
    public partial class ScreenForm : Form,  IScreen
    {
        private StateMachine mStateMachine;

        public ScreenForm(IButtonsBox buttonsBox, ICamera camera, IPrinter printer)
        {
            InitializeComponent();

            mStateMachine = new StateMachine(buttonsBox, camera, printer, this);
        }

        public void SetImage(Image image)
        {
            pictureBox.Image = image;
        }

        // Show a 5..4..3..2..1..Click! countdown on-screen (5 sec in total)
        public void ShowCountdown()
        {
            // Doing nothing for now
        }
    }
}
