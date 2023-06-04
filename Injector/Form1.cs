﻿using System;
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
        public string strProcess = "";

        public Form1()
        {
            InitializeComponent();

            // If the file exists, we load the saved profiles, if not, we create the file
            if (!File.Exists(Application.StartupPath + "\\Config.ini"))
                File.Create(Application.StartupPath + "\\Config.ini").Close();
            else
                for (int i = 0; i < IniFile.GetSectionNames().Length; i++)
                    cbb_profile.Items.Add(IniFile.GetSectionNames()[i]);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_dll_add_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedialog_path_dll = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse dll Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "dll",
                Filter = "dll (*.dll)|*.dll",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true
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
            // Remove selected dll
            if (listview_dlls.SelectedIndices.Count > 0)
                if (ReaLTaiizor.Controls.CrownMessageBox.ShowInformation("Do you really want to remove the selected dll?", "Info", ReaLTaiizor.Enum.Crown.DialogButton.YesNo) == DialogResult.Yes)
                    listview_dlls.Items.Remove(listview_dlls.SelectedItems[0]);
        }

        private void btn_dll_removeAll_Click(object sender, EventArgs e)
        {
            // Remove selected dlls
            if (ReaLTaiizor.Controls.CrownMessageBox.ShowInformation("Do you really want to remove all dlls?", "Info", ReaLTaiizor.Enum.Crown.DialogButton.YesNo) == DialogResult.Yes)
                listview_dlls.Items.Clear();
        }

        private void btn_process_listAllProcess_Click(object sender, EventArgs e)
        {
            // We hide the first screen and show the second one for selecting the listed processes
            Hide();
            Form2 f2 = new Form2(this);
            f2.ShowDialog();
            Show();

            // We get the return of the selected process from the other screen
            if (!string.IsNullOrEmpty(strProcess)) txb_process.Text = strProcess;
        }

        public void msg_inject_sucess(string dll)
        {
            if (statusbar.Text.Equals("successfully injected"))
            {
                statusbar.RectColor = Color.FromArgb(35, 168, 109);
                statusbar.Text = $"{dll} {statusbar.Text} in {txb_process.Text}";
            }
            else statusbar.RectColor = Color.IndianRed;
        }

        public void inject()
        {
            for (int i = 0; i < listview_dlls.Items.Count; i++)
            {
                if (listview_dlls.Items[i].Checked)
                {
                    if (rdb_method_standardA.Checked)
                    {
                        statusbar.Text = Injection.standardA(listview_dlls.Items[0].SubItems[1].Text, Process.GetProcessesByName(txb_process.Text)[0].Id, cbb_peHeader.SelectedIndex);
                        msg_inject_sucess(listview_dlls.Items[0].Text);
                    }

                    if (rdb_method_standardW.Checked)
                    {
                        statusbar.Text = Injection.standardW(listview_dlls.Items[0].SubItems[1].Text, Process.GetProcessesByName(txb_process.Text)[0].Id, cbb_peHeader.SelectedIndex);
                        msg_inject_sucess(listview_dlls.Items[0].Text);
                    }

                    if (rdb_method_manuallMap.Checked)
                    {
                        statusbar.Text = Injection.manuallMap(listview_dlls.Items[0].SubItems[1].Text, Process.GetProcessesByName(txb_process.Text)[0].Id, cbb_peHeader.SelectedIndex);
                        msg_inject_sucess(listview_dlls.Items[0].Text);
                    }
                }
            }
            if (chk_injection_close_on_inject.Checked) Close();
        }

        private void btn_inject_Click(object sender, EventArgs e)
        {
            inject();
        }

        private void timer_automatic_injection_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txb_process.Text) && Process.GetProcessesByName(txb_process.Text).Length > 0)
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

        private void btn_save_config_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txb_process.Text) && listview_dlls.Items.Count > 0)
            {
                // Save selected process
                IniFile.Write(txb_process.Text, "Process", txb_process.Text);
                for (int i = 0; i < listview_dlls.Items.Count; i++)
                {
                    // Save path of listed dlls
                    IniFile.Write(txb_process.Text, "Path" + i, listview_dlls.Items[i].SubItems[1].Text);

                    // Save status of selected dlls
                    if (listview_dlls.Items[i].Checked) IniFile.Write(txb_process.Text, "Dll_checked" + i, "true");
                    else IniFile.Write(txb_process.Text, "Dll_checked" + i, "false");
                }

                if (chk_injection_close_on_inject.Checked) IniFile.Write(txb_process.Text, "CloseOnInject", "true");
                else IniFile.Write(txb_process.Text, "CloseOnInject", "false");

                if (cbb_peHeader.SelectedIndex != -1) IniFile.Write(txb_process.Text, "PeHeader", cbb_peHeader.Text);
                ReaLTaiizor.Controls.CrownMessageBox.ShowInformation("Profile Added successfully!", "Info", ReaLTaiizor.Enum.Crown.DialogButton.Ok);
            }
        }

        private void btn_remove_config_Click(object sender, EventArgs e)
        {
            // Checks if any profile has been selected
            if (cbb_profile.SelectedIndex == -1)
            {
                ReaLTaiizor.Controls.CrownMessageBox.ShowError("No profiles have been selected to be removed!", "Info", ReaLTaiizor.Enum.Crown.DialogButton.Ok);
                return;
            }

            // Precaution if you really want to remove the profile
            if (ReaLTaiizor.Controls.CrownMessageBox.ShowInformation("Do you really want to remove the saved configuration?", "Info", ReaLTaiizor.Enum.Crown.DialogButton.YesNo) == DialogResult.Yes)
            {
                IniFile.Clear_Section(cbb_profile.SelectedItem.ToString());
            }
        }

        private void cbb_profile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbb_profile.SelectedIndex != -1)
            {
                listview_dlls.Items.Clear();
                txb_process.Text = IniFile.Read(cbb_profile.Text, "Process");

                if (!String.IsNullOrEmpty(IniFile.Read("All", "peHeader"))) cbb_peHeader.Text = IniFile.Read("All", "peHeader");

                if (IniFile.Read("All", "CloseOnInject") == "true") chk_injection_close_on_inject.Checked = true;
                else chk_injection_close_on_inject.Checked = false;

                for (int i = 0; i < 20; i++)
                {
                    if (String.IsNullOrEmpty(IniFile.Read(cbb_profile.Text, "Path" + i))) break;
                    string str = IniFile.Read(cbb_profile.Text, "Path" + i);
                    ListViewItem listViewItem = new ListViewItem(Path.GetFileName(str));
                    listViewItem.SubItems.Add(str);
                    listview_dlls.Items.Add(listViewItem);
                    if (IniFile.Read(cbb_profile.Text, "dll_checked" + i) == "true") listview_dlls.Items[i].Checked = true;
                }
            }
        }
    }
}
