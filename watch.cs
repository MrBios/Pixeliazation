using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixelization
{
    public partial class watch : Form
    {
        public watch(Bitmap inp)
        {
            InitializeComponent();
            pictureBox1.Image = Pixelization.Resize(inp, 15);
        }

        private void watch_Load(object sender, EventArgs e)
        {

        }
    }
}
