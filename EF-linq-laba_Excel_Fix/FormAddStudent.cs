using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF_linq_laba
{
    public partial class FormAddStudent : Form
    {
        public demo1Entities db = new demo1Entities();
        public FormAddStudent()
        {
            InitializeComponent();
            var groups_for_list = (from g in db.groups
                                   select g.name_group).Distinct();
            foreach (string it in groups_for_list)
            {
                comboBox1.Items.Add(it);
            }
        }

        private void FormAddStudent_Load(object sender, EventArgs e)
        {
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

            // no larger than screen size
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0 || textBox2.TextLength == 0)
            {
                MessageBox.Show("Поле оставлено пустым, данные не могут быть добавлены");
            }

            else
            {
                var query = (from g in db.groups
                             where g.name_group == comboBox1.SelectedItem.ToString()
                             select g.code_group).ToList();

                int number_of_student = db.students.Max(n => n.code_stud) + 1;

                student new_student = new student { code_stud = number_of_student, surname = textBox1.Text, name = textBox2.Text, code_group = query[0] };
                db.students.Add(new_student);

                db.SaveChanges();
                this.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string temp = textBox1.Text.Substring(Math.Max(0, textBox1.Text.Length - 1));

            if (temp == "_" || temp == "@" || temp == "#" || temp == "1" || temp == "2" || temp == "3" || temp == "4" || temp == "5" || temp == "6" || temp == "7" || temp == "8" || temp == "9" || temp == "0")
            {

                textBox1.Text = textBox1.Text.Remove((textBox1.Text.Length - 1), 1);
                textBox1.SelectionStart = textBox1.Text.Length;

                MessageBox.Show("Введён недопустимый символ!");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            string temp = textBox2.Text.Substring(Math.Max(0, textBox2.Text.Length - 1));

            if (temp == "_" || temp == "@" || temp == "#" || temp == "1" || temp == "2" || temp == "3" || temp == "4" || temp == "5" || temp == "6" || temp == "7" || temp == "8" || temp == "9" || temp == "0")
            {

                textBox2.Text = textBox2.Text.Remove((textBox2.Text.Length - 1), 1);
                textBox2.SelectionStart = textBox2.Text.Length;

                MessageBox.Show("Введён недопустимый символ!");
            }
        }
    }
}
