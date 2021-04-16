using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_win_fs_watcher
{
    public partial class FrmMain : Form
    {
        string WatchPath = string.Empty;
        private delegate void UpdateUIWatcherDelegate(FileSystemEventArgs e);
        private void UpdateUIWatcher(FileSystemEventArgs e) =>
            _ = Invoke(new UpdateUIWatcherDelegate(UpdateUIWithDiffThread), e);

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (FolderBro.ShowDialog() == DialogResult.OK)
            {
                WatchPath = FolderBro.SelectedPath;
                FSWatcher.Path = WatchPath;
                btnStart.Enabled = true;
            }
        }

        private void UpdateUIWithDiffThread(FileSystemEventArgs e)
        {
            richUpdate.ForeColor = e.ChangeType == WatcherChangeTypes.Created ? Color.Blue : Color.Magenta;
            richUpdate.AppendText(string.Format("File {0}, {1}{2}{3}", e.FullPath, e.ChangeType, Environment.NewLine, Environment.NewLine));
        }

        private void FSWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //C - 1
            if (c1.CheckState == CheckState.Checked)
            {
                UpdateUIWatcher(e);
            }
        }

        private void FSWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            //R - 2
            if (c2.CheckState == CheckState.Checked)
            {
                UpdateUIWatcher(e);
            }
        }

        private void FSWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //U -3
            if (c3.CheckState == CheckState.Checked)
            {
                UpdateUIWatcher(e);
            }
        }

        private void FSWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            //D - 4
            if (c4.CheckState == CheckState.Checked)
            {
                UpdateUIWatcher(e);
            }
        }

        private void FSWatcher_Error(object sender, ErrorEventArgs e)
        {
            //to-do
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Format("Watching... {0}", WatchPath);
            FSWatcher.Path = WatchPath;
            FSWatcher.EnableRaisingEvents = true;
            btnSelect.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // FSWatcher.Path = string.Empty;
            lblStatus.Text = string.Format("Waiting....");
            FSWatcher.EnableRaisingEvents = false;
            btnSelect.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
        }

        private void ChkSubPath_CheckedChanged(object sender, EventArgs e)
        {
            FSWatcher.IncludeSubdirectories = ChkSubPath.Checked;
        }
    }
}
