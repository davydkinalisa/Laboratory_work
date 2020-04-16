using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kontrolnaya1
{
    public partial class Spisok_studentov : Form
    {
        public contEntities db = new contEntities();
        public List<s_students> studentsheet;


        public Spisok_studentov()
        {
            InitializeComponent();

            var groups_for_list = (from g in db.s_in_group
                                   select g.group_num.ToString()).Distinct();
            foreach (string it in groups_for_list)
            {
                comboBox1.Items.Add(it);
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            studentsheet = (from stud in db.s_students
                            select stud).ToList();


            var query = (from s_students in studentsheet
                         join g in db.s_in_group on s_students.id_group equals g.id_group
                         orderby g.group_num
                         select new { s_students.id, s_students.surname, s_students.name, s_students.middlename, g.kurs_num, g.group_num }).ToList();

            dataGridView1.DataSource = query;
            dataGridView1.Columns[0].HeaderText = "Номер зачётной книжки";
            dataGridView1.Columns[1].HeaderText = "Фамилия";
            dataGridView1.Columns[2].HeaderText = "Имя";
            dataGridView1.Columns[3].HeaderText = "Отчество";
            dataGridView1.Columns[4].HeaderText = "Номер курса";
            dataGridView1.Columns[5].HeaderText = "Номер группы";
            dataGridView1.ReadOnly = true;

            if (dataGridView1.RowCount == 0) label1.Visible = true;
            else label1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var query = (from s_students in studentsheet
                         join g in db.s_in_group on s_students.id_group equals g.id_group
                         orderby g.group_num
                         select new { s_students.id, s_students.surname, s_students.name, s_students.middlename, g.kurs_num, g.group_num }).ToList();

            dataGridView1.DataSource = query.Where(p => (p.surname.ToString() == textBox1.Text.ToString())
                || (p.kurs_num.ToString() == textBox2.Text.ToString())
                || p.group_num.ToString() == textBox3.Text.ToString()).ToList();



            dataGridView1.Update();
            if (dataGridView1.RowCount == 0) label1.Visible = true; else label1.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Spisok_studentov_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id = int.Parse(comboBox1.Text);

            var query = (from g in db.s_students
                         where g.id_group == id
                         select g.id_group).ToList();


            int number_of_student = db.s_students.Max(n => n.id) + 1;

            id = int.Parse(comboBox1.Text);
            var query1 = (from g in db.s_in_group
                          where g.group_num == id
                          select g.id_group).ToList();

            s_students new_student = new s_students { id = number_of_student, surname = textBox4.Text, name = textBox5.Text, middlename = textBox6.Text, id_group = query1[0] };
            db.s_students.Add(new_student);

            db.SaveChanges();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string temp = textBox1.Text.Substring(Math.Max(0, textBox1.Text.Length - 1));

            if (temp == " " || temp == "_" || temp == "@" || temp == "#" || temp == "1" || temp == "2" || temp == "3" || temp == "4" || temp == "5" || temp == "6" || temp == "7" || temp == "8" || temp == "9" || temp == "0")
            {

                textBox1.Text = textBox1.Text.Remove((textBox1.Text.Length - 1), 1);
                textBox1.SelectionStart = textBox1.Text.Length;

                MessageBox.Show("Restricted character enterred!");
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string temp = textBox1.Text.Substring(Math.Max(0, textBox1.Text.Length - 1));

            if (temp == " " || temp == "_" || temp == "@" || temp == "#" || temp == "1" || temp == "2" || temp == "3" || temp == "4" || temp == "5" || temp == "6" || temp == "7" || temp == "8" || temp == "9" || temp == "0")
            {

                textBox1.Text = textBox1.Text.Remove((textBox1.Text.Length - 1), 1);
                textBox1.SelectionStart = textBox1.Text.Length;

                MessageBox.Show("Restricted character enterred!");
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string temp = textBox1.Text.Substring(Math.Max(0, textBox1.Text.Length - 1));

            if (temp == " " || temp == "_" || temp == "@" || temp == "#" || temp == "1" || temp == "2" || temp == "3" || temp == "4" || temp == "5" || temp == "6" || temp == "7" || temp == "8" || temp == "9" || temp == "0")
            {

                textBox1.Text = textBox1.Text.Remove((textBox1.Text.Length - 1), 1);
                textBox1.SelectionStart = textBox1.Text.Length;

                MessageBox.Show("Restricted character enterred!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<s_students> query = (from stud in db.s_students
                                      select stud).ToList();

            if (dataGridView1.SelectedCells.Count == 1)
            {
                if (Application.OpenForms.Count == 2)
                {
                    s_students item = query.First(w => w.id.ToString() == dataGridView1.SelectedCells[0]
                      .OwningRow.Cells[0].Value.ToString());

                    EditStudents editSt = new EditStudents(item);
                    editSt.Owner = this;
                    editSt.Show();

                }
                else Application.OpenForms[0].Focus();
            }
        }

    }
}
    


