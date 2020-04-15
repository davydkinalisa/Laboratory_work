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

namespace LabaVP_6
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 a, string Title)
        {
            InitializeComponent();

            this.textBox1.Text = a.textBox1.Text;
            this.textBox2.Text = a.textBox2.Text;
            this.Text = Title;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("users.txt");
            string line;
            string[] linesOfFile;
            int lineIndex = 0;

            if ((this.Text == "Пользователь" && textBox1.Text == "pass") || (this.Text == "Администратор"))
            {

                while ((line = reader.ReadLine()) != null)
                {
                    lineIndex++;

                    if (((line.Split(','))[0] == textBox1.Text) && ((line.Split(','))[1] == textBox2.Text))
                    {

                        linesOfFile = System.IO.File.ReadAllLines("users.txt");

                        reader.Close();
                        reader.Dispose();
                        File.Create("users.txt").Dispose();
                        StreamWriter writer = new StreamWriter("users.txt");

                        linesOfFile[lineIndex - 1] = textBox1.Text + "," + textBox3.Text + "," + ((line.Split(','))[2]);

                        foreach (string s in linesOfFile)
                            writer.WriteLine(s);

                        //writer.WriteLine(linesOfFile);
                        writer.Close();

                        break;

                    }
                }
               
            }
            else 
            {

                MessageBox.Show("Вы не имеете право на это и точка!");
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
