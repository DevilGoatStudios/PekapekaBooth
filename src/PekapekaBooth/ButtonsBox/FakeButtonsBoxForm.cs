using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PekapekaBooth.ButtonsBox
{
    public partial class FakeButtonsBoxForm : Form
    {
        public FakeButtonsBoxForm()
        {
            InitializeComponent();
        }

        public Button GetButtonTakePicture() { return buttonTakePicture; }
        public Button GetButtonPrint() { return buttonPrint; }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
