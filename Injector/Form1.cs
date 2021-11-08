using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Injector
{
    public partial class Form1 : Form
    {
        Ini IniFile = new Ini(Application.StartupPath + "\\Config.ini");
        public string strProcess = ""; //public int pId = 0;

        public Form1()
        {
            InitializeComponent();
            if (!File.Exists(Application.StartupPath + "\\Config.ini")) File.Create(Application.StartupPath + "\\Config.ini").Close();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_dll_add_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedialog_path_dll = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                Filter = "dll (*.dll)|*.dll",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (filedialog_path_dll.ShowDialog() == DialogResult.OK)
            {
                ListViewItem listViewItem = new ListViewItem(filedialog_path_dll.SafeFileName);
                listViewItem.SubItems.Add(filedialog_path_dll.FileName);
                listview_dlls.Items.Add(listViewItem);
            }
        }

        private void btn_dll_remove_Click(object sender, EventArgs e)
        {
            if(listview_dlls.SelectedIndices.Count > 0)
                if (ReaLTaiizor.Controls.CrownMessageBox.ShowInformation("Deseja realmente remover a dll selecionada?", "Info", ReaLTaiizor.Enum.Crown.DialogButton.YesNo) == DialogResult.Yes)
                    listview_dlls.Items.Remove(listview_dlls.SelectedItems[0]);
        }

        private void btn_dll_removeAll_Click(object sender, EventArgs e)
        {
            if(ReaLTaiizor.Controls.CrownMessageBox.ShowInformation("Deseja realmente remover todas dlls?", "Info", ReaLTaiizor.Enum.Crown.DialogButton.YesNo) == DialogResult.Yes)
                listview_dlls.Items.Clear();
        }

        private void btn_process_listAllProcess_Click(object sender, EventArgs e)
        {
            Hide();
            Form2 f2 = new Form2(this);
            f2.ShowDialog();
            Show();
            if(!string.IsNullOrEmpty(strProcess)) txb_process.Text = strProcess;
        }

        public void msg_inject_sucess(string msg)
        {
            if (statusbar.Text.Equals("successfully injected"))
            {
                statusbar.RectColor = Color.FromArgb(35, 168, 109);
                statusbar.Text = $"{listview_dlls.SelectedItems[0].SubItems[0].Text} {statusbar.Text} in {txb_process.Text}";
            }
            else statusbar.RectColor = Color.IndianRed;
        }

        public void inject()
        {
            if (rdb_method_standardA.Checked && listview_dlls.SelectedIndices.Count > 0)
            {
                statusbar.Text = MethodInjection.standardA(listview_dlls.SelectedItems[0].SubItems[1].Text, Process.GetProcessesByName(txb_process.Text)[0].Id, cbb_peHeader.SelectedIndex);
                msg_inject_sucess(statusbar.Text);
            }

            if (rdb_method_standardW.Checked && listview_dlls.SelectedIndices.Count > 0)
            {
                statusbar.Text = MethodInjection.standardW(listview_dlls.SelectedItems[0].SubItems[1].Text, Process.GetProcessesByName(txb_process.Text)[0].Id, cbb_peHeader.SelectedIndex);
                msg_inject_sucess(statusbar.Text);
            }

            if (chk_injection_close_on_inject.Checked)  Close();
        }

        private void btn_inject_Click(object sender, EventArgs e)
        {
            inject();
        }

        private void timer_automatic_injection_Tick(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txb_process.Text) && Process.GetProcessesByName(txb_process.Text).Length > 0)
            {
                inject();
                chk_injection_automatic.Checked = false;
                timer_automatic_injection.Stop();
                timer_automatic_injection.Enabled = false;
            }
        }

        private void chk_injection_automatic_CheckedChanged(object sender)
        {
            if (chk_injection_automatic.Checked)
            {
                timer_automatic_injection.Enabled = true;
                timer_automatic_injection.Start();
            }
            else
            {
                timer_automatic_injection.Stop();
                timer_automatic_injection.Enabled = false;
            }
        }
    }
}
