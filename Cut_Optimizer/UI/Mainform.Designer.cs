namespace Cut_Optimizer.UI
{
    partial class Mainform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.fromdate = new System.Windows.Forms.DateTimePicker();
            this.todate = new System.Windows.Forms.DateTimePicker();
            this.exportbrws = new System.Windows.Forms.FolderBrowserDialog();
            this.export = new System.Windows.Forms.Button();
            this.exportpath = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cls = new System.Windows.Forms.PictureBox();
            this.lnkd = new System.Windows.Forms.PictureBox();
            this.edecs = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.assign = new System.Windows.Forms.Button();
            this.excelfile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.cls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lnkd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edecs)).BeginInit();
            this.SuspendLayout();
            // 
            // fromdate
            // 
            this.fromdate.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.fromdate.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.fromdate.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.fromdate.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.fromdate.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.fromdate.Location = new System.Drawing.Point(12, 35);
            this.fromdate.Name = "fromdate";
            this.fromdate.Size = new System.Drawing.Size(200, 21);
            this.fromdate.TabIndex = 1;
            this.fromdate.ValueChanged += new System.EventHandler(this.fromdate_ValueChanged);
            // 
            // todate
            // 
            this.todate.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.todate.CalendarMonthBackground = System.Drawing.Color.White;
            this.todate.CalendarTitleBackColor = System.Drawing.Color.White;
            this.todate.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.todate.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.todate.Location = new System.Drawing.Point(218, 35);
            this.todate.Name = "todate";
            this.todate.Size = new System.Drawing.Size(200, 21);
            this.todate.TabIndex = 2;
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(344, 95);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(75, 23);
            this.export.TabIndex = 3;
            this.export.Text = "Export";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // exportpath
            // 
            this.exportpath.BackColor = System.Drawing.Color.White;
            this.exportpath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.exportpath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.exportpath.Location = new System.Drawing.Point(13, 63);
            this.exportpath.Multiline = true;
            this.exportpath.Name = "exportpath";
            this.exportpath.Size = new System.Drawing.Size(404, 23);
            this.exportpath.TabIndex = 4;
            this.exportpath.TextChanged += new System.EventHandler(this.exportpath_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.panel1.Location = new System.Drawing.Point(12, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(406, 25);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "From Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "To Date";
            // 
            // title
            // 
            this.title.BackColor = System.Drawing.Color.White;
            this.title.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.title.Location = new System.Drawing.Point(13, 94);
            this.title.Multiline = true;
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(323, 23);
            this.title.TabIndex = 8;
            this.title.TextChanged += new System.EventHandler(this.title_TextChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.panel2.Location = new System.Drawing.Point(12, 93);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(325, 25);
            this.panel2.TabIndex = 6;
            // 
            // cls
            // 
            this.cls.Image = global::Cut_Optimizer.Properties.Resources.Close;
            this.cls.Location = new System.Drawing.Point(403, 180);
            this.cls.Name = "cls";
            this.cls.Size = new System.Drawing.Size(15, 15);
            this.cls.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cls.TabIndex = 11;
            this.cls.TabStop = false;
            this.cls.Click += new System.EventHandler(this.cls_Click);
            // 
            // lnkd
            // 
            this.lnkd.Image = global::Cut_Optimizer.Properties.Resources.Linkedin;
            this.lnkd.Location = new System.Drawing.Point(383, 180);
            this.lnkd.Name = "lnkd";
            this.lnkd.Size = new System.Drawing.Size(15, 15);
            this.lnkd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.lnkd.TabIndex = 10;
            this.lnkd.TabStop = false;
            this.lnkd.Click += new System.EventHandler(this.lnkd_Click);
            // 
            // edecs
            // 
            this.edecs.Image = global::Cut_Optimizer.Properties.Resources.EDECS;
            this.edecs.Location = new System.Drawing.Point(12, 159);
            this.edecs.Name = "edecs";
            this.edecs.Size = new System.Drawing.Size(125, 50);
            this.edecs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.edecs.TabIndex = 9;
            this.edecs.TabStop = false;
            this.edecs.Click += new System.EventHandler(this.edecs_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.panel3.Location = new System.Drawing.Point(66, 23);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(100, 2);
            this.panel3.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.panel4.Location = new System.Drawing.Point(255, 23);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(100, 2);
            this.panel4.TabIndex = 7;
            // 
            // assign
            // 
            this.assign.Location = new System.Drawing.Point(12, 124);
            this.assign.Name = "assign";
            this.assign.Size = new System.Drawing.Size(407, 23);
            this.assign.TabIndex = 14;
            this.assign.Text = "Assign";
            this.assign.UseVisualStyleBackColor = true;
            this.assign.Click += new System.EventHandler(this.assign_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(431, 206);
            this.Controls.Add(this.assign);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.cls);
            this.Controls.Add(this.lnkd);
            this.Controls.Add(this.edecs);
            this.Controls.Add(this.title);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exportpath);
            this.Controls.Add(this.export);
            this.Controls.Add(this.todate);
            this.Controls.Add(this.fromdate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(96)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Mainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cut Optimizer";
            ((System.ComponentModel.ISupportInitialize)(this.cls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lnkd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edecs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker fromdate;
        private System.Windows.Forms.DateTimePicker todate;
        private System.Windows.Forms.FolderBrowserDialog exportbrws;
        private System.Windows.Forms.Button export;
        private System.Windows.Forms.TextBox exportpath;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox title;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox edecs;
        private System.Windows.Forms.PictureBox lnkd;
        private System.Windows.Forms.PictureBox cls;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button assign;
        private System.Windows.Forms.OpenFileDialog excelfile;
    }
}