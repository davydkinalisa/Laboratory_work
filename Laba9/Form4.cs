using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba9
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void publisherBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.publisherBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.booksDataSet);

        }

        private void Form4_Load(object sender, EventArgs e)
        {

            try
            { 
             this.publisherTableAdapter.Fill(this.booksDataSet.publisher);
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных!");
                this.Close();
            }

        }
    }
}
