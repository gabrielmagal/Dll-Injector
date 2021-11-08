using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Injector
{
    public partial class Form2 : Form
    {
        private Form1 parentForm1;
        public Form2(Form1 parentForm1)
        {
            this.parentForm1 = parentForm1;
            InitializeComponent();
            Process[] listProcess = Process.GetProcesses();
            foreach (Process process in listProcess)
            {
                ListViewItem item = new ListViewItem(process.Id.ToString());
                item.SubItems.Add(process.ProcessName);
                listview_process.Items.Add(item);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_select_process_Click(object sender, EventArgs e)
        {
            if (listview_process.SelectedIndices.Count > 0)
            {
                parentForm1.strProcess = listview_process.SelectedItems[0].SubItems[1].Text;
                //parentForm1.pId = int.Parse(listview_process.SelectedItems[0].Text);
            }
            Close();
        }
    }
}
