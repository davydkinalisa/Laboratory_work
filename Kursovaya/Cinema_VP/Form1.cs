using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace Cinema_VP
{
    public partial class Form1 : Form
    {
        int couner = 0;                     //счётчик морганий label
        private Bitmap MyImage;             // картинка пользователя
        private string roleIndex ="null";   // Роль= Администратор, Пользователь, Управляюший
        public cinemaEntities2 db = new cinemaEntities2(); 
        public List<role> roleSheet;        //список ролей для вхождения в систему
        string[] seats_array = new string[10];               // обьявление массив мест в зрительном зале
        int seats_array_index = 0;

        private EditHallLayout ehl = null; //обявляем новую форму для передачи данных

        public Form1()
        {
            InitializeComponent();
            ShowMyImage(Properties.Resources.User_Guest, 200, 200);  //image гостя 
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            int movID = 0;
            int shoID = 0;
            string result = "";

            //запрос movie ID по его названию 

            var movieID = (from movie in db.movies
                           where (movie.title == cmbTitleFilmTab1.Text)
                           select new { movie.movieid }).Distinct();

            foreach (var it in movieID)
            {
                movID = it.movieid;
            }

            //запрос showtimeID по фильму
            var query = (from showtime in db.showtimes
                         orderby showtime.movieid
                         where (showtime.time.ToString() == cmbTimeTab1.Text.ToString() && showtime.movieid == movID)
                         select new {showtime.showtime_id }).Distinct();

            foreach (var it in query)
            {
                shoID = it.showtime_id;
            }

             

            int number_of_tickets = db.tickets.Max(n => n.ticket_id) + 1; //ищем макс ID билета из существ., чтобы записать следующий билет

            //получаем hall_ID из названия зала
            var query_hall = (from g in db.halls
                                where g.hall_name == cmbHallTab1.Text
                                select g.hall_id).Distinct();

            int hall_ID = query_hall.First();

            // помечаем место как занятое в базе данных
            string[] seat_for_split = cmbSeatTab1.Text.Split(',');
            int seat_to_DB = ((int.Parse(seat_for_split[0]))-1) + (int.Parse(seat_for_split[1])-1) * 10;

            string[] seats_forProcess = seats_array[0].Split(',');  // получаем карту зала которую хотим обновить

            //int hall_for_update = hall_forList.First();
            seats_forProcess[seat_to_DB] = "2";

            result = seats_forProcess[0];

            for (int i = 1; i < 50; i++)
            {
                result += ("," + seats_forProcess[i]);
            }

            //запись билета в бд
            ticket new_ticket = new ticket
            {
                ticket_id = number_of_tickets,
                price = int.Parse(lblPrice.Text.Remove(3, 3)),
                seat = cmbSeatTab1.Text,
                showtime_id = shoID,
                hall_id = hall_ID,
                role = roleIndex,
            };

            db.tickets.Add(new_ticket);

            db.Configuration.ValidateOnSaveEnabled = false;
            var verifcation = db.hall_showtime.Where(x => x.showtime_id == shoID && x.hall_id == hall_ID).FirstOrDefault();
            verifcation.seat_map = result;
            //MessageBox.Show("Зал обновлён! " + hall_ID.ToString() + "  " + seat_to_DB.ToString());


            db.SaveChanges();

            seats_array[0] = result;
            
            clear_seatMap(tabPage1);
            draw_seatMap(seats_array[0], tabPage1);

            MessageBox.Show("Данные записаны!");
        }

           
        void clear_seatMap(TabPage tabPage )
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White); //рисуем белый прямоугольник по всему полю нашей карты
            System.Drawing.Graphics formGraphics; 
            formGraphics = tabPage.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(50, 50, 600, 600)); //начало координат(50,50) с размерами 600*600 пикселей
            myBrush.Dispose(); 
            formGraphics.Dispose();  //удаляем обьект рисования
        }

        /* принцип кодирования мест: 0-нет места 1-место не занято, 2-место занято, 3- диванчик, 4- диванчик занятый */
        void draw_seatMap (string Map, TabPage Tab_Page )
        {
            int i = 0;
            string[] seat = Map.Split(',');

            Graphics g = Tab_Page.CreateGraphics();

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
                        g.DrawRectangle(Pens.Red, x+3, y+3, 34, 34);
                        //x += 50;
                        //i++;
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
        //бронирование билета
        private void btnBookTicketTab1_Click(object sender, EventArgs e)
        {

            int movID = 0;
            var movieID = (from movie in db.movies
                           where (movie.title == cmbTitleFilmTab1.Text)
                           select new { movie.movieid }).Distinct();

            foreach (var it in movieID)
            {
                movID = it.movieid;
            }

            // ID сеанса из времени сеанса для записи билета
            var query = (from showtime in db.showtimes
                         orderby showtime.movieid
                         where (showtime.time.ToString() == cmbTimeTab1.Text.ToString() && showtime.movieid == movID)
                         select new { showtime.showtime_id }).Distinct();

            foreach (var it in query)
            {
                movID = it.showtime_id;

            }

            int number_of_tickets = db.tickets.Max(n => n.ticket_id) + 1; //Запись с max ID билета,чтобы записать новый с ID на 1 больше

            ticket new_ticket = new ticket
            {
                ticket_id = number_of_tickets,
                price = 0,
                seat = cmbSeatTab1.Text,
                showtime_id = movID,
                role = roleIndex,
            };

            db.tickets.Add(new_ticket);
            db.SaveChanges();
            MessageBox.Show("Данные записаны!");

        }

        private void cmbHallTab2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
             // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

            // no larger than screen size
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);           
            
            load_data();
        }

        void Add_to_seats(string seats)
        {
            //MessageBox.Show(seats);
            seats_array[seats_array_index] = seats;
            seats_array_index++;

        }


        void load_data()
        {
            // Подготовка свободных мест в зале 
            var hall_forList = (from s in db.halls
                                orderby s.hall_name
                                select new {s.hall_name, s.hall_id, s.seats, s.hall_class }).Distinct();

            cmbHallTab1.Items.Clear();
            cmbHallTab2.Items.Clear();
            cmbHallTab3.Items.Clear();

            seats_array_index = 0;
            Array.Clear(seats_array, 0, seats_array.Length);

            foreach (var it in hall_forList)
            {
                cmbHallTab1.Items.Add(it.hall_name);
                cmbHallTab2.Items.Add(it.hall_name);
                cmbHallTab3.Items.Add(it.hall_name);
                //cmbClass.Items.Add(it.hall_class);
                Add_to_seats(it.seats);
            }


            cmbHallTab2.DrawMode = DrawMode.OwnerDrawFixed;   // for test

            var movie_forList = (from s in db.movies
                                 orderby s.title
                                 select s.title).Distinct();

            foreach (string it in movie_forList)
            {
                cmbTitleFilmTab1.Items.Add(it);
                cmbTitleFilmTab2.Items.Add(it);
                cmbTitleFilmTab3.Items.Add(it);
            }

            var time_forList = (from s in db.showtimes
                                orderby s.time
                                select s.time).Distinct();

            foreach (var it in time_forList)
            {
                //cmbTimeTab1.Items.Add(it.Remove(5,3));
                //cmbTimeTab2.Items.Add(it.Remove(5,3));
                cmbTimeTab1.Items.Add(it.ToString());
                cmbTimeTab2.Items.Add(it.ToString());
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm:ss") ;
        }
        

        private void btnBuyTicket_Click(object sender, EventArgs e)
        {
            if (roleIndex == "Администратор" || roleIndex == "Пользователь")
            {
                tabControl1.SelectedTab = tabPage1;
            }
            else
            {
                loginRequired();
            }
        }

        private void btnRetTicket_Click(object sender, EventArgs e)
        {
            if (roleIndex == "Администратор" || roleIndex == "Пользователь")
            {
                tabControl1.SelectedTab = tabPage5;
            }
            else
            {
                loginRequired();

            }
        }

        private void btnBookRet_Click(object sender, EventArgs e)
        {
            if (roleIndex == "Администратор" || roleIndex == "Пользователь")
            {
                tabControl1.SelectedTab = tabPage5;
            }
            else
            {
                loginRequired();

            }
        }

        void loginRequired ()
        {
            tabControl1.SelectedTab = tabPage4;
            Timer_errUser_event.Enabled = true;

        }

        private void Timer_errUser_event_Tick(object sender, EventArgs e) //моргание label при необходимости войти в систему 
        {
 
            if (this.label14.ForeColor == SystemColors.ControlText)
            {
                label14.ForeColor = System.Drawing.Color.Red;
                tabPage4.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                label14.ForeColor = SystemColors.ControlText;
                tabPage4.ForeColor = SystemColors.ControlText;
            }

            if (couner++ == 5) //кол-во морганий label краным цветом
            {
                Timer_errUser_event.Enabled = false;
                couner = 0;
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            tabPage1.Enabled = false;
            txbGenreTab3.Enabled = false;
            tabPage5.Enabled = false;
            btnLoginEnter.Enabled = true;
            label14.Text = "Вход не выполнен - возможен только промотр";
            ShowMyImage(Properties.Resources.User_Guest, 200, 200);
            clear_userInput();
            this.Text = "Заказ билета в кинотеатр - Гость";
            loginFormLock(false);
            roleIndex = "null";
        }

        void loginFormLock(bool action)
        {
            if (action == true)
            {
                textBox_Login.Enabled = false;
                textBox_Pass.Enabled = false;
                btnLoginEnter.Enabled = false;
            }
            
            else
            {
                textBox_Login.Enabled = true ;
                textBox_Pass.Enabled = true;
                btnLoginEnter.Enabled = true;
                textBox_Login.Clear();
                textBox_Pass.Clear();
            }

        }

        public (string, string)  loginFunc ()
        {
            var temp_login = (from r in db.roles
                             select new { r.role1 , r.login, r.password}).ToList();

            foreach (var item in temp_login.OrderBy(o => o.role1))
            {
                if (textBox_Login.Text == item.login && textBox_Pass.Text == item.password)
                {
                    return (item.role1.ToString(), item.login.ToString());
                }

            }

            return ("failed", "no data");    // failed attempt to login
        }

        public void btnLoginEnter_Click(object sender, EventArgs e)
        {
            var values = loginFunc();

            if (values.Item2 == "admin")
            {
                tabPage1.Enabled = true;
                txbGenreTab3.Enabled = true;
                tabPage5.Enabled = true;
                label14.Text = "Вход выполнен как " +  values.Item1;
                this.Text = "Заказ билета в кинотеатр - " +  values.Item1;
                tabControl1.SelectedTab = tabPage1;
                ShowMyImage(Properties.Resources.User_Admin, 200, 200);
                loginFormLock(true);
                MessageBox.Show("Вы вошли как " + values.Item1);
                roleIndex = "Администратор";
            }

            else if (values.Item2 == "user")
            {
                tabPage1.Enabled = true;
                tabPage5.Enabled = true;
                label14.Text = "Вход выполнен как " + values.Item1 + " Внесение данных невозможно";
                this.Text = "Заказ билета в кинотеатр - " + values.Item1;
                tabControl1.SelectedTab = tabPage1;
                ShowMyImage(Properties.Resources.User_User, 200, 200);
                loginFormLock(true);
                MessageBox.Show("Вы вошли как " + values.Item1);
                roleIndex = "Пользователь";
            }
            
            else if (values.Item2 == "user1")
            {
                txbGenreTab3.Enabled = true;
                label14.Text = "Вход выполнен как " + values.Item1 + " Доступно только внесение данных";
                this.Text = "Заказ билета в кинотеатр - " + values.Item1;
                tabControl1.SelectedTab = txbGenreTab3;
                ShowMyImage(Properties.Resources.User_User1, 200, 200); 
                loginFormLock(true);
                MessageBox.Show("Вы вошли как " + values.Item1);
                roleIndex = "Управляющий";
            }

            else
            {
                label_wrongLogin.Visible = true;
                textBox_Login.BackColor = System.Drawing.Color.Red;
                textBox_Pass.BackColor = System.Drawing.Color.Red;
            }

        }

        private void textBox_Login_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Login.BackColor != SystemColors.Window)
            {
                clear_userInput();
            }

        }

        private void textBox_Pass_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Pass.BackColor != SystemColors.Window)
            {
                //clear_userInput();
                textBox_Pass.Clear();
                label_wrongLogin.Visible = false;
                textBox_Pass.BackColor = SystemColors.Window;
                textBox_Login.BackColor = SystemColors.Window;
            }
        }


        public void clear_userInput ()
        {
            textBox_Login.Clear();
            textBox_Pass.Clear();
            label_wrongLogin.Visible = false;
            textBox_Pass.BackColor = SystemColors.Window;
            textBox_Login.BackColor = SystemColors.Window;
        }

        public void ShowMyImage(Image fileToDisplay, int xSize, int ySize)
        {
            // Sets up an image object to be displayed.
            if (MyImage != null)
            {
                MyImage.Dispose();
            }

            // Stretches the image to fit the pictureBox.
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            MyImage = new Bitmap(fileToDisplay); 
            pictureBox1.ClientSize = new Size(xSize, ySize);
            pictureBox1.Image = (Image)MyImage;
        }

        private void cmbTitleFilmTab2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] sh_ID = new int[10];
            int i = 0;
            int temp = 0;

            // запрос поиска с отбором по названию фильма
            var query = (from movie in db.movies
                         join st in db.showtimes on movie.movieid equals st.movieid
                         orderby movie.title
                         where (movie.title == cmbTitleFilmTab2.Text.ToString())
                         select new { movie.title, movie.duration, movie.director, movie.genre, movie.cast_movie, st.time, st.showtime_id }).Distinct();
            //var temp = query.Where(p => (p.title.ToString() == cmbTitleFilmTab2.Text.ToString()));

            cmbTimeTab2.Items.Clear();
            cmbHallTab2.Items.Clear();

            // заполнение combobox о фильме           
            foreach (var item in query.OrderBy(o => o.title))
            {
                i++;
                sh_ID[i] = item.showtime_id;
                cmbTimeTab2.Items.Add(item.time.ToString());
            }

            while(i > 0)
            {
                temp = sh_ID[i];

                var query1 = (from g in db.hall_showtime
                              where g.showtime_id == temp
                              select g.hall_id).Distinct();

                var query2 = (from g in db.halls
                              where g.hall_id == query1.FirstOrDefault()
                              select g.hall_name).Distinct();

                foreach (string item in query2)
                {
                    cmbHallTab2.Items.Add(item);
                }

                i--;
            }
        }

        private void cmbTimeTab2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] sh_ID = new int[10];
            int i = 0;
            int temp = 0;

            // запрос поиска с отбором по названию фильма
            var query = (from movie in db.movies
                         join st in db.showtimes on movie.movieid equals st.movieid
                         orderby movie.title
                         where (st.time.ToString() == cmbTimeTab2.Text.ToString())
                         select new {movie.title, movie.duration, movie.director, movie.genre, movie.cast_movie, st.time, st.showtime_id }).Distinct();
            //var temp = query.Where(p => (p.title.ToString() == cmbTitleFilmTab2.Text.ToString()));

            cmbTitleFilmTab2.Items.Clear();
            cmbHallTab2.Items.Clear();

            // заполнение combobox о фильме           
            foreach (var item in query.OrderBy(o => o.title))
            {
                i++;
                sh_ID[i] = item.showtime_id;
                cmbTitleFilmTab2.Items.Add(item.title.ToString());
            }

            while (i > 0)
            {
                temp = sh_ID[i];

                var query1 = (from g in db.hall_showtime
                              where g.showtime_id == temp
                              select g.hall_id).Distinct();

                var query2 = (from g in db.halls
                              where g.hall_id == query1.FirstOrDefault()
                              select g.hall_name).Distinct();

                foreach (string item in query2)
                {
                    cmbHallTab2.Items.Add(item);
                }

                i--;
            }
        }

        private void cmbTitleFilmTab1_SelectedIndexChanged(object sender, EventArgs e)
        {

            int[] sh_ID = new int[10];
            int i = 0;
            int temp = 0;

            // запрос поиска с отбором по названию фильма
            var query = (from movie in db.movies
                         join st in db.showtimes on movie.movieid equals st.movieid
                         orderby movie.title
                         where (movie.title == cmbTitleFilmTab1.Text.ToString())
                         select new { movie.title, movie.duration, movie.director, movie.genre, movie.cast_movie, st.time, st.showtime_id, st.price , movie.year_movie }).Distinct();
            //var temp = query.Where(p => (p.title.ToString() == cmbTitleFilmTab2.Text.ToString()));

            cmbTimeTab1.Items.Clear();
            cmbHallTab1.Items.Clear();

            // заполнение combobox о фильме           
            foreach (var item in query.OrderBy(o => o.title))
            {
                i++;
                sh_ID[i] = item.showtime_id;
                cmbTimeTab1.Items.Add(item.time.ToString());
                txbDirectorTab1.Text=item.director;
                txbGenerTab1.Text = item.genre;
                txbYearTab1.Text = item.year_movie.ToString();
                txbDurationTab1.Text = item.duration.ToString().Remove(5, 3);
                rtbCastTab1.Text = item.cast_movie;
                lblPrice.Text = item.price.ToString() + " p."; 
            }

            while (i > 0)
            {
                temp = sh_ID[i];

                //var query1 = db.showtimes.Where(c => c.showtime_id == temp).SelectMany(c => c.halls).Distinct();
                
                var query1 = (from g in db.hall_showtime
                                 where g.showtime_id == temp
                              select g.hall_id).Distinct();

                var query2 = (from g in db.halls
                              where g.hall_id == query1.FirstOrDefault()
                              select g.hall_name).Distinct();

                foreach (string item in query2)
                {
                    cmbHallTab1.Items.Add(item);
                }

                i--;
            }

        }

        private void cmbTimeTab1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //int[] sh_ID = new int[10];
            //int i = 0;
            //int temp = 0;

            //// запрос поиска с отбором по названию фильма
            //var query = (from movie in db.movies
            //             join st in db.showtimes on movie.movieid equals st.movieid
            //             orderby movie.title
            //             where (st.time.ToString() == cmbTimeTab1.Text.ToString())
            //             select new { movie.title, movie.duration, movie.director, movie.genre, movie.cast_movie, st.time, st.showtime_id }).Distinct();
            ////var temp = query.Where(p => (p.title.ToString() == cmbTitleFilmTab2.Text.ToString()));

            //cmbTitleFilmTab1.Items.Clear();
            //cmbHallTab1.Items.Clear();

            //// заполнение combobox о фильме           
            //foreach (var item in query.OrderBy(o => o.title))
            //{
            //    i++;
            //    sh_ID[i] = item.showtime_id;
            //    cmbTitleFilmTab1.Items.Add(item.title.ToString());
            //}


            //while (i > 0)
            //{
            //    temp = sh_ID[i];

            //    //var query1 = db.showtimes.Where(c => c.showtime_id == temp).SelectMany(c => c.halls).Distinct();

            //    var query1 = (from g in db.hall_showtime
            //                  where g.showtime_id == temp
            //                  select g.hall_id).Distinct();

            //    var query2 = (from g in db.halls
            //                  where g.hall_id == query1.FirstOrDefault()
            //                  select g.hall_name).Distinct();

            //    foreach (string item in query2)
            //    {
            //        cmbHallTab1.Items.Add(item);
            //    }

            //    i--;
            //}

            //cmbTitleFilmTab1.SelectedIndexChanged -= new System.EventHandler(this.cmbTitleFilmTab1_SelectedIndexChanged);    // отключаем событие по изменению индекса в combobox
            //cmbTitleFilmTab1.SelectedIndex = 0;
            //cmbTitleFilmTab1.SelectedIndexChanged += new System.EventHandler(this.cmbTitleFilmTab1_SelectedIndexChanged);    // включаем событие по изменению индекса в combobox


        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            cmbTitleFilmTab2.Items.Clear();
            cmbHallTab2.Items.Clear();
            cmbTimeTab2.Items.Clear();

            load_data();
        }

        private void btnClearTab1_Click(object sender, EventArgs e)
        {
            cmbTitleFilmTab1.Items.Clear();
            cmbHallTab1.Items.Clear();
            cmbTimeTab1.Items.Clear();

            load_data();

        }

        private void txbDurationTab1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox_Pass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnLoginEnter_Click(null, null);
            }
        }

        private void textBox_Login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnLoginEnter_Click(null, null);
            }
        }

        private void rtbCastTab1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddDataTab3_Click(object sender, EventArgs e)
        {

            if (txbDirectorTab1.TextLength == 0 || txbGenerTab1.TextLength == 0)
            {
                MessageBox.Show("Поле оставлено пустым, данные не могут быть добавлены");
            }

            else
            {
                var query = (from g in db.movies
                             //where g.name_group == comboBox1.SelectedItem.ToString()
                             select g.movieid).ToList();

                int number_of_movies = db.movies.Max(n => n.movieid) + 1;

                movie new_movie = new movie
                {
                    movieid = number_of_movies,
                    title = cmbTitleFilmTab3.Text,
                    director = txbDirectorTab3.Text,
                    genre = txbGenerTab3.Text,
                    year_movie = int.Parse(txbYearTab3.Text),
                    duration = TimeSpan.Parse(txbDurationTab3.Text),
                    cast_movie = rtbCastTab3.Text  /*code_group = query[0]*/
                };

                int number_of_showtimes = db.showtimes.Max(n => n.showtime_id) + 1;

                showtime new_showtime = new showtime
                {
                    showtime_id = number_of_showtimes,
                    movieid = number_of_movies,
                    price = (int)(float.Parse(txbPriceTab3.Text) * ((100 - float.Parse(cmbDiscountTab3.Text.Remove(2, 1))) / 100)),
                    ////hall = cmbHallTab3.Text,
                    date = dateTimePicker1.Value,
                    time = TimeSpan.Parse(cmbTimeTab3.Text)
                };

                var query_hall = (from g in db.halls
                                 where g.hall_name == cmbHallTab3.Text
                                select g.hall_id).Distinct();

                int hall_ID = query_hall.First();

                var query_hall1 = (from g in db.halls
                                  where g.hall_id == hall_ID
                                  select g.seats).Distinct();

                string seats_toDB = query_hall1.First();
   
                db.movies.Add(new_movie);
                db.showtimes.Add(new_showtime);
                db.SaveChanges();

                int number_of_hSht = db.hall_showtime.Max(n => n.hsh_id) + 1; //Запись с max ID билета,чтобы записать новый с ID на 1 больше

                hall_showtime new_hSht = new hall_showtime
                {
                    hall_id = hall_ID,
                    showtime_id = number_of_showtimes,
                    hsh_id = number_of_hSht,
                    seat_map = seats_toDB,// записываем пустой зал
                };

                db.hall_showtime.Add(new_hSht);
                db.SaveChanges();


                //using (var context = new cinemaEntities2())
                //{
                //    var a = new hall { hall_id = hall_ID };
                //    var b = new showtime { showtime_id = number_of_showtimes };

                //    // Если сущности c указанными ключами уже загружены в контекст - тут будет ошибка
                //    // Постарайтесь, чтобы так не случалось (лучший способ - каждый раз создавать новый контекст)
                //    context.Entry(a).State = EntityState.Unchanged;
                //    context.Entry(b).State = EntityState.Unchanged;
                //    a.showtimes = new List<showtime> { b }; // Если тут использовать массив - полезут ошибки при отслеживании связей в будущем. Но если контекст - временный, то можно и массив использовать.
                //    context.SaveChanges();
                //    a.showtimes.Add(b);

                //    // Очистка контекста - можно не делать, если контекст больше не будет использоваться
                //    context.Entry(a).State = EntityState.Detached;
                //    context.Entry(b).State = EntityState.Detached;
                //}

                //db.SaveChanges();
                //db.showtimes.Add(new_showtime);
                MessageBox.Show("Данные записаны!");
            }

            load_data();
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbDiscountTab3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txbYearTab3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbDurationTab3_TextChanged(object sender, EventArgs e)
        {

        }

        private void rtbCastTab3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbGenerTab3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbDirectorTab3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void cmbTitleFilmTab3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] sh_ID = new int[10];
            int i = 0;
            int temp = 0;

            // запрос поиска с отбором по названию фильма
            var query = (from movie in db.movies
                         join st in db.showtimes on movie.movieid equals st.movieid
                         orderby movie.title
                         where (movie.title == cmbTitleFilmTab3.Text.ToString())
                         select new { movie.title, movie.duration, movie.director, movie.genre, movie.cast_movie, st.time, st.date, st.showtime_id, st.price, movie.year_movie }).Distinct();

            cmbTimeTab3.Items.Clear();
            cmbHallTab3.Items.Clear();

            // заполнение combobox о фильме           
            foreach (var item in query.OrderBy(o => o.title))
            {
                i++;
                sh_ID[i] = item.showtime_id;
                cmbTimeTab3.Items.Add(item.time.ToString());
                txbDirectorTab3.Text = item.director;
                txbGenerTab3.Text = item.genre;
                txbYearTab3.Text = item.year_movie.ToString();
                txbDurationTab3.Text = item.duration.ToString().Remove(5, 3);
                rtbCastTab3.Text = item.cast_movie;
                //lblPrice.Clear();
                txbPriceTab3.Text = item.price.ToString() + " p.";
                dateTimePicker1.Value = (DateTime)item.date;
            }


            while (i > 0)
            {
                temp = sh_ID[i];

                var query1 = (from g in db.hall_showtime
                              where g.showtime_id == temp
                              select g.hall_id).Distinct();

                var query2 = (from g in db.halls
                              where g.hall_id == query1.FirstOrDefault()
                              select g.hall_name).Distinct();

                foreach (string item in query2)
                {
                    cmbHallTab3.Items.Add(item);
                }

                i--;
            }
        }


        private void cmbHallTab2_DrawItem(object sender, DrawItemEventArgs e)
        {

            int index;

            // Draw the background of the ComboBox control for each item.
            e.DrawBackground();

            // Define the default color of the brush as black.
            // Draw the current item text based on the current Font 
            e.Graphics.DrawString(cmbHallTab2.Items[e.Index].ToString(), e.Font, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();

            if ((e.State & DrawItemState.Selected) != 0)
            {
                index = cmbHallTab2.SelectedIndex;

                clear_seatMap(tabPage2);
                draw_seatMap(seats_array[index], tabPage2);
            }
            else
            {
                // else label1.Text = "not selected";
            }
        }


        private void cmbHallTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // берём название фильма и время чтобы получить movie_ID чтобы потом получить showtime_ID используя время
            int movID = 0;
            int shoID = 0;

            //запрос movie ID по его названию 
            var movieID = (from movie in db.movies
                           where (movie.title == cmbTitleFilmTab1.Text)
                           select new { movie.movieid }).Distinct();

            foreach (var it in movieID)
            {
                movID = it.movieid;
            }

            //запрос showtimeID по фильму
            var query = (from showtime in db.showtimes
                         orderby showtime.movieid
                         where (showtime.time.ToString() == cmbTimeTab1.Text.ToString() && showtime.movieid == movID)
                         select new { showtime.showtime_id }).Distinct();

            foreach (var it in query)
            {
                shoID = it.showtime_id;
            }

            // получаем hall_id по названию зала
                        var query_hall = (from g in db.halls
                              where g.hall_name == cmbHallTab1.Text
                              select g.hall_id).Distinct();

            int hall_ID = query_hall.First();


            var query11 = (from hall_showtime in db.hall_showtime
                           orderby hall_showtime.hall_id
                           where (hall_showtime.showtime_id == shoID && hall_showtime.hall_id == hall_ID)
                           select new { hall_showtime.seat_map}).Distinct();

            seats_array_index = 0;
            Array.Clear(seats_array, 0, seats_array.Length);

            foreach (var it in query11)
            {
                seats_array[0] = it.seat_map;
            }

            clear_seatMap(tabPage1);
            draw_seatMap(seats_array[0], tabPage1); 
        }

        private void cmbHallTab1_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index;
            // Change the DrawMode property of combobox!!!!!!!!!!!!!

            // Draw the background of the ComboBox control for each item.
            e.DrawBackground();

            // Define the default color of the brush as black.
            // Draw the current item text based on the current Font 
            e.Graphics.DrawString(cmbHallTab1.Items[e.Index].ToString(), e.Font, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();

 


            if ((e.State & DrawItemState.Selected) != 0)
            {
                index = cmbHallTab1.SelectedIndex;



                clear_seatMap(tabPage1);
                draw_seatMap(seats_array[index], tabPage1);
            }
            else
            {
                // else label1.Text = "not selected";
            }


        }

        private void cmbHallTab3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbSeatTab1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void txbPriceTab3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count == 1)
            {
                ehl = new EditHallLayout(this);
                ehl.Show();

            }

        }

        private void btnReturnTab5_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(tbxTicketForReturn.Text);
            int shoID = 0;
            int hall_ID = 0;
            string seat_for_delete = "";
            string result = "";

            try
            {
                var de = db.tickets.Where(x => x.ticket_id == ID).FirstOrDefault();
                
                var query1 = (from ticket in db.tickets
                                orderby ticket.ticket_id
                                where (ticket.ticket_id == ID)
                                select new { ticket.hall_id, ticket.showtime_id, ticket.seat }).Distinct();

                foreach (var it in query1)
                {
                    shoID = it.showtime_id;
                    hall_ID = (int)it.hall_id;
                    seat_for_delete = it.seat;
                }

                DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите удалить билет", "Удаление билета", MessageBoxButtons.YesNo);
                
                if (dialogResult == DialogResult.Yes)
                {

                    /* освобождаем занятое место */

                    MessageBox.Show("hall_id " + hall_ID);

                    var query11 = (from hall_showtime in db.hall_showtime
                                   orderby hall_showtime.hall_id
                                   where (hall_showtime.showtime_id == shoID && hall_showtime.hall_id == hall_ID)
                                   select new { hall_showtime.seat_map }).Distinct();

                    foreach (var it in query11)
                    {
                        seats_array[0] = it.seat_map;
                    }

                    // помечаем место как свободное в базе данных
                    string[] seat_for_split = seat_for_delete.Split(',');
                    int seat_to_DB = ((int.Parse(seat_for_split[0])) - 1) + (int.Parse(seat_for_split[1]) - 1) * 10;

                    string[] seats_forProcess = seats_array[0].Split(',');  // получаем карту зала которую хотим обновить

                    //int hall_for_update = hall_forList.First();
                    seats_forProcess[seat_to_DB] = "1";

                    result = seats_forProcess[0];

                    for (int i = 1; i < 50; i++)
                    {
                        result += ("," + seats_forProcess[i]);
                    }

                    db.Configuration.ValidateOnSaveEnabled = false;
                    var verifcation = db.hall_showtime.Where(x => x.showtime_id == shoID && x.hall_id == hall_ID).FirstOrDefault();
                    verifcation.seat_map = result;
                    MessageBox.Show("Место в зале освобождено!");

                    // delete from database 
                    db.Entry(de).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    MessageBox.Show("Удалено");
                }

                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

                db.SaveChanges();
            }

            catch
            {
                MessageBox.Show("Билет не найден!");
            }

        }

        private void tbxTicketForReturn_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUnBookTab5_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(tbxTicketForUnBook.Text);

            try
            {
                var de = db.tickets.Where(x => x.ticket_id == ID).FirstOrDefault();

                DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите снять бронь", "Снятие брони", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    // delete from database 
                    db.Entry(de).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    MessageBox.Show("Бронь снята");
                }

                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
             
            }

            catch
            {
                MessageBox.Show("Билет не найден!");
            }
        }


        void seatRelease(int seat)
        {


        }


        private void tabPage1_MouseDown(object sender, MouseEventArgs e)
        {
            int x, y, index, seat_to_change;
            string result = "";

            x = e.X;
            y = e.Y;


            if ((x > 100 && x < 600) && (y > 100 && y < 380))
            {
                x = ((x - 100) / 52) ;
                y = ((y - 100) / 60) ;
                
                cmbSeatTab1.Text = (x + 1) + "," + (y + 1); 

                seat_to_change = x + (y * 10);

                index = cmbHallTab1.SelectedIndex;
                clear_seatMap(tabPage1);

                //string temp = seats_array[index];
                string temp = seats_array[0];
                string[] numbers = temp.Split(',');
                
                if (numbers[seat_to_change] != "0")
                {
                    numbers[seat_to_change] = "5";

                    result = numbers[0];

                    for (int i = 1; i < 50; i++)
                    {
                        result += ("," + numbers[i]);
                    }

                    draw_seatMap(result, tabPage1);
                }


            }

        }

        private void tbxTicketForUnBook_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbYearTab1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbGenerTab1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbDirectorTab1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
