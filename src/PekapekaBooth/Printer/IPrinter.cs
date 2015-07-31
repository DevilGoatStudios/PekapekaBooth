using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekapekaBooth.Printer
{
    public delegate void FinishPrintingEventHandler();

    public interface IPrinter
    {
        event FinishPrintingEventHandler FinishPrinting; // Triggered the printer finish to print

        void Print(Image imageToPrint);
    }
}
