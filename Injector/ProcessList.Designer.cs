namespace Injector
{
    partial class ProcessList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessList));
            this.nightForm1 = new ReaLTaiizor.Forms.NightForm();
            this.group_dll = new ReaLTaiizor.Controls.ThunderGroupBox();
            this.crownLabel1 = new ReaLTaiizor.Controls.CrownLabel();
            this.listview_process = new ReaLTaiizor.Controls.PoisonListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txb_process = new ReaLTaiizor.Controls.CrownTextBox();
            this.btn_select_process = new ReaLTaiizor.Controls.LostButton();
            this.btn_close = new ReaLTaiizor.Controls.LostButton();
            this.nightForm1.SuspendLayout();
            this.group_dll.SuspendLayout();
            this.SuspendLayout();
            // 
            // nightForm1
            // 
            this.nightForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.nightForm1.Controls.Add(this.group_dll);
            this.nightForm1.Controls.Add(this.btn_select_process);
            this.nightForm1.Controls.Add(this.btn_close);
            this.nightForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nightForm1.DrawIcon = false;
            this.nightForm1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nightForm1.HeadColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.nightForm1.Location = new System.Drawing.Point(0, 0);
            this.nightForm1.MinimumSize = new System.Drawing.Size(100, 42);
            this.nightForm1.Name = "nightForm1";
            this.nightForm1.Padding = new System.Windows.Forms.Padding(0, 31, 0, 0);
            this.nightForm1.Size = new System.Drawing.Size(548, 490);
            this.nightForm1.TabIndex = 0;
            this.nightForm1.Text = "Process";
            this.nightForm1.TextAlignment = ReaLTaiizor.Forms.NightForm.Alignment.Left;
            this.nightForm1.TitleBarTextColor = System.Drawing.Color.Gainsboro;
            // 
            // group_dll
            // 
            this.group_dll.BackColor = System.Drawing.Color.Transparent;
            this.group_dll.BodyColorA = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.group_dll.BodyColorB = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.group_dll.BodyColorC = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.group_dll.BodyColorD = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.group_dll.Controls.Add(this.crownLabel1);
            this.group_dll.Controls.Add(this.listview_process);
            this.group_dll.Controls.Add(this.txb_process);
            this.group_dll.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.group_dll.Location = new System.Drawing.Point(6, 37);
            this.group_dll.Name = "group_dll";
            this.group_dll.Size = new System.Drawing.Size(535, 415);
            this.group_dll.TabIndex = 10;
            this.group_dll.Text = "Process";
            // 
            // crownLabel1
            // 
            this.crownLabel1.AutoSize = true;
            this.crownLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crownLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.crownLabel1.Location = new System.Drawing.Point(7, 385);
            this.crownLabel1.Name = "crownLabel1";
            this.crownLabel1.Size = new System.Drawing.Size(119, 17);
            this.crownLabel1.TabIndex = 9;
            this.crownLabel1.Text = "Input pId or Name:";
            // 
            // listview_process
            // 
            this.listview_process.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.listview_process.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listview_process.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.listview_process.FullRowSelect = true;
            this.listview_process.Location = new System.Drawing.Point(9, 28);
            this.listview_process.Name = "listview_process";
            this.listview_process.OwnerDraw = true;
            this.listview_process.Size = new System.Drawing.Size(517, 351);
            this.listview_process.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Silver;
            this.listview_process.TabIndex = 5;
            this.listview_process.UseCompatibleStateImageBehavior = false;
            this.listview_process.UseSelectable = true;
            this.listview_process.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "pId";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 413;
            // 
            // txb_process
            // 
            this.txb_process.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.txb_process.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txb_process.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txb_process.Location = new System.Drawing.Point(126, 383);
            this.txb_process.Name = "txb_process";
            this.txb_process.Size = new System.Drawing.Size(400, 23);
            this.txb_process.TabIndex = 8;
            this.txb_process.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txb_process.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txb_process_KeyUp);
            // 
            // btn_select_process
            // 
            this.btn_select_process.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.btn_select_process.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_select_process.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_select_process.ForeColor = System.Drawing.Color.White;
            this.btn_select_process.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btn_select_process.Image = null;
            this.btn_select_process.Location = new System.Drawing.Point(167, 458);
            this.btn_select_process.Name = "btn_select_process";
            this.btn_select_process.Size = new System.Drawing.Size(214, 23);
            this.btn_select_process.TabIndex = 7;
            this.btn_select_process.Text = "Select Process";
            this.btn_select_process.Click += new System.EventHandler(this.btn_select_process_Click);
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.btn_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_close.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_close.ForeColor = System.Drawing.Color.White;
            this.btn_close.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.btn_close.Image = global::Injector.Properties.Resources.macos_close_30px;
            this.btn_close.Location = new System.Drawing.Point(501, -3);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(40, 34);
            this.btn_close.TabIndex = 4;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // ProcessList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 490);
            this.Controls.Add(this.nightForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1032);
            this.MinimumSize = new System.Drawing.Size(190, 40);
            this.Name = "ProcessList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "formTheme1";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.nightForm1.ResumeLayout(false);
            this.group_dll.ResumeLayout(false);
            this.group_dll.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Forms.NightForm nightForm1;
        private ReaLTaiizor.Controls.LostButton btn_close;
        private ReaLTaiizor.Controls.PoisonListView listview_process;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ReaLTaiizor.Controls.LostButton btn_select_process;
        private ReaLTaiizor.Controls.CrownTextBox txb_process;
        private ReaLTaiizor.Controls.ThunderGroupBox group_dll;
        private ReaLTaiizor.Controls.CrownLabel crownLabel1;
    }
}