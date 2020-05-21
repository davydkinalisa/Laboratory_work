using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kontrolnaya2
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            prepareNextForm();
        }


        void prepareNextForm()
        {
            string DataToForm2 = "";
            string email = null;

            try
            {
                var eMailValidator = new System.Net.Mail.MailAddress(tbxSender.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("неправильный Email");
            }

            if (radioButton1.Checked == true)
            {
                DataToForm2 += "Pic1";
            }
            else if (radioButton2.Checked == true)
            {
                DataToForm2 += "Pic2";
            }
            else if (radioButton3.Checked == true)
            {
                DataToForm2 += "Pic3";
            }

            DataToForm2 += ",";
            DataToForm2 += tbxName.Text + "," + tbxSender.Text + "," + tbxReceiver.Text + "," + richTextBox1.Text;

 
            Form2 frm = new Form2(DataToForm2);
            frm.Owner = this;
            frm.Show();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4();
            frm.Show();
        }
    }
}
