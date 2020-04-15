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
using NPOI.XSSF.UserModel;

namespace EF_linq_laba
{
    public partial class FormAcademicPerformance : Form
    {
        public demo1Entities db = new demo1Entities();


        public List<progress> progressSheet;
        DataTable dt = new DataTable();
        public FormAcademicPerformance()
        {
            InitializeComponent();

            var students_for_list = (from g in db.students
                                     select g.surname).Distinct();
            //var students_for_list = (from stud in progressSheet
            //             join g in db.students on stud.code_stud equals g.code_stud
            //             join e in db.subjects on stud.code_subject equals e.code_subject
            //             orderby g.surname
            //             select new {g.surname}).Distinct();

            foreach (string it in students_for_list)
            {
                comboBox1.Items.Add(it);
            }
            var subject_for_list = (from s in db.subjects
                                    select s.name_subject).Distinct();
            foreach (string it in subject_for_list)
            {
                comboBox3.Items.Add(it);
            }
            var lectors_for_list = (from h in db.lectors
                                    select h.name_lector).Distinct();
            foreach (string it in lectors_for_list)
            {
                comboBox2.Items.Add(it);
            }


            loadData();



        }
        public void loadData()
        {
            progressSheet = (from prog in db.progresses
                             select prog).ToList();

            var query = (from stud in progressSheet
                         join g in db.students on stud.code_stud equals g.code_stud
                         join e in db.subjects on stud.code_subject equals e.code_subject
                         join h in db.lectors on stud.code_lector equals h.code_lector
                         orderby g.surname
                         select new { stud.idpro, g.surname, e.name_subject, h.name_lector, stud.date_exam, stud.estimate }).ToList();
            dataGridView1.DataSource = query;
            dataGridView1.Columns[0].HeaderText = "ID оценки";
            dataGridView1.Columns[1].HeaderText = "Фамилия студента";
            dataGridView1.Columns[2].HeaderText = "Название предмета";
            dataGridView1.Columns[3].HeaderText = "ФИО преподавателя";
            dataGridView1.Columns[4].HeaderText = "Дата экзамена";
            dataGridView1.Columns[5].HeaderText = "Оценка";
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[0].Visible = false;
            if (dataGridView1.RowCount == 0) label1.Visible = true;
            else label1.Visible = false;
        }
        private void FormAcademicPerformance_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = (from stud in progressSheet
                         join g in db.students on stud.code_stud equals g.code_stud
                         join s in db.subjects on stud.code_subject equals s.code_subject
                         join h in db.lectors on stud.code_lector equals h.code_lector
                         orderby g.surname
                         select new { g.surname, s.name_subject, h.name_lector, stud.date_exam, stud.estimate }).ToList();
            dataGridView1.DataSource = query.Where(p => p.surname.ToString() == comboBox1.Text.ToString()).ToList();

            dataGridView1.Update();
            if (dataGridView1.RowCount == 0) label1.Visible = true; else label1.Visible = false;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = (from stud in progressSheet
                         join g in db.students on stud.code_stud equals g.code_stud
                         join s in db.subjects on stud.code_subject equals s.code_subject
                         join h in db.lectors on stud.code_lector equals h.code_lector
                         orderby s.name_subject
                         select new { g.surname, s.name_subject, h.name_lector, stud.date_exam, stud.estimate }).ToList();
            dataGridView1.DataSource = query.Where(p => p.name_subject.ToString() == comboBox3.Text.ToString()).ToList();
            dataGridView1.Update();
            if (dataGridView1.RowCount == 0) label1.Visible = true; else label1.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var query = (from stud in progressSheet
                         join g in db.students on stud.code_stud equals g.code_stud
                         join s in db.subjects on stud.code_subject equals s.code_subject
                         join h in db.lectors on stud.code_lector equals h.code_lector
                         orderby g.surname
                         select new { g.surname, s.name_subject, h.name_lector, stud.date_exam, stud.estimate }).ToList();

            dataGridView1.DataSource = query.Where(p => (p.surname.ToString() == comboBox1.Text.ToString())
            && (p.name_subject.ToString() == comboBox3.Text.ToString())
            && p.name_lector.ToString() == comboBox2.Text.ToString()).ToList();


            dataGridView1.Update();
            if (dataGridView1.RowCount == 0) label1.Visible = true; else label1.Visible = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = (from stud in progressSheet
                         join g in db.students on stud.code_stud equals g.code_stud
                         join e1 in db.subjects on stud.code_subject equals e1.code_subject
                         join h in db.lectors on stud.code_lector equals h.code_lector
                         orderby g.surname
                         select new { g.surname, e1.name_subject, h.name_lector, stud.date_exam, stud.estimate }).ToList();
            dataGridView1.DataSource = query.Where(p => p.name_lector.ToString() == comboBox2.Text.ToString()).ToList();
            dataGridView1.Update();
            if (dataGridView1.RowCount == 0) label1.Visible = true; else label1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog(); dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); dialog.DefaultExt = ".xls";
            dialog.Filter =
                "Ta6лицы Excel (*.xls)|*.xls|Bce файлы (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.FileName = "Отчёт";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var file = new FileStream(dialog.FileName, FileMode.Create, FileAccess.ReadWrite);
                var query = (from stud in progressSheet
                             join g in db.students on stud.code_stud equals g.code_stud
                             join i in db.subjects on stud.code_subject equals i.code_subject
                             join h in db.lectors on stud.code_lector equals h.code_lector
                             orderby g.surname
                             select new { g.surname, i.name_subject, h.name_lector, stud.date_exam, stud.estimate }).ToList();

                var template = new MemoryStream(Properties.Resources.Template_AP, true);
                var workbook = new XSSFWorkbook(template);
                var sheetl = workbook.GetSheet("Лист1");

                int row = 7;

                foreach (var item in query.OrderBy(o => o.surname))
                {
                    var rowInsert = sheetl.CreateRow(row);
                    rowInsert.CreateCell(column: 1).SetCellValue(item.surname);
                    rowInsert.CreateCell(column: 2).SetCellValue(item.name_subject);
                    rowInsert.CreateCell(column: 3).SetCellValue(item.name_lector);
                    rowInsert.CreateCell(column: 4).SetCellValue(item.date_exam.ToString("MM'/'dd yyyy"));
                    rowInsert.CreateCell(column: 5).SetCellValue(item.estimate.ToString());


                    row++;
                }
                //MessageBox.Show(("raw"+ row);
                workbook.Write(file);
                file.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = this.dataGridView1.Rows[selectedrow];
            var idpr = Convert.ToInt32(row.Cells[0].Value.ToString());
            var verifcation = db.progresses.Where(x => x.idpro == idpr).FirstOrDefault();
            Form2 nb = new Form2();
            nb.txtName.Text = verifcation.student.name.ToString() +" " + verifcation.student.surname.ToString();
            nb.CbEstimate.Text = verifcation.estimate.ToString();
            nb.cbDisp.Text = verifcation.subject.name_subject.ToString();
            nb.codePro = verifcation.idpro;
            nb.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            var verification = selectedrow;
            DataGridViewRow row = this.dataGridView1.Rows[selectedrow];
            var idpr = Convert.ToInt32(row.Cells[0].Value.ToString());
            var de = db.progresses.Where(x => x.idpro == idpr).FirstOrDefault();
            db.Entry(de).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            MessageBox.Show("Удалено");
        }
        int selectedrow;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedrow = e.RowIndex;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormAddEstimate addEst = new FormAddEstimate();
            addEst.Owner = this;
            addEst.Show();
        }
    }
}
