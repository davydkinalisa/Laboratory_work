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
    public partial class Form4 : Form
    {
        public postcardEntities db = new postcardEntities();
        public List<postcard> studentsheet;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            studentsheet = (from psd in db.postcards
                            select psd).ToList();

            var query = (from psd in studentsheet
                         select new { psd.postcard_id, psd.senser_email, psd.receiver_email, psd.picname, psd.text_for_postcard }).ToList();

            dataGridView1.DataSource = query;
            dataGridView1.Columns[0].HeaderText = "Имя отправителя";
            dataGridView1.Columns[1].HeaderText = "арес отправителя";
            dataGridView1.Columns[2].HeaderText = "адрес получатея";
            dataGridView1.Columns[3].HeaderText = "Текст поздравления";
            dataGridView1.ReadOnly = true;
        }
    }
}
