using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Cinema_VP
{
    public partial class Form1 : Form
    {
        int couner = 0;                         //счётчик морганий label
        private Bitmap MyImage;                 // картинка пользователя
        private string roleIndex = "null";      // Роль= Администратор, Пользователь, Управляюший
        public cinemaEntities2 db = new cinemaEntities2();
        public List<role> roleSheet;            //список ролей для вхождения в систему
        public string[] seats_array = new string[10];   // обьявление массив мест в зрительном зале
        public int seats_array_index = 0;
        int hall_ID = 0;

        databaseAccess dbAccess = new databaseAccess(); //класс обращения к бд

        public Form1()
        {
            InitializeComponent();

            ShowMyImage(Properties.Resources.User_Guest1, 200, 200);  // картинка гостя 
        }

        public void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Проверьте правильность данных", "Покупука  билета", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                seats_array[0] = dbAccess.addTicket(cmbTitleFilmTab1.Text, cmbTimeTab1.Text, cmbHallTab1.Text, cmbRowTab1.Text, cmbSeatTab1.Text, lblPrice.Text, roleIndex, seats_array[0]);

                clear_seatMap(tabPage1);
                draw_seatMap(seats_array[0], tabPage1);

            }
            else if (dialogResult == DialogResult.No)
            {
                // do something else
            }
        }

        public  void clear_seatMap(TabPage tabPage)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White); //рисуем белый прямоугольник по всему полю нашей карты
            System.Drawing.Graphics formGraphics;
            formGraphics = tabPage.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(50, 50, 600, 600)); //начало координат(50,50) с размерами 600*600 пикселей
            myBrush.Dispose();
            formGraphics.Dispose();  //удаляем обьект рисования
        }

        /* принцип кодирования мест: 0-нет места 1-место не занято, 2-место занято */
        public void draw_seatMap(string Map, TabPage Tab_Page)
        {
            int i = 0;

            try
            {
               string[] seat = Map.Split(',');

            Graphics g = Tab_Page.CreateGraphics();

            g.DrawRectangle(Pens.Red, 175, 50, 350, 20);    // рисуем экран зрительного зала(начало координат экрана )

            for (int y = 100; y < 400; y += 60)             // рисуем места на поле для рисования задаём y от 100 до 400
            {

                for (int x = 100; x < 600; x += 50)  // рисуем места по x с шагом 50 пикселей
                {

                    if (seat[i] == "1")
                    {
                        g.DrawRectangle(Pens.Green, x, y, 40, 40); //не занятое место
                    }

                    if (seat[i] == "2")
                    {
                        g.DrawRectangle(Pens.Red, x, y, 40, 40);
                    }

                    if (seat[i] == "5") // это место выбрано для оформления билета 
                    {
                        g.DrawRectangle(Pens.Red, x, y, 40, 40);
                        g.DrawRectangle(Pens.Red, x + 3, y + 3, 34, 34); //рисуем второй красный квадрат на 6 пикселей меньше
                    }

                    if (seat[i] == "3")
                    {
                        g.DrawRectangle(Pens.Green, x, y, 90, 40); //рисуем свободный диванчик
                        x += 50;
                        i++;
                    }

                    if (seat[i] == "4")
                    {
                        g.DrawRectangle(Pens.Red, x, y, 90, 40); //рисуем занятый диванчик
                            x += 50; //передвигаемся на один шаг вправо для след.нарисовки
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
            catch 
            {
                MessageBox.Show("Сначала выберите фильм и время");
            }

            
        }

        // бронирование билета
        private void btnBookTicketTab1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Проверьте правильность данных", "Покупука  билета", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                seats_array[0] = dbAccess.addTicket(cmbTitleFilmTab1.Text, cmbTimeTab1.Text, cmbHallTab1.Text, cmbRowTab1.Text, cmbSeatTab1.Text, "000 p.", roleIndex, seats_array[0]);
                clear_seatMap(tabPage1);
                draw_seatMap(seats_array[0], tabPage1);
            }
            else if (dialogResult == DialogResult.No)
            {
                // do something else
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // не меньше начального размера 
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            // не больше размера экрана для формы
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            load_data();
        }

        /* функция добавления распределения мест в зале */
        void Add_to_seats(string seats)
        {
            seats_array[seats_array_index] = seats; //заполнение массива мест в зале из бд
            seats_array_index++;
        }

        /* заполнение всех compbobox и других полей */
        void load_data()
        {
            var hall_forList = (from s in db.halls      // Подготовка свободных мест в зале 
                                orderby s.hall_name
                                select new { s.hall_name, s.hall_id, s.seats }).Distinct();

            cmbHallTab1.Items.Clear();
            cmbHallTab2.Items.Clear();
            cmbHallTab3.Items.Clear();
            tbxTicketID.Clear();
            seats_array_index = 0;
            Array.Clear(seats_array, 0, seats_array.Length);

            foreach (var it in hall_forList)
            {
                cmbHallTab1.Items.Add(it.hall_name);
                cmbHallTab2.Items.Add(it.hall_name);
                cmbHallTab3.Items.Add(it.hall_name);
                Add_to_seats(it.seats);
            }

            cmbHallTab2.DrawMode = DrawMode.OwnerDrawFixed;

            var movie_forList = (from s in db.movies    // фильмы для списка
                                 orderby s.title
                                 select s.title).Distinct();

            foreach (string it in movie_forList)
            {
                cmbTitleFilmTab1.Items.Add(it);
                cmbTitleFilmTab2.Items.Add(it);
                cmbTitleFilmTab3.Items.Add(it);
            }

            var time_forList = (from s in db.showtimes  // время сеансов для списка
                                orderby s.time
                                select s.time).Distinct();

            foreach (var it in time_forList)
            {
                cmbTimeTab1.Items.Add(it.ToString());
                cmbTimeTab2.Items.Add(it.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnBuyTicket_Click(object sender, EventArgs e) // кнопка купить билет
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

        private void btnRetTicket_Click(object sender, EventArgs e) // кнопка вернуть билет
        {
            if (roleIndex == "Администратор" || roleIndex == "Пользователь")
            {
                tabControl1.SelectedTab = tabPage5; //возвращает на логин,если не вошёл в систему
            }
            else
            {
                loginRequired();
            }
        }

        private void btnBookRet_Click(object sender, EventArgs e)   // кнопка бронировать билет
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

        void loginRequired()  // функция для моргания при не выолненном входе
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

        private void btnExit_Click(object sender, EventArgs e) // кнопка выхода 
        {

        }

        void loginFormLock(bool action) // блокировка формы входа
        {
            if (action == true)
            {
                textBox_Login.Enabled = false;
                textBox_Pass.Enabled = false;
                btnLoginEnter.Enabled = false;
            }
            else
            {
                textBox_Login.Enabled = true;
                textBox_Pass.Enabled = true;
                btnLoginEnter.Enabled = true;
                textBox_Login.Clear();
                textBox_Pass.Clear();
            }
        }

        public (string, string) loginFunc() // функция входа для логина и пароля
        {
            var temp_login = (from r in db.roles
                              select new { r.role1, r.login, r.password }).ToList();

            foreach (var item in temp_login.OrderBy(o => o.role1))
            {
                if (textBox_Login.Text == item.login && textBox_Pass.Text == item.password)
                {
                    return (item.role1.ToString(), item.login.ToString());
                }
            }

            return ("failed", "no data");    // неверная попытка входа
        }

        public void btnLoginEnter_Click(object sender, EventArgs e) // кнопка входа 
        {
            var values = dbAccess.loginFunc(textBox_Login.Text, textBox_Pass.Text);

            bool result = false;
            login lgn = new login();
            result = lgn.checkUserInput(textBox_Login.Text, textBox_Pass.Text);

            if (values.Item2 == "admin")
            {
                tabPage1.Enabled = true;
                tabPage3.Enabled = true;
                tabPage5.Enabled = true;
                label14.Text = "Вход выполнен как " + values.Item1;
                this.Text = "Заказ билета в кинотеатр - " + values.Item1;
                tabControl1.SelectedTab = tabPage1;
                ShowMyImage(Properties.Resources.User_Admin, 200, 200);
                loginFormLock(true);
                roleIndex = "Администратор";
                this.tabControl1.Controls.Remove(this.tabPage4);
                btnExitMainFrm.Visible = true;
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
                roleIndex = "Пользователь";
                this.tabControl1.Controls.Remove(this.tabPage4);
                btnExitMainFrm.Visible = true;
            }

            else if (values.Item2 == "user1")
            {
                tabPage3.Enabled = true;
                label14.Text = "Вход выполнен как " + values.Item1 + " Доступно только внесение данных";
                this.Text = "Заказ билета в кинотеатр - " + values.Item1;
                tabControl1.SelectedTab = tabPage3;
                ShowMyImage(Properties.Resources.User_User1, 200, 200);
                loginFormLock(true);
                roleIndex = "Управляющий";
                this.tabControl1.Controls.Remove(this.tabPage4);
                btnExitMainFrm.Visible = true;
            }

            else
            {
                label_wrongLogin.Visible = true;
                textBox_Login.BackColor = System.Drawing.Color.Red;
                textBox_Pass.BackColor = System.Drawing.Color.Red;
            }
        }

        public class login //класс логина и пароля
        {
            public login()
            {

            }

            public bool checkUserInput (string login, string pass)
            {
                if (login == "admin")
                {
                    return true;
                }

                return false;
            }
        }

        private void textBox_Login_TextChanged(object sender, EventArgs e) //очищает поле логина
        {
            if (textBox_Login.BackColor != SystemColors.Window)
            {
                clear_userInput();
            }
        }

        private void textBox_Pass_TextChanged(object sender, EventArgs e) //очищает поле пароля
        {
            if (textBox_Pass.BackColor != SystemColors.Window)
            {
                textBox_Pass.Clear();
                label_wrongLogin.Visible = false;
                textBox_Pass.BackColor = SystemColors.Window;
                textBox_Login.BackColor = SystemColors.Window;
            }
        }

        public void clear_userInput() //очищает всё 
        {
            textBox_Login.Clear();
            textBox_Pass.Clear();
            label_wrongLogin.Visible = false;
            textBox_Pass.BackColor = SystemColors.Window;
            textBox_Login.BackColor = SystemColors.Window;
        }

        // функция для смены картинки
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

        /* выбор фильма на 2 вкладке*/
        private void cmbTitleFilmTab2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbTimeTab2.Items.Clear();
            cmbHallTab2.Items.Clear();
            dbAccess.selectTimeByMovieTitle(ref cmbTimeTab2, ref cmbTitleFilmTab2, ref cmbHallTab2, ref txbDirectorTab1, ref txbYearTab1, ref txbGenerTab1, ref txbDurationTab1, ref rtbCastTab1, ref lblPrice );
        }

        // событие выбора времени
        private void cmbTimeTab2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        // Выбор фильма на 1 вкладке
        private void cmbTitleFilmTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbTimeTab1.Items.Clear();
            cmbHallTab1.Items.Clear();
            tbxTicketID.Clear();
            dbAccess.selectTimeByMovieTitle(ref cmbTimeTab1, ref cmbTitleFilmTab1, ref cmbHallTab1, ref txbDirectorTab1, ref txbYearTab1, ref txbGenerTab1, ref txbDurationTab1, ref rtbCastTab1, ref lblPrice);
        }

        /* выбор и сортировка по времени */
        private void cmbTimeTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbxTicketID.Clear();
        }

        /* кнопка очистить на 2 вкладке */
        private void btnClearList_Click(object sender, EventArgs e)
        {
            cmbTitleFilmTab2.Items.Clear();
            cmbHallTab2.Items.Clear();
            cmbTimeTab2.Items.Clear();
            load_data();
        }

        /*кнопка очистить на 1 вкладке */
        private void btnClearTab1_Click(object sender, EventArgs e)
        {
            cmbTitleFilmTab1.Items.Clear();
            cmbHallTab1.Items.Clear();
            cmbTimeTab1.Items.Clear();
            load_data();
        }

        /* активация кнопки Enter при вводе пароля */
        private void textBox_Pass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnLoginEnter_Click(null, null);
            }
        }

        /* активация кнопки Enter при вводе логина */
        private void textBox_Login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnLoginEnter_Click(null, null);
            }
        }

        /* кнопка внесения данных на 3 вкладке */
        private void btnAddDataTab3_Click(object sender, EventArgs e)
        {
            if (txbDirectorTab1.TextLength == 0 || txbGenerTab1.TextLength == 0)
            {
                MessageBox.Show("Поле оставлено пустым, данные не могут быть добавлены");
            }

            else
            {
                dbAccess.addMovie(ref cmbTitleFilmTab3, ref txbDirectorTab3, ref txbGenerTab3, ref txbYearTab3, ref txbDurationTab3, ref rtbCastTab3, 
                    ref txbPriceTab3, ref dateTimePicker1, ref cmbDiscountTab3, ref cmbTimeTab3, ref cmbHallTab3);
                MessageBox.Show("Данные записаны!");
            }

            load_data(); // обновляем данные в форме
        }

        // выбор фильма на 3 вкладке
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
                txbPriceTab3.Text = item.price.ToString();
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

        /* обработка события DrawItem для combobox для возможности просмотра карты при движении мыши без щелчка */
        private void cmbHallTab2_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index;

            // Рисуем прямоугольник выделения на combobox для каждого элемента
            e.DrawBackground();
            // отрисовываем чёрным шрифтом текст шрифт на основании 
            e.Graphics.DrawString(cmbHallTab2.Items[e.Index].ToString(), e.Font, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            // переносим фокус на выбранный элемент
            e.DrawFocusRectangle();

            if ((e.State & DrawItemState.Selected) != 0)
            {
                index = cmbHallTab2.SelectedIndex;
                clear_seatMap(tabPage2);
                draw_seatMap(seats_array[index], tabPage2);
            }
        }

        // выбор зала на 1 вкладке
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

            // запрос showtimeID по фильму
            var query = (from showtime in db.showtimes
                         orderby showtime.movieid
                         where (showtime.time.ToString() == cmbTimeTab1.Text.ToString() && showtime.movieid == movID)
                         select new {showtime.showtime_id}).Distinct();

            foreach (var it in query)
            {
                shoID = it.showtime_id;
            }

            // получаем hall_id по названию зала
            var query_hall = (from g in db.halls
                              where g.hall_name == cmbHallTab1.Text
                              select g.hall_id).Distinct();

            hall_ID = query_hall.First();

            var query11 = (from hall_showtime in db.hall_showtime
                           orderby hall_showtime.hall_id
                           where (hall_showtime.showtime_id == shoID && hall_showtime.hall_id == hall_ID)
                           select new { hall_showtime.seat_map }).Distinct();

            seats_array_index = 0;
            Array.Clear(seats_array, 0, seats_array.Length);

            foreach (var it in query11)
            {
                seats_array[0] = it.seat_map;
            }

            clear_seatMap(tabPage1);
            draw_seatMap(seats_array[0], tabPage1);
            tbxTicketID.Clear();
        }

        /* обработка события DrawItem для combobox для возможности просмотра карты при движении мыши без щелчка для 2 вкладке */
        private void cmbHallTab1_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index;

            e.DrawBackground();
            e.Graphics.DrawString(cmbHallTab1.Items[e.Index].ToString(), e.Font, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();

            if ((e.State & DrawItemState.Selected) != 0)
            {
                index = cmbHallTab1.SelectedIndex;

                clear_seatMap(tabPage1);
                draw_seatMap(seats_array[index], tabPage1);
            }

        }
        public void selectTimeByMovieTitle(ComboBox cmbTime, ComboBox cmbHall, string Title)
        {
            int[] sh_ID = new int[10];
            int i = 0;
            int temp = 0;

            // запрос поиска с отбором по названию фильма
            var query = (from movie in db.movies
                         join st in db.showtimes on movie.movieid equals st.movieid
                         orderby movie.title
                         where (movie.title == Title)
                         select new { movie.title, movie.duration, movie.director, movie.genre, movie.cast_movie, st.time, st.showtime_id, st.price, movie.year_movie }).Distinct();

            // заполнение combobox о фильме           
            foreach (var item in query.OrderBy(o => o.title))
            {
                i++;
                sh_ID[i] = item.showtime_id;
                cmbTime.Items.Add(item.time.ToString());
                txbDirectorTab1.Text = item.director;
                txbGenerTab1.Text = item.genre;
                txbYearTab1.Text = item.year_movie.ToString();
                txbDurationTab1.Text = item.duration.ToString().Remove(5, 3);
                rtbCastTab1.Text = item.cast_movie;
                lblPrice.Text = item.price.ToString() + " p.";
            }

            while (i > 0)
            {
                temp = sh_ID[i];

                var query1 = (from g in db.hall_showtime
                              where g.showtime_id == temp
                              select g.hall_id).Distinct();

                var query2 = (from g in db.halls
                              where g.hall_id == query1.FirstOrDefault()
                              orderby g.hall_name
                              select new { g.hall_name }).Distinct();

                foreach (var item in query2)
                {
                    // если в combobox уже внесён этот зал то мы его не добавляем
                    if (!cmbHall.Items.Contains(item.hall_name))
                    {
                        cmbHall.Items.Add(item.hall_name);
                    }
                }

                i--;
            }
        }

        // кнопка возврата билета 
        private void btnReturnTab5_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(tbxTicketForReturn.Text);

            // проверяем существует ли билет
            if (dbAccess.validateTicket(ID) == true)
            {
                DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите удалить билет", "Удаление билета", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    dbAccess.DeleteTicket(ID);
                    MessageBox.Show("Место в зале освобождено!");
                }

                else if (dialogResult == DialogResult.No)
                {

                }
            }
            else
            {
                MessageBox.Show("Билет не найден!");
            }

        }

        /* кнопка отмены бронирования */
        private void btnUnBookTab5_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(tbxTicketForUnBook.Text);

            // проверяем существует ли билет
            if (dbAccess.validateTicket(ID) == true )
            {
                if (dbAccess.isTicketBooked(ID) == true)
                {
                    DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите снять бронь", "Снятие брони", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        dbAccess.DeleteTicket(ID);
                        MessageBox.Show("Место в зале освобождено!");
                    }

                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else
                {
                    MessageBox.Show("Этот билет нужно возвращать как купленный");
                }
            }
            else
            {
                MessageBox.Show("Билет не найден!");
            }
        }

        // обработка события щелчка мыши по карте распределения мест в зале, выбор места
        private void tabPage1_MouseDown(object sender, MouseEventArgs e)
        {
            int x, y, index, seat_to_change;
            string result = "";

            x = e.X;
            y = e.Y;

            if ((x > 100 && x < 600) && (y > 100 && y < 380))
            {
                x = ((x - 100) / 52);
                y = ((y - 100) / 60);

                cmbSeatTab1.Text = (x + 1).ToString();
                cmbRowTab1.Text = (y + 1).ToString();

                seat_to_change = x + (y * 10);

                index = cmbHallTab1.SelectedIndex;
                clear_seatMap(tabPage1);

                string temp = seats_array[0];
                string[] numbers = temp.Split(',');

                if (numbers[seat_to_change] == "1")     // выбираем только то место которое существует
                {
                    numbers[seat_to_change] = "5"; //выбранное место
                    result = numbers[0];
                }

                if (numbers[seat_to_change] == "2")     // если место занято запрашиваем и выводим Ticket_ID
                {
                    string seat_to_funnc = (x + 1).ToString() + "," + (y + 1).ToString();

                    tbxTicketID.Text = dbAccess.GetTicketNummber(seat_to_funnc, hall_ID, cmbTitleFilmTab1.Text, cmbTimeTab1.Text).ToString();
                }
                else
                {
                    tbxTicketID.Clear();
                }

                for (int i = 1; i < 50; i++)
                {
                    result += ("," + numbers[i]);
                }

                draw_seatMap(result, tabPage1);
            }
        }

        private void btnExitMainFrm_Click(object sender, EventArgs e)
        {
            tabPage1.Enabled = false;
            tabPage3.Enabled = false;
            tabPage5.Enabled = false;
            btnLoginEnter.Enabled = true;
            label14.Text = "Вход не выполнен - возможен только промотр";
            ShowMyImage(Properties.Resources.User_Guest1, 200, 200);
            clear_userInput();
            this.Text = "Заказ билета в кинотеатр - Вход не выполнен";
            loginFormLock(false);
            roleIndex = "null";
            this.tabControl1.Controls.Add(this.tabPage4);
            tabControl1.SelectedTab = tabPage4;
            btnExitMainFrm.Visible = false;
        }

        private void cmbHallTab2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // берём название фильма и время чтобы получить movie_ID чтобы потом получить showtime_ID используя время
            int movID = 0;
            int shoID = 0;

            //запрос movie ID по его названию 
            var movieID = (from movie in db.movies
                           where (movie.title == cmbTitleFilmTab2.Text)
                           select new { movie.movieid }).Distinct();

            foreach (var it in movieID)
            {
                movID = it.movieid;
            }

            // запрос showtimeID по фильму
            var query = (from showtime in db.showtimes
                         orderby showtime.movieid
                         where (showtime.time.ToString() == cmbTimeTab2.Text.ToString() && showtime.movieid == movID)
                         select new { showtime.showtime_id }).Distinct();

            foreach (var it in query)
            {
                shoID = it.showtime_id;
            }

            // получаем hall_id по названию зала
            var query_hall = (from g in db.halls
                              where g.hall_name == cmbHallTab2.Text
                              select g.hall_id).Distinct();

            hall_ID = query_hall.First();

            var query11 = (from hall_showtime in db.hall_showtime
                           orderby hall_showtime.hall_id
                           where (hall_showtime.showtime_id == shoID && hall_showtime.hall_id == hall_ID)
                           select new { hall_showtime.seat_map }).Distinct();

            seats_array_index = 0;
            Array.Clear(seats_array, 0, seats_array.Length);

            foreach (var it in query11)
            {
                seats_array[0] = it.seat_map;
            }

            clear_seatMap(tabPage2);
            draw_seatMap(seats_array[0], tabPage2);
            tbxTicketID.Clear();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            cmbTitleFilmTab1.Items.Clear();
            cmbHallTab1.Items.Clear();
            cmbTimeTab1.Items.Clear();
            cmbTitleFilmTab2.Items.Clear();
            cmbHallTab2.Items.Clear();
            cmbTimeTab2.Items.Clear();
            cmbTitleFilmTab3.Items.Clear();
            load_data();
        }
    }

    public class databaseAccess //класс для работы с бд
    {
        public cinemaEntities2 db = new cinemaEntities2();
        public string[] seats_array = new string[10];   // обьявление массива мест в зрительном зале
        public databaseAccess()
        {

        }

        public string addTicket(string Title, string Time, string Hall, string Row, string Seat, string Price, string RoleIndex, string seats_array)
        {
            int movID = 0;
            int shoID = 0;
            string result = "";

            //запрос movie ID по его названию 
            var movieID = (from movie in db.movies
                           where (movie.title == Title)
                           select new { movie.movieid }).Distinct();

            foreach (var it in movieID)
            {
                movID = it.movieid;
            }

            // запрос showtimeID по фильму
            var query = (from showtime in db.showtimes
                         orderby showtime.movieid
                         where (showtime.time.ToString() == Time.ToString() && showtime.movieid == movID)
                         select new { showtime.showtime_id }).Distinct();

            foreach (var it in query)
            {
                shoID = it.showtime_id;
            }

            int number_of_tickets = db.tickets.Max(n => n.ticket_id) + 1; // ищем макс ID билета из существ., чтобы записать следующий билет

            //получаем hall_ID из названия зала
            var query_hall = (from g in db.halls
                              where g.hall_name == Hall
                              select g.hall_id).Distinct();

            int hall_ID = query_hall.First();
            string[] seat_for_split = new string[2];
            seat_for_split[0] = Row;
            seat_for_split[1] = Seat;
            int seat_to_DB = ((int.Parse(Seat)) - 1) + (int.Parse(Row) - 1) * 10; //определяем место,которое будет помечено как занятое
            string[] seats_forProcess = seats_array.Split(',');  // получаем карту зала которую хотим обновить
            seats_forProcess[seat_to_DB] = "2"; //2-занятое место
            result = seats_forProcess[0];

            for (int i = 1; i < 50; i++)
            {
                result += ("," + seats_forProcess[i]);
            }

            // запись билета в бд
            ticket new_ticket = new ticket
            {
                ticket_id = number_of_tickets,
                price = int.Parse(Price.Remove(3, 3)),
                seat = (Seat + "," + Row),
                showtime_id = shoID,
                hall_id = hall_ID,
                role = RoleIndex,
            };

            db.tickets.Add(new_ticket);

            // запись занятого места
            db.Configuration.ValidateOnSaveEnabled = false;
            var verifcation = db.hall_showtime.Where(x => x.showtime_id == shoID && x.hall_id == hall_ID).FirstOrDefault();
            verifcation.seat_map = result;

            db.SaveChanges();
            seats_array = result;
            MessageBox.Show("Данные записаны!");
            tckFRM tckFRM = new tckFRM(Title, Hall, Row, Seat, Time, number_of_tickets.ToString(), Price);
            TicketDisplay frm_ttd = new TicketDisplay(tckFRM);
            frm_ttd.Show();
            return seats_array;   // возвращаем 0 если процедура не выолнена успешно, 1 если успешно
        }
        public bool DeleteTicket(int ID)
        {
            int shoID = 0;
            int hall_ID = 0;
            string seat_for_delete = "";
            string result = "";
            bool retCode = false; //возвращаемое значение удачно ли нет

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

            /* освобождаем занятое место */
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
            seats_forProcess[seat_to_DB] = "1";
            result = seats_forProcess[0];

            for (int i = 1; i < 50; i++) //50-кол-во мест в зале
            {
                result += ("," + seats_forProcess[i]);
            }

            db.Configuration.ValidateOnSaveEnabled = false; //update bd
            var verifcation = db.hall_showtime.Where(x => x.showtime_id == shoID && x.hall_id == hall_ID).FirstOrDefault();
            verifcation.seat_map = result;

            // удаляем билет с базы данных
            db.Entry(de).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return retCode;
        }

        public int GetTicketNummber(string seat, int Hall_ID, string Movietitle, string MovieTime) //получаем номер билета
        {
            int result = 0;
            int movID = 0;
            int shoID = 0;

            // подготавливаем Showtime ID и hall_ID по имени зала
            var movieID = (from movie in db.movies
                           where (movie.title == Movietitle)
                           select new { movie.movieid }).Distinct();

            foreach (var it in movieID)
            {
                movID = it.movieid;
            }

            // запрос showtimeID по фильму
            var query = (from showtime in db.showtimes
                         orderby showtime.movieid
                         where (showtime.time.ToString() == MovieTime && showtime.movieid == movID)
                         select new { showtime.showtime_id }).Distinct();

            foreach (var it in query)
            {
                shoID = it.showtime_id;
            }

            // запрос поиска с отбором по названию фильма
            var query1 = (from ticket in db.tickets
                         orderby ticket.showtime_id
                         where (ticket.seat == seat && ticket.showtime_id == shoID && ticket.hall_id == Hall_ID)
                         select new { ticket.ticket_id }).Distinct();

            foreach (var it in query1)
            {
                result = it.ticket_id;
            }

            return result;
        }

        public bool validateTicket(int ID) //если ли билет в бд
        {
            bool retCode = false;

            var de = db.tickets.Where(x => x.ticket_id == ID).FirstOrDefault();

            if (de != null)
            {
                retCode = true;
            }

            return retCode;
        }

        public bool isTicketBooked (int ID) //является ли билет забронированным или купленным
        {
            bool result = false;

            var query1 = (from ticket in db.tickets
                          where ticket.ticket_id == ID
                          select ticket.price);

            if (query1.First() == 0)
            {
                result = true;
            }

            return result;
        }

        //метод для добавления нового фильма
        public int addMovie(ref ComboBox cmbTitleFilmTab3, ref TextBox txbDirectorTab3, ref TextBox txbGenerTab3,
                   ref TextBox txbYearTab3, ref TextBox txbDurationTab3, ref RichTextBox rtbCastTab3, ref TextBox txbPriceTab3,
                   ref DateTimePicker dateTimePicker1, ref ComboBox cmbDiscountTab3, ref ComboBox cmbTimeTab3, ref ComboBox cmbHallTab3)
        {

            var query = (from g in db.movies
                         select g.movieid).ToList();
            int number_of_movies = 1;
            int number_of_showtimes = 1;
            int number_of_hSht = 1;

            try
            {
                number_of_movies = db.movies.Max(n => n.movieid); // получаем количество записей в таблице фильмы
            }
            catch
            {
                number_of_movies = 1;
            }

            string title = cmbTitleFilmTab3.Text;

            // проверяем есть ли данный фильм в базе данных и добавляем если его нет, иначе записываем всё кроме таблицы Movie
            var query_MovTitle = (from g in db.movies
                                  where g.title == title
                                  select g.movieid).ToList();

            if (query_MovTitle.Count == 0)
            {
                number_of_movies++;

                movie new_movie = new movie     	// заполнение таблицы фильмов
                {
                    movieid = number_of_movies,     // увеличиваем на 1 счёткик ID фильмов
                    title = cmbTitleFilmTab3.Text,
                    director = txbDirectorTab3.Text,
                    genre = txbGenerTab3.Text,
                    year_movie = int.Parse(txbYearTab3.Text),
                    duration = TimeSpan.Parse(txbDurationTab3.Text),
                    cast_movie = rtbCastTab3.Text
                };

                db.movies.Add(new_movie);
            }
            else
            {
                number_of_movies = query_MovTitle.First();
            }

            try
            {
                number_of_showtimes = db.showtimes.Max(n => n.showtime_id) + 1; //находим мак. индекс записи в таблице для доб.новой записи
            }
            catch
            {
                number_of_showtimes = 1;
            }

            showtime new_showtime = new showtime
            {
                showtime_id = number_of_showtimes,
                movieid = number_of_movies,
                price = (int)(float.Parse(txbPriceTab3.Text) * ((100 - float.Parse(cmbDiscountTab3.Text.Remove(2, 1))) / 100)), //скидка на цену
                date = dateTimePicker1.Value,
                time = TimeSpan.Parse(cmbTimeTab3.Text) 
            };

            string hall = cmbHallTab3.Text; //выбор id по названию

            var query_hall = (from g in db.halls
                              where g.hall_name == hall
                              select g.hall_id).Distinct();

            int hall_ID = query_hall.First();

            var query_seats = (from g in db.halls
                               where g.hall_id == hall_ID
                               select g.seats).Distinct();

            string seats_toDB = query_seats.First();

            try
            {
                number_of_hSht = db.hall_showtime.Max(n => n.hsh_id) + 1;   //Запись с max ID билета,чтобы записать новый с ID на 1 больше
            }
            catch
            {
                number_of_hSht = 1;
            }

            hall_showtime new_hSht = new hall_showtime
            {
                hall_id = hall_ID,
                showtime_id = number_of_showtimes,
                hsh_id = number_of_hSht,
                seat_map = seats_toDB,      // записываем пустой зал из таблицы залов
            };

            // проверяем не является ли запрос добавлением скидки. Новое время и старое должны совпадать
            var time = TimeSpan.Parse(cmbTimeTab3.Text);
       

            var query_sho = (from g in db.showtimes
                             where g.showtime_id == number_of_hSht - 1
                             select g.time).Distinct();

            string time12 = query_sho.First().ToString();


            if (time12 == cmbTimeTab3.Text)
            {
                db.showtimes.Add(new_showtime);
                db.hall_showtime.Add(new_hSht);
            }
            else
            {
                //--------------------------- обновляем только цену при добавлении скидки------------------//
                int priceAfterDiscount = (int)(float.Parse(txbPriceTab3.Text) * ((100 - float.Parse(cmbDiscountTab3.Text.Remove(2, 1))) / 100));
                
                int shoID = 0;
                string time_1 = cmbTimeTab3.Text.ToString();

                // запрос showtimeID по фильму
                var querySho = (from showtime in db.showtimes
                             orderby showtime.movieid
                             where (showtime.time.ToString() == time_1 && showtime.movieid == number_of_movies)
                             select new { showtime.showtime_id }).Distinct();

                foreach (var it in querySho)
                {
                    shoID = it.showtime_id;
                }

                db.Configuration.ValidateOnSaveEnabled = false;
                //MessageBox.Show(number_of_showtimes.ToString() + "  " + number_of_movies.ToString());
                var verifcation = db.showtimes.Where(x => x.showtime_id == shoID && x.movieid == number_of_movies).FirstOrDefault();
                verifcation.price = priceAfterDiscount;
                MessageBox.Show("Скидка записана");
                //					
            }

            db.SaveChanges();

            return 0;
        }

        public (string, string) loginFunc(string login, string password) // метод в классе для входа  логина и пароля
        {
            var temp_login = (from r in db.roles
                              select new { r.role1, r.login, r.password }).ToList();

            foreach (var item in temp_login.OrderBy(o => o.role1))
            {
                if (login == item.login && password == item.password)
                {
                    return (item.role1.ToString(), item.login.ToString());
                }
            }

            return ("failed", "no data");    // неверная попытка входа
        }

        public void selectTimeByMovieTitle(ref ComboBox cmbTime, ref ComboBox cmbTitleFilmTab1, ref ComboBox cmbHall, ref TextBox txbDirectorTab1, 
            ref TextBox txbYearTab1, ref TextBox txbGenerTab1,  ref TextBox txbDurationTab1, ref RichTextBox rtbCastTab1, ref Label lblPrice)
        {
            //заполняет combobox времени после выбора названия фильма
            int[] sh_ID = new int[10];
            int i = 0;
            int temp = 0;
            string title = cmbTitleFilmTab1.Text;

            // запрос поиска с отбором по названию фильма
            var query = (from movie in db.movies
                         join st in db.showtimes on movie.movieid equals st.movieid
                         orderby movie.title
                         where (movie.title == title)
                         select new { movie.title, movie.duration, movie.director, movie.genre, movie.cast_movie, st.time, st.showtime_id, st.price, movie.year_movie }).Distinct();

            // заполнение combobox о фильме           
            foreach (var item in query.OrderBy(o => o.title))
            {
                i++;
                sh_ID[i] = item.showtime_id;
                cmbTime.Items.Add(item.time.ToString());
                txbDirectorTab1.Text = item.director;
                txbGenerTab1.Text = item.genre;
                txbYearTab1.Text = item.year_movie.ToString();
                txbDurationTab1.Text = item.duration.ToString().Remove(5, 3);
                rtbCastTab1.Text = item.cast_movie;
                lblPrice.Text = item.price.ToString() + " p.";
            }

            while (i > 0)
            {
                temp = sh_ID[i];

                var query1 = (from g in db.hall_showtime
                              where g.showtime_id == temp
                              select g.hall_id).Distinct();

                var query2 = (from g in db.halls
                              where g.hall_id == query1.FirstOrDefault()
                              orderby g.hall_name
                              select new { g.hall_name }).Distinct();

                foreach (var item in query2)
                {
                    // если в combobox уже внесён этот зал то мы его не добавляем
                    if (!cmbHall.Items.Contains(item.hall_name))
                    {
                        cmbHall.Items.Add(item.hall_name);
                    }
                }

                i--;
            }
        }
    }
}
