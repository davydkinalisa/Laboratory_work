using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema_VP
{
    public partial class TicketDisplay : Form
    {
        public TicketDisplay(tckFRM tckFRM)
        {
            InitializeComponent();

            lblTitle.Text = tckFRM.title;
            lblHall.Text = tckFRM.hall;
            lblNumber.Text = tckFRM.number;
            lblRow.Text = tckFRM.row;
            lblSeat.Text = tckFRM.seat;
            lblTime.Text = tckFRM.time.Remove(5,3);
            lblPrice.Text = tckFRM.price;
        }
    }
}
