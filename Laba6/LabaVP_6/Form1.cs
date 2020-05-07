using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace LabaVP_6
{
    public partial class Form1 : Form
    {

        //public class FileStream : System.IO.Stream;
            
        public Form1()
        {
            InitializeComponent();
        }

        public void form1_Visible (bool value)
        {
            this.Visible = value;

        }
        

        public void button1_Click(object sender, EventArgs e)
        {

            User usr = new User(textBox1.Text, textBox2.Text);
            string Title;

            if (usr.Check().Item1 == true)
            {
                Title = usr.Check().Item2;

                Form2 frm = new Form2(this, Title);

                form1_Visible(false);

                textBox1.Clear();
                textBox2.Clear();

                frm.Show();
            }
            else
            {
                MessageBox.Show("Введен неверный пароль!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string temp = textBox1.Text.Substring(Math.Max(0, textBox1.Text.Length - 1));

            if (temp == " " || temp == "_" || temp == "@" || temp == "#" || temp == "1" || temp == "2" || temp == "3" || temp == "4" || temp == "5" || temp == "6" || temp == "7" || temp == "8" || temp == "9" || temp == "0")
            {

                textBox1.Text = textBox1.Text.Remove((textBox1.Text.Length - 1), 1);
                textBox1.SelectionStart = textBox1.Text.Length;

                MessageBox.Show("Restricted character enterred!");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        class User
        {

            public string Login { get; set; }
            public string Password { get; set; }

            public User(string login, string password)
            {
                Login  = login;
                Password = password;

            }
            public (bool, string) Check()
            {
                StreamReader reader = new StreamReader("users.txt");
                string line, title = "Empty";
                bool result = false;

                while ((line = reader.ReadLine()) != null)
                {
                    if (((line.Split(','))[0]==Login) && ((line.Split(','))[1]==Password))
                    {
                        result = true;
                        title = (line.Split(','))[2];  
                        
                    }
                }

                reader.Close();
                reader.Dispose();
                return (result, title);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

            // no larger than screen size
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               button1_Click(this, null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
