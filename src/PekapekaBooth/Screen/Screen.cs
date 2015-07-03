using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace PekapekaBooth.Screen
{
    class Screen : IScreen
    {
        private ScreenForm mScreenForm;

        public Screen(ScreenForm screenForm)
        {
            mScreenForm = screenForm;
        }

        public void SetImage(Image image)
        {
            mScreenForm.GetScreen().Image = image;
        }

        // Show a 5..4..3..2..1..Click! countdown on-screen (5 sec in total)
        public void ShowCountdown()
        {
            // Doing nothing for now
        }
    }
}
