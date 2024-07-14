using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixelization
{
    public partial class Form1 : Form
    {
        bool ispic = false;
        Bitmap pic = null;
        string path = "";
        Bitmap pixel = null;
        bool ispixe = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ispic = false;
            ispixe = false;
            pixel = null;
            pic = null;
            pictureBox1.Image = null;
        }

        void render()
        {
            int width = Int32.Parse(textBox2.Text) * 32;
            int height = Int32.Parse(textBox1.Text) * 32;
            switch(comboBox1.SelectedIndex)
            { 
                case 0:
                    pixel = Pixelization.PixelateAverageColor(pic, width, height);
                    break;
                case 1:
                    pixel = Pixelization.PixelateNearestNeighbor(pic, width, height);
                    break;
                case 2:
                    pixel = Pixelization.PixelateGaussianBlur(pic, width, height, Int32.Parse(textBox3.Text));
                    break;
                case 3:
                    pixel = Pixelization.PixelateCross(pic, width, height, Int32.Parse(textBox3.Text));
                    break;
                case 4:
                    pixel = Pixelization.PixelateTriangle(pic, width, height, Int32.Parse(textBox3.Text));
                    break;
            }
            ispixe = true;
            pictureBox1.Image = Pixelization.Resize(pixel, 10);
            MessageBox.Show("Пикселизировал");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ispic)
                return;
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите метод");
                return;
            }
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Не заполнили ширину или высоту");
                return;
            }
            render();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                Image pc = Bitmap.FromFile(openFileDialog1.FileName);
                Bitmap picture = new Bitmap(pc);
                pic = picture;
                ispic = true;
                ispixe = false;
                pixel = null;
                pictureBox1.Image = picture;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!ispixe) return;
            watch wtch = new watch(pixel);
            wtch.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!ispixe) return;
            var res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                pixel.Save(saveFileDialog1.FileName);
                MessageBox.Show("Сохранено");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] nub = { 2, 3, 4 };
            List<int> list = nub.ToList();
            if(list.Contains(comboBox1.SelectedIndex))
            {
                textBox3.Enabled = true;
                switch(comboBox1.SelectedIndex)
                {
                    case 2:
                        label4.Text = "Радиус размытия";
                        break;
                    case 3:
                        label4.Text = "Размер кросса";
                        break;
                    case 4:
                        label4.Text = "Размер треугольников";
                        break;
                }
            }
            else
            {
                textBox3.Enabled = false;
                label4.Text = "Ничего не нужно";
            }
        }
    }
}
