using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.XSSF.UserModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;

namespace EF_linq_laba
{
    public partial class Form1 : Form
    {
        public demo1Entities db = new demo1Entities();
        public List<student> studentsheet;
        
        public Form1()
        {
            InitializeComponent();
            studentsheet = (from stud in db.students
                            select stud).ToList();

            var query = (from stud in studentsheet
                         join g in db.groups on stud.code_group equals g.code_group
                         orderby stud.code_stud
                         select new { stud.code_stud, stud.surname, stud.name, g.name_group, stud.code_group }).ToList();
            dataGridView1.DataSource = query;
            dataGridView1.Columns[0].HeaderText = "Номер студента";
            dataGridView1.Columns[1].HeaderText = "Фамилия";
            dataGridView1.Columns[2].HeaderText = "Имя";
            dataGridView1.Columns[3].HeaderText = "Номер группы";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.ReadOnly = true;

            if (dataGridView1.RowCount == 0) label1.Visible = true;
            else label1.Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var query = (from stud in db.students
                         join g in db.groups on stud.code_group equals g.code_group
                         orderby stud.code_stud
                         select new { stud.code_stud, stud.surname, stud.name, g.name_group, stud.code_group }).ToList();
            if (textBox1.Text != "")
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        dataGridView1.DataSource = query.Where(p => p.code_stud.ToString() == textBox1.Text.ToString()).ToList(); break;
                    case 1:
                        dataGridView1.DataSource = query.Where(p => p.surname.ToString() == textBox1.Text.ToString()).ToList(); break;
                    case 2:
                        dataGridView1.DataSource = query.Where(p => p.name.ToString() == textBox1.Text.ToString()).ToList(); break;
                    case 3:
                        dataGridView1.DataSource = query.Where(p => p.name_group.ToString() == textBox1.Text.ToString()).ToList(); break;
                }
            }
            else
            {
                dataGridView1.DataSource = query;
            }
            dataGridView1.Update();
            if (dataGridView1.RowCount == 0) label1.Visible = true; else label1.Visible = false;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<student> query = (from stud in db.students
                                   select stud).ToList();

            if (dataGridView1.SelectedCells.Count == 1)
            {
                if (Application.OpenForms.Count == 2)
                {
                    student item = query.First(w => w.code_stud.ToString() == dataGridView1.SelectedCells[0]
                      .OwningRow.Cells[0].Value.ToString());

                    FormEditStudent editSt = new FormEditStudent(item);
                    editSt.Owner = this;
                    editSt.Show();

            }
            else Application.OpenForms[0].Focus();
        }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count == 2)
            {
                FormAddStudent addSt = new FormAddStudent();
                addSt.Owner = this;
                addSt.Show();

            }
            else Application.OpenForms[0].Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var query = (from stud in db.students
                         join g in db.groups on stud.code_group equals g.code_group
                         orderby stud.code_stud
                         select new { stud.code_stud, stud.surname, stud.name, g.name_group, stud.code_group }).ToList();
            dataGridView1.DataSource = query;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<student> query = (from stud in db.students
                                   select stud).ToList();
            if (dataGridView1.SelectedCells.Count == 1)
            {
                if (Application.OpenForms.Count == 2)
                {
                    student item = query.First(w => w.code_stud.ToString() == dataGridView1.SelectedCells[0]
                      .OwningRow.Cells[0].Value.ToString());

                    FormAcademicPerformance abc = new FormAcademicPerformance();
                    abc.Owner = this;
                    abc.Show();

                }
                else Application.OpenForms[0].Focus();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog(); dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); dialog.DefaultExt = ".xls";
            dialog.Filter =
                "Ta6лицы Excel (*.xls)|*.xls|Bce файлы (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.FileName = "Отчёт";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var file = new FileStream(dialog.FileName, FileMode.Create, FileAccess.ReadWrite);
                var query = (from stud in db.students
                             join g in db.groups on stud.code_group equals g.code_group
                             orderby stud.code_stud
                             select new {stud.code_stud, stud.surname, stud.name, g.name_group, stud.code_group }).ToList();
                
                var template = new MemoryStream(Properties.Resources.Template_sList, true);
                var workbook = new XSSFWorkbook(template);
                var sheetl = workbook.GetSheet("Лист1");

                int row = 6;

                foreach (var item in query.OrderBy(o => o.code_stud))
                {
                    var rowInsert = sheetl.CreateRow(row);
                    rowInsert.CreateCell(column: 1).SetCellValue(item.code_stud);
                    rowInsert.CreateCell(column: 2).SetCellValue(item.surname);
                    rowInsert.CreateCell(column: 3).SetCellValue(item.name);
                    rowInsert.CreateCell(column: 4).SetCellValue(int.Parse(item.name_group));
                    row++;


                }

                workbook.Write(file);
                file.Close();
            }

        }

    }

}
