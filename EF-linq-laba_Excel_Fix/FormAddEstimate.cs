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


namespace EF_linq_laba
{
    public partial class FormAddEstimate : Form
    {
        public demo1Entities db = new demo1Entities();
        public List<student> studentsheet;

        public FormAddEstimate()
        {
            InitializeComponent();

            var students_for_list = (from g in db.students
                                     select g.surname.ToString()).Distinct();

            foreach (string it in students_for_list)
            {
                comboBox1.Items.Add(it);
            }

            var subjects_for_list = (from g in db.subjects
                                     select g.name_subject.ToString()).Distinct();

            foreach (string it in subjects_for_list)
            {
                comboBox2.Items.Add(it);
            }

            var lectors_for_list = (from g in db.lectors
                                     select g.name_lector.ToString()).Distinct();

            foreach (string it in lectors_for_list)
            {
                comboBox4.Items.Add(it);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int number_of_progress = db.progresses.Max(n => n.idpro) + 1;
            int code_student, code_lector, code_subject;
            
            var query = (from g in db.students
                         where g.surname == (comboBox1.SelectedItem.ToString())
                         select g.code_stud).ToList();

            code_student = query.First();

            var query1 = (from g in db.lectors
                         where g.name_lector == comboBox4.SelectedItem.ToString()
                         select g.code_lector).ToList();

            code_lector = query1.First();

            var query2 = (from g in db.subjects
                          where g.name_subject == comboBox2.SelectedItem.ToString()
                          select g.code_subject).ToList();

            code_subject = query2.First();

            //progress new_progress = new progress { idpro = number_of_progress, code_stud = int.Parse(comboBox1.Text), code_lector = int.Parse(comboBox4.Text),
            //    code_subject = int.Parse(comboBox2.Text), estimate = int.Parse(comboBox3.Text)};
            progress new_progress = new progress
            {
                idpro = number_of_progress,
                code_stud = code_student,
                code_lector = code_lector,
                code_subject = code_subject,
                estimate = int.Parse(comboBox3.Text),
                date_exam = DateTime.Now
            };

            db.progresses.Add(new_progress);
            db.SaveChanges();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
