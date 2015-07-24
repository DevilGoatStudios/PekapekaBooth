using PekapekaBooth;
using System;
using System.Windows.Forms;

namespace PekapekaBooth
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // WinForm ButtonsBox
            Application.Run(new Screen.ScreenForm(new ButtonsBox.FakeButtonsBox(), new Camera.WebCamera(), null));

            // H/W ButtonsBox
            //Application.Run(new Screen.ScreenForm(new ButtonsBox.ButtonsBox(), new Camera.Camera(), null));
        }
    }
}
