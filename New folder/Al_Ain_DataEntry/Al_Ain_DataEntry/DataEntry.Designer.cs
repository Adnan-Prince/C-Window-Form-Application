
namespace Al_Ain_DataEntry
{
    partial class DataEntry
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
            this.components = new System.ComponentModel.Container();
            this.textboxno = new System.Windows.Forms.TextBox();
            this.textfilebarcode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btncomplete = new System.Windows.Forms.Button();
            this.btnRenter = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textcount = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.combobranchname = new System.Windows.Forms.ComboBox();
            this.textdate = new System.Windows.Forms.TextBox();
            this.textsubdate = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBagnumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textboxno
            // 
            this.textboxno.Location = new System.Drawing.Point(185, 56);
            this.textboxno.Name = "textboxno";
            this.textboxno.Size = new System.Drawing.Size(154, 22);
            this.textboxno.TabIndex = 0;
            this.textboxno.TextChanged += new System.EventHandler(this.textboxno_TextChanged);
            // 
            // textfilebarcode
            // 
            this.textfilebarcode.Location = new System.Drawing.Point(185, 103);
            this.textfilebarcode.Name = "textfilebarcode";
            this.textfilebarcode.Size = new System.Drawing.Size(154, 22);
            this.textfilebarcode.TabIndex = 1;
            this.textfilebarcode.TextChanged += new System.EventHandler(this.textfilebarcode_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = " Box Number";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = " File barcode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(536, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Branch Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(536, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(536, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Submission Date";
            // 
            // btncomplete
            // 
            this.btncomplete.Location = new System.Drawing.Point(82, 153);
            this.btncomplete.Name = "btncomplete";
            this.btncomplete.Size = new System.Drawing.Size(109, 36);
            this.btncomplete.TabIndex = 8;
            this.btncomplete.Text = "Complete";
            this.btncomplete.UseVisualStyleBackColor = true;
            this.btncomplete.Click += new System.EventHandler(this.btncomplete_Click);
            // 
            // btnRenter
            // 
            this.btnRenter.Location = new System.Drawing.Point(225, 153);
            this.btnRenter.Name = "btnRenter";
            this.btnRenter.Size = new System.Drawing.Size(114, 36);
            this.btnRenter.TabIndex = 13;
            this.btnRenter.Text = "Clear";
            this.btnRenter.UseVisualStyleBackColor = true;
            this.btnRenter.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(675, 241);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(103, 36);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "Save";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(455, 275);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 16);
            this.label6.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(437, 262);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 16);
            this.label7.TabIndex = 16;
            this.label7.Text = "Count";
            // 
            // textcount
            // 
            this.textcount.Location = new System.Drawing.Point(500, 258);
            this.textcount.Name = "textcount";
            this.textcount.Size = new System.Drawing.Size(72, 22);
            this.textcount.TabIndex = 17;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // combobranchname
            // 
            this.combobranchname.FormattingEnabled = true;
            this.combobranchname.Location = new System.Drawing.Point(675, 93);
            this.combobranchname.Name = "combobranchname";
            this.combobranchname.Size = new System.Drawing.Size(231, 24);
            this.combobranchname.TabIndex = 3;
            this.combobranchname.SelectedIndexChanged += new System.EventHandler(this.combobranchname_SelectedIndexChanged);
            // 
            // textdate
            // 
            this.textdate.Location = new System.Drawing.Point(675, 138);
            this.textdate.Name = "textdate";
            this.textdate.Size = new System.Drawing.Size(231, 22);
            this.textdate.TabIndex = 4;
            this.textdate.TextChanged += new System.EventHandler(this.textdate_TextChanged);
            // 
            // textsubdate
            // 
            this.textsubdate.Location = new System.Drawing.Point(675, 189);
            this.textsubdate.Name = "textsubdate";
            this.textsubdate.Size = new System.Drawing.Size(231, 22);
            this.textsubdate.TabIndex = 5;
            this.textsubdate.TextChanged += new System.EventHandler(this.textsubdate_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(917, 241);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 36);
            this.button2.TabIndex = 9;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // textBagnumber
            // 
            this.textBagnumber.Location = new System.Drawing.Point(675, 56);
            this.textBagnumber.Name = "textBagnumber";
            this.textBagnumber.Size = new System.Drawing.Size(231, 22);
            this.textBagnumber.TabIndex = 2;
            this.textBagnumber.TextChanged += new System.EventHandler(this.textBagnumber_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(536, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 16);
            this.label8.TabIndex = 24;
            this.label8.Text = "Bag Number";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(800, 241);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 36);
            this.button1.TabIndex = 7;
            this.button1.Text = "Add more";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1094, 28);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.reportsToolStripMenuItem.Text = "Reports";
            this.reportsToolStripMenuItem.Click += new System.EventHandler(this.reportsToolStripMenuItem_Click);
            // 
            // DataEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 507);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBagnumber);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textsubdate);
            this.Controls.Add(this.textdate);
            this.Controls.Add(this.combobranchname);
            this.Controls.Add(this.textcount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnRenter);
            this.Controls.Add(this.btncomplete);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textfilebarcode);
            this.Controls.Add(this.textboxno);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DataEntry";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DataEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textboxno;
        private System.Windows.Forms.TextBox textfilebarcode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btncomplete;
        private System.Windows.Forms.Button btnRenter;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textcount;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ComboBox combobranchname;
        private System.Windows.Forms.TextBox textsubdate;
        private System.Windows.Forms.TextBox textdate;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBagnumber;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
    }
}

