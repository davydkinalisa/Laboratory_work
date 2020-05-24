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

    public partial class Form3 : Form
    {
        string item = "";
        private Bitmap MyImage;             //  пользователя
        public postcardEntities db = new postcardEntities();
        string[] data_for_split;

        public Form3(string pcd)
        {
            InitializeComponent();
            item = pcd;

            data_for_split = item.Split(',');
            label1.Text = data_for_split[1];    // 
            label1.Text = "Открытка отправлена по адресу" + data_for_split[2];
            textBox2.Text = data_for_split[3];
            //richTextBox1.Text = data_for_split[4];

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

        private void Form3_Load(object sender, EventArgs e)
        {

            int number_of_postcards = 0;

            try
            {

                 number_of_postcards = db.postcards.Max(n => n.postcard_id); // получаем количество записей в таблице фильмы
            }
            catch
            {
                number_of_postcards = 1;
            }


            // запись данных в БД
            postcard new_postcard = new postcard
            {
                postcard_id = number_of_postcards,
                picname = data_for_split[0],
                senser_email = data_for_split[1],
                receiver_email = data_for_split[2],
                //Text_for_postcard = data_for_split[3],

            };

            db.SaveChanges();

        }
    }
}
