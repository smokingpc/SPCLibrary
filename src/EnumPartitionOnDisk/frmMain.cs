using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnumPartitionOnDisk
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public void InitUI()
        {
            cbDiskList.Items.Clear();
            InitDiskList();
        }
        public void InitDiskList()
        {
            try
            {
                cbDiskList.Items.Clear();
                cbDiskList.DisplayMember = "PhyDiskName";
                cbDiskList.Items.AddRange(EnumPhysicalDisks());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cbDiskList.Items.Count > 0)
                    cbDiskList.SelectedIndex = 0;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitUI();
        }

        private void cbDiskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            CPhyDisk disk = (CPhyDisk)cbDiskList.SelectedItem;
            if (null != disk)
            {
                PrintPartitionInfo(disk);
            }
            if (textBox1.Text.Length == 0)
                textBox1.Text = "Physical Disk not found...\r\n";
        }
    }
}
