namespace Injector
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.nightForm1 = new ReaLTaiizor.Forms.NightForm();
            this.btn_select_process = new ReaLTaiizor.Controls.LostButton();
            this.listview_process = new ReaLTaiizor.Controls.PoisonListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_close = new ReaLTaiizor.Controls.LostButton();
            this.nightForm1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nightForm1
            // 
            this.nightForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.nightForm1.Controls.Add(this.btn_select_process);
            this.nightForm1.Controls.Add(this.listview_process);
            this.nightForm1.Controls.Add(this.btn_close);
            this.nightForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nightForm1.DrawIcon = false;
            this.nightForm1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nightForm1.HeadColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.nightForm1.Location = new System.Drawing.Point(0, 0);
            this.nightForm1.MinimumSize = new System.Drawing.Size(100, 42);
            this.nightForm1.Name = "nightForm1";
            this.nightForm1.Padding = new System.Windows.Forms.Padding(0, 31, 0, 0);
            this.nightForm1.Size = new System.Drawing.Size(537, 424);
            this.nightForm1.TabIndex = 0;
            this.nightForm1.Text = "Process";
            this.nightForm1.TextAlignment = ReaLTaiizor.Forms.NightForm.Alignment.Left;
            this.nightForm1.TitleBarTextColor = System.Drawing.Color.Gainsboro;
            // 
            // btn_select_process
            // 
            this.btn_select_process.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.btn_select_process.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_select_process.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_select_process.ForeColor = System.Drawing.Color.White;
            this.btn_select_process.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btn_select_process.Image = null;
            this.btn_select_process.Location = new System.Drawing.Point(10, 393);
            this.btn_select_process.Name = "btn_select_process";
            this.btn_select_process.Size = new System.Drawing.Size(517, 23);
            this.btn_select_process.TabIndex = 7;
            this.btn_select_process.Text = "Select Process";
            this.btn_select_process.Click += new System.EventHandler(this.btn_select_process_Click);
            // 
            // listview_process
            // 
            this.listview_process.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.listview_process.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listview_process.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.listview_process.FullRowSelect = true;
            this.listview_process.Location = new System.Drawing.Point(10, 36);
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
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.btn_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_close.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_close.ForeColor = System.Drawing.Color.White;
            this.btn_close.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(61)))));
            this.btn_close.Image = global::Injector.Properties.Resources.macos_close_30px;
            this.btn_close.Location = new System.Drawing.Point(493, -3);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(40, 34);
            this.btn_close.TabIndex = 4;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 424);
            this.Controls.Add(this.nightForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1032);
            this.MinimumSize = new System.Drawing.Size(190, 40);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "formTheme1";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.nightForm1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Forms.NightForm nightForm1;
        private ReaLTaiizor.Controls.LostButton btn_close;
        private ReaLTaiizor.Controls.PoisonListView listview_process;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ReaLTaiizor.Controls.LostButton btn_select_process;
    }
}