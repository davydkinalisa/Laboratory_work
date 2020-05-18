using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema_VP
{
    public partial class EditHallLayout : Form
    {
        private Form1 m_MainFrm = null;
        string seats_array = "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1";   // обьявление массив мест в зрительном зале
        string[] seats_fromDB = new string[10];
        int seats_fromDB_index = 0;

        public cinemaEntities2 db = new cinemaEntities2();


        public EditHallLayout(Form1 mainFrm)
        {
            InitializeComponent();
            m_MainFrm = mainFrm;
        }

        private void EditHallLayout_Load(object sender, EventArgs e)
        {
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            // no larger than screen size
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            loadDataFromDB();

        }

        void loadDataFromDB ()
        {
            var hall_forList = (from s in db.halls
                                orderby s.hall_name
                                select new { s.hall_name, s.hall_id, s.seats, s.hall_class }).Distinct();

            cmbHallTab1.Items.Clear();
            cmbClass.Items.Clear();
            seats_fromDB_index = 0;
            Array.Clear(seats_fromDB, 0, seats_fromDB.Length);

            foreach (var it in hall_forList)
            {
                cmbHallTab1.Items.Add(it.hall_name);
                cmbClass.Items.Add(it.hall_class);
                Add_to_seats(it.seats);
            }
        }

        void Add_to_seats (string seats)
        {
         //MessageBox.Show(seats);
         seats_fromDB[seats_fromDB_index] = seats;
         seats_fromDB_index++;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int x, y, index, seat_to_change;
            string result = "";

            x = e.X;
            y = e.Y;

            if ((x > 100 && x < 600) && (y > 100 && y < 380))
            {
                x = ((x - 100) / 52);
                y = ((y - 100) / 60);

                //cmbHallTab1.Text = (x + 1) + "," + (y + 1);

                seat_to_change = x + (y * 10);

                index = cmbHallTab1.SelectedIndex;
                clear_seatMap();

                string[] numbers = seats_array.Split(',');

                if (numbers[seat_to_change] == "5")
                {
                    numbers[seat_to_change] = "1";
                } 
                else
                {
                    numbers[seat_to_change] = "5";
                }

                result = numbers[0];

                for (int i = 1; i < 50; i++)
                {
                    result += ("," + numbers[i]);
                }

                seats_array = result;
                draw_seatMap(seats_array);
            }
        }

        void clear_seatMap()
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White); //рисуем белый прямоугольник по всему полю нашей карты
            System.Drawing.Graphics formGraphics;
            formGraphics = pictureBox1.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(50, 50, 600, 600)); //начало координат(50,50) с размерами 600*600 пикселей
            myBrush.Dispose();
            formGraphics.Dispose();  //удаляем обьект рисования
        }

        /* принцип кодирования мест: 0-нет места 1-место не занято, 2-место занято, 3- диванчик, 4- диванчик занятый */
        void draw_seatMap(string Map)
        {
            int i = 0;
            string[] seat = Map.Split(',');

            Graphics g = pictureBox1.CreateGraphics();

            g.DrawRectangle(Pens.Red, 175, 50, 350, 20);    // рисуем экран зрительного зала

            for (int y = 100; y < 400; y += 60) //рисуем места
            {

                for (int x = 100; x < 600; x += 50)
                {

                    if (seat[i] == "1")
                    {
                        g.DrawRectangle(Pens.Green, x, y, 40, 40);
                    }

                    if (seat[i] == "2")
                    {
                        g.DrawRectangle(Pens.Red, x, y, 40, 40);
                    }

                    if (seat[i] == "5") // это место выбрано для оформления билета 
                    {
                        g.DrawRectangle(Pens.Red, x, y, 40, 40);
                        g.DrawRectangle(Pens.Red, x + 3, y + 3, 34, 34);
                    }

                    if (seat[i] == "3")
                    {
                        g.DrawRectangle(Pens.Green, x, y, 90, 40);
                        x += 50;
                        i++;
                    }

                    if (seat[i] == "4")
                    {
                        g.DrawRectangle(Pens.Red, x, y, 90, 40);
                        x += 50;
                        i++;
                    }

                    if (seat[i] == "6") // выбранное место диванчика для оформления билета
                    {
                        g.DrawRectangle(Pens.Red, x, y, 90, 40);
                        g.DrawRectangle(Pens.Red, x + 3, y + 3, 84, 34);
                        x += 50;
                        i++;
                    }

                    i++;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // запрашиваем подтверждение на запись зала
            DialogResult dialogResult = MessageBox.Show("Сохранить изменения?", "Добавление / редактирование зала", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                // создание нового зала
                addOrUpdateNewHallToDb(cmbHallTab1.Text);
            }

            loadDataFromDB();
        }

        private void addOrUpdateNewHallToDb (string hallName)
        {
            string[] seats_forProcess = seats_array.Split(',');
            string result = "";

            //MessageBox.Show(hallName);

            // запрашиваем в БД имя зала и если он уже существует то изменяем то что если иначе создаём новый зал
            try
            {
                var hall_forList = (from hall in db.halls
                                    where (hall.hall_name == hallName)
                                    select hall.hall_id).Distinct();

                //MessageBox.Show(hall_forList.First().ToString());

                int hall_for_update = hall_forList.First();

                for (int i = 0; i < 50; i++)
                {
                    if (seats_forProcess[i] == "1")
                    {
                        seats_forProcess[i] = "0";
                    }

                    if (seats_forProcess[i] == "5")
                    {
                        seats_forProcess[i] = "1";
                    }
                }

                result = seats_forProcess[0];

                for (int i = 1; i < 50; i++)
                {
                    result += ("," + seats_forProcess[i]);
                }

                db.Configuration.ValidateOnSaveEnabled = false;
                var verifcation = db.halls.Where(x => x.hall_id == hall_for_update).FirstOrDefault();
                verifcation.hall_name = hallName;
                verifcation.hall_id = hall_for_update;
                verifcation.seats = result;
                //verifcation.class = class_for_update;
                MessageBox.Show("Зал обновлён!");
            }
            catch
            {

                for (int i = 0; i < 50; i++)
                {
                    if (seats_forProcess[i] == "1")
                    {
                        seats_forProcess[i] = "0";
                    }

                    if (seats_forProcess[i] == "5")
                    {
                        seats_forProcess[i] = "1";
                    }
                }

                result = seats_forProcess[0];

                for (int i = 1; i < 50; i++)
                {
                    result += ("," + seats_forProcess[i]);
                }

                int number_of_halls = db.halls.Max(n => n.hall_id) + 1;

                hall new_hall = new hall
                {
                    hall_name = hallName,
                    hall_id = number_of_halls,
                    seats = result,
                    hall_class = cmbClass.Text,
                };

                db.halls.Add(new_hall);
                MessageBox.Show("Зал добавлен");
            }

            db.SaveChanges();
        }


        private void cmbHallTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index;
            string result = "";

            index = cmbHallTab1.SelectedIndex;
            clear_seatMap();

            string[] seats_forProcess = seats_fromDB[index].Split(',');
            
            for (int i = 0; i < 50; i++)
            {
                if (seats_forProcess[i] == "1" || seats_forProcess[i] == "2")
                {
                    seats_forProcess[i] = "5";
                }

                if (seats_forProcess[i] == "0")
                {
                    seats_forProcess[i] = "1";
                }
            }

            result = seats_forProcess[0];
            
            for (int i = 1; i < 50; i++)
            {
                result += ("," + seats_forProcess[i]);
            }

            //MessageBox.Show(result);
            draw_seatMap(result);
            seats_array = result;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }


}
