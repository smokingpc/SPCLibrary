using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitAndFormatPhysicalDisk
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public void InitDiskList() 
        {
            try
            {
                cbDiskList.Items.Clear();
                cbDiskList.DisplayMember = "DiskDevName";
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

        public void ShowDevPath(CPhyDisk disk) 
        {
            label2.Text = disk.DevPath;
        }

        public void InitUI() 
        {
            label2.Text = "(Please Select PhysicalDisk First!!)";
            InitDiskList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitUI();
        }

        private void cbDiskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CPhyDisk disk = (CPhyDisk)cbDiskList.SelectedItem;
            if (null != disk)
            {
                ShowDevPath(disk);
            }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            return;
            CPhyDisk disk = (CPhyDisk)cbDiskList.SelectedItem;
            if (null != disk)
                CleanupDisk(disk);
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            return;
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            return;
        }
    }
}
