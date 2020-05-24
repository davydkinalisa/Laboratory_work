﻿using System;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void authorsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.authorsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.booksDataSet);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            try
            {
                this.authorsTableAdapter.Fill(this.booksDataSet.authors);
            }

            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных!");
                this.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form7 a = new Form7();
            a.Show();
        }
    }
}