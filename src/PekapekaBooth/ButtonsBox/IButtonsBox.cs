using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PekapekaBooth.ButtonsBox
{
    public interface IButtonsBox
    {
        // Events triggered when a button is clicked
        event EventHandler ButtonTakePictureClick;
        event EventHandler ButtonPrintClick;

        // Controls for TakePicture (green) light
        void TurnOnTakePictureLight();
        void TurnOffTakePictureLight();
        void FlashTakePictureLight();

        // Controls for Print (blue) light
        void TurnOnPrintLight();
        void TurnOffPrintLight();
        void FlashPrintLight();
    }
}
