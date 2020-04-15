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

        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
