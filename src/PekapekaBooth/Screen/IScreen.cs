using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekapekaBooth.Screen
{
    interface IScreen
    {
        void SetImage(Image image);

        // Show a 5..4..3..2..1..Click! countdown on-screen (5 sec in total)
        void ShowCountdown();
    }
}
