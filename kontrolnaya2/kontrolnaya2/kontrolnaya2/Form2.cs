using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace kontrolnaya2
{


    public partial class Form2 : Form
    {

        string item = "";
        private Bitmap MyImage;             //  пользователя

        public Form2(string pcd)
        {
            item = pcd;
            
            InitializeComponent();
             
            
           string[] data_for_split = item.Split(',');
            label1.Text = data_for_split[1];
            textBox1.Text = data_for_split[2]; // идёт имя вместо адреса получателя
            textBox2.Text = data_for_split[3];
            richTextBox1.Text = data_for_split[4];

            Image Picture = null;

            string Pic1 = data_for_split[0];

            if (data_for_split[0] == "Pic1")
            {
                Picture = Properties.Resources.Pic1;
            }

            if (data_for_split[0] == "Pic2")
            {
                Picture = Properties.Resources.Pic2;
            }

            if (data_for_split[0] == "Pic3")
            {
                Picture = Properties.Resources.Pic3;
            }

            ShowMyImage(Picture, 200, 200);  // картинка гостя

        }

        public void ShowMyImage(Image fileToDisplay, int xSize, int ySize)
        {
            // выбор картинки при входк в систему для отображения роли
            if (MyImage != null)
            {
                MyImage.Dispose();
            }

            // расширение картинки на размер pictureBox
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            MyImage = new Bitmap(fileToDisplay);
            pictureBox1.ClientSize = new Size(xSize, ySize);
            pictureBox1.Image = (Image)MyImage;
        }

        void prepareNextForm()
        {

            Form3 frm = new Form3(item);
            frm.Owner = this;
            frm.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            prepareNextForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
    }
}
