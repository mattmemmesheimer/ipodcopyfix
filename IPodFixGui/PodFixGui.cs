using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IPodFixGui.ipodfix;
using IPodFixGui.Properties;

namespace IPodFixGui
{
    public partial class PodFixGui : Form
    {
        private PodFix _podFix;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PodFixGui()
        {
            InitializeComponent();
            _podFix = new PodFix();
        }

        /// <summary>
        /// Click handler for the set source directory button.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void sourceDirButton_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog.ShowDialog();
            _podFix.SourceDir = folderBrowserDialog.SelectedPath;
            if (_podFix.ValidSourceDir())
            {
                sourceDirInput.Text = folderBrowserDialog.SelectedPath;
            }
            else
            {
                MessageBox.Show(Resources.ErrorInvalidSourceDirectory, Resources.TitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                folderBrowserDialog.SelectedPath = "";
            }
            
        }

        /// <summary>
        /// Click handler for the set destination directory button.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void destinationDirButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            bool empty = (Directory.GetFiles(folderBrowserDialog.SelectedPath).Length == 0);
            empty = empty && (Directory.GetDirectories(folderBrowserDialog.SelectedPath).Length == 0);
            bool selectDir = false;
            if (!empty)
            {
                DialogResult res = MessageBox.Show(Resources.WarningDestinationDirectoryNotEmpty, Resources.TitleWarning, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.OK)
                {
                    selectDir = true;
                }
            }
            if (empty || selectDir)
            {
                destinationDirInput.Text = folderBrowserDialog.SelectedPath;
                _podFix.DestinationDir = destinationDirInput.Text;
            }
            else 
            {
                folderBrowserDialog.SelectedPath = "";
            }
        }

        /// <summary>
        /// Click handler for the start fix button.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void fixButton_Click(object sender, EventArgs e)
        {
            //_podFix = new PodFix(sourceDirInput.Text, destinationDirInput.Text);
            //_podFix = new PodFix("E:\\my-ipod-backup", "R:\\ipod");
            if (!ValidInput() || !_podFix.ValidSourceDir())
            {
                //MessageBox.Show("Invalid source and/or destination directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }
            _podFix.OnUpdateStatus += new PodFix.StatusUpdateHandler(m_Fixer_OnUpdateStatus);
            SetButtonsEnabled(false);
            statusLabel.Text = Resources.PerformingFix;
            _podFix.StartFixAsync();
            log.Info("Performing fix.");
            
        }

        /// <summary>
        /// Callback for iPod fixer status update.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        void m_Fixer_OnUpdateStatus(object sender, ProgressEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate{
                int val = 0;
                if (e.Completed)
                {
                    SetButtonsEnabled(true);
                    SetButtonsVisible(true);
                    val = 100;
                    statusLabel.Text = Resources.FixCompleted;
                }
                else
                {
                    val = (int)e.PercentCompleted;
                }
                fixProgressBar.Value = val;
            }));
        }

        /// <summary>
        /// Determines if the
        /// </summary>
        /// <returns></returns>
        private bool ValidInput()
        {
            bool valid = false;
            valid = !string.IsNullOrEmpty(sourceDirInput.Text);
            valid = valid && !string.IsNullOrEmpty(destinationDirInput.Text);
            return valid;
        }

        /// <summary>
        /// Sets the buttons' enabled states.
        /// </summary>
        /// <param name="enabled">true if the buttons should be enabled, false otherwise</param>
        private void SetButtonsEnabled(bool enabled)
        {
            sourceDirButton.Enabled = enabled;
            destinationDirButton.Enabled = enabled;
            fixButton.Enabled = enabled;
        }

        private void SetButtonsVisible(bool visible)
        {
            openButton.Visible = visible;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            string destinationPath = destinationDirInput.Text;
            System.Diagnostics.Process.Start("explorer.exe", destinationPath);
        }
    }
}
