using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekapekaBooth.Printer
{
    public class Printer : IPrinter
    {
        public event FinishPrintingEventHandler FinishPrinting;
        private Image mCurrentImage;

        public void Print(Image imageToPrint)
        {
            mCurrentImage = (Image)imageToPrint.Clone();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += this.PrintPage;
            doc.EndPrint += PrintEnd;
            doc.Print();
        }

        void PrintEnd(object sender, PrintEventArgs e)
        {
            if (FinishPrinting != null)
            {
                FinishPrinting();
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                PointF pf = new PointF(10, 10);
                ev.Graphics.DrawImage(mCurrentImage, pf);
            }
            catch (Exception)
            {
            }

        }
    }
}
