
namespace InitAndFormatPhysicalDisk
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbDiskList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnFormat = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Disk List";
            // 
            // cbDiskList
            // 
            this.cbDiskList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiskList.FormattingEnabled = true;
            this.cbDiskList.Location = new System.Drawing.Point(82, 12);
            this.cbDiskList.Name = "cbDiskList";
            this.cbDiskList.Size = new System.Drawing.Size(295, 27);
            this.cbDiskList.TabIndex = 1;
            this.cbDiskList.SelectedIndexChanged += new System.EventHandler(this.cbDiskList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "(Please Select PhysicalDisk First!!)";
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(438, 10);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(120, 28);
            this.btnClean.TabIndex = 2;
            this.btnClean.Text = "Clean Disk";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(564, 11);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(120, 28);
            this.btnInit.TabIndex = 2;
            this.btnInit.Text = "Init Disk";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnFormat
            // 
            this.btnFormat.Location = new System.Drawing.Point(690, 11);
            this.btnFormat.Name = "btnFormat";
            this.btnFormat.Size = new System.Drawing.Size(120, 28);
            this.btnFormat.TabIndex = 2;
            this.btnFormat.Text = "Format";
            this.btnFormat.UseVisualStyleBackColor = true;
            this.btnFormat.Click += new System.EventHandler(this.btnFormat_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 84);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(798, 335);
            this.textBox1.TabIndex = 3;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 431);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnFormat);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.cbDiskList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InitAndFormatPhysicalDisk";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDiskList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button btnFormat;
        private System.Windows.Forms.TextBox textBox1;
    }
}

