using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekapekaBooth.Printer
{
    interface IPrinter
    {
        void Print(Image image);
    }
}
