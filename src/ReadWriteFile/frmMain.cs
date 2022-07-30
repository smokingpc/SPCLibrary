using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadWriteFile
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public void InitUI()
        {
            textBox1.Text = textBox2.Text = "";
            radioButton1.Checked = true;
            radioButton2.Checked = false;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            InitUI();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label2.Visible = textBox2.Visible = true;
            label3.Visible = label4.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label2.Visible = textBox2.Visible = false;
            label3.Visible = label4.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (0 == textBox1.Text.Length)
            {
                MessageBox.Show("Please select file first!");
                return;
            }

            if (true == radioButton1.Checked)
            {
                int read_len = 8;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "All files(*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                //read first 8 bytes only
                    byte[] data = DoReadFile(textBox1.Text, read_len);
                    if (data != null)
                    {
                        textBox2.Text = data.ToHexString();
                        MessageBox.Show($"Read {read_len} bytes from specified file done!");
                    }
                }
            }
            else if (true == radioButton2.Checked)
            {
                int read_len = label4.Text.Length;
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "All files(*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    byte[] data = Encoding.UTF8.GetBytes(label4.Text);
                    DoWriteFile(textBox1.Text, data);
                    MessageBox.Show($"Write {data.Length} bytes to file done!");
                }
            }
            else
            {
                MessageBox.Show("radio button select error!");
            }
        }
    }
}
