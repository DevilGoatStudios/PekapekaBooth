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
            Application.Run(new Screen.ScreenForm(new ButtonsBox.FakeButtonsBox(), new Camera.Camera(), null));
        }
    }
}
