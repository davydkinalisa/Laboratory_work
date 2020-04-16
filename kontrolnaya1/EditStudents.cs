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
    public partial class EditStudents : Form
    {
        public contEntities db = new contEntities();
        s_students item;
        public EditStudents (s_students stud)
        {
            item = stud;
            InitializeComponent();

            var groups_for_list = (from g in db.s_in_group
                                   select g.group_num.ToString()).Distinct();
            foreach (string it in groups_for_list)
            {
                comboBox1.Items.Add(it);
            }

            textBox1.Text = item.surname.ToString();
            textBox2.Text = item.name.ToString();

        }


        public EditStudents()
        {
            InitializeComponent();
        }

        private void EditStudents_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var result = ((Edit)Owner).db.s .SingleOrDefault(w => w.code_stud == item.code_stud);
            //result.surname = textBox1.Text.ToString();
            //result.name = textBox2.Text.ToString();
            //var query = (from g in db.groups
            //             where g.name_group == comboBox1.SelectedItem.ToString()
            //             select g.code_group).ToList();

            //result.code_group = query[0];
            //((Form1)Owner).db.SaveChanges();
            //((Form1)Owner).studentsheet = ((Form1)Owner).db.students.OrderBy(o => o.code_stud).ToList();
            //this.Close();
        }
    }
}
