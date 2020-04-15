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

        public void button1_Click(object sender, EventArgs e)
        {

            User usr = new User(textBox1.Text, textBox2.Text);
            string Title;

            if (usr.Check().Item1 == true)
            {
                Title = usr.Check().Item2;

                Form2 frm = new Form2(this, Title);
                frm.Show();
            }
            else
            {
                MessageBox.Show("Введен неверный пароль!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
    }
}
