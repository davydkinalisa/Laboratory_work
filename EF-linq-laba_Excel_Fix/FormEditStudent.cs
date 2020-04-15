using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace EF_linq_laba
{
    public partial class FormEditStudent : Form
    {
        public demo1Entities db = new demo1Entities();
        student item;
        public FormEditStudent(student stud)
        {
            item = stud;
            InitializeComponent();
            var groups_for_list = (from g in db.groups
                                   select g.name_group).Distinct();
            foreach (string it in groups_for_list)
            {
                comboBox1.Items.Add(it);

            }
            textBox1.Text = item.surname.ToString();
            textBox2.Text = item.name.ToString();

            var query = (from g in db.groups
                         where g.code_group == item.code_group
                         select g.name_group).ToList();
            comboBox1.SelectedItem = query[0];
        }

        private void FormEditStudent_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = ((Form1)Owner).db.students.SingleOrDefault(w => w.code_stud == item.code_stud);
            result.surname = textBox1.Text.ToString();
            result.name = textBox2.Text.ToString();
            var query = (from g in db.groups
                         where g.name_group == comboBox1.SelectedItem.ToString()
                         select g.code_group).ToList();

            result.code_group = query[0];
            ((Form1)Owner).db.SaveChanges();
            ((Form1)Owner).studentsheet = ((Form1)Owner).db.students.OrderBy(o => o.code_stud).ToList();
            this.Close();
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        //обработка неправильно введенных символов
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string temp = textBox1.Text.Substring(Math.Max(0, textBox1.Text.Length - 1));

            if (temp == " " || temp == "_" || temp == "@" || temp == "#" || temp=="1" || temp == "2" || temp == "3" || temp == "4" || temp == "5" || temp == "6" || temp == "7" || temp == "8" || temp == "9" || temp == "0")
            {

                textBox1.Text = textBox1.Text.Remove((textBox1.Text.Length - 1), 1);
                textBox1.SelectionStart = textBox1.Text.Length;

                MessageBox.Show("Restricted character enterred!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var rows = from o in db.students
                        //where o.code_stud == 9
                       where (o.surname == textBox1.Text.ToString() && o.name == textBox2.Text.ToString())
                       select o;

            foreach (var row in rows)
            {
                db.students.Remove(row);
            }

            db.SaveChanges();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
