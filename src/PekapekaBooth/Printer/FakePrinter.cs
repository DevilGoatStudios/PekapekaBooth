using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekapekaBooth.Printer
{
    public class FakePrinter : IPrinter
    {
        public event FinishPrintingEventHandler FinishPrinting; // Triggered the printer finish to print

        public void Print(Image imageToPrint)
        {
            if(FinishPrinting != null)
            {
                FinishPrinting();
            }
        }
    }
}
