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
    public partial class Form2 : Form
    {
        demo1Entities db = new demo1Entities();
        public Form2()
        {
            InitializeComponent();
        }
        public int codePro;
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int newEs = int.Parse(CbEstimate.Text);
            int codeP = codePro;
            db.Configuration.ValidateOnSaveEnabled = false;
            var verifcation = db.progresses.Where(x => x.idpro == codeP).FirstOrDefault();
            verifcation.estimate = newEs;
            verifcation.idpro = codeP;
            db.SaveChanges();
            MessageBox.Show("Оценка изменена");
            FormAcademicPerformance fa = new FormAcademicPerformance();
            fa.dataGridView1.ClearSelection();
            fa.loadData();
            fa.Show();
            this.Close();

        }

        private void cbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cbDisp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
