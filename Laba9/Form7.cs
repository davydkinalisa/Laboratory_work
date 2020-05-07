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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();

            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            // no larger than screen size
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'BooksDataSet1.authors' table. You can move, or remove it, as needed.
            this.authorsTableAdapter.Fill(this.BooksDataSet1.authors);
            this.reportViewer1.RefreshReport();
        }
    }
}
