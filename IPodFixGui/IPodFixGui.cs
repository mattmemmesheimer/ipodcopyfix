﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IPodFixGui.ipodfix;

namespace IPodFixGui
{
    public partial class IPodFixGui : Form
    {
        private iPodFix m_Fixer;

        public IPodFixGui()
        {
            InitializeComponent();
            m_Fixer = new iPodFix();
            m_Fixer.Test();
        }

        /// <summary>
        /// Click handler for the set source directory button.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void sourceDirButton_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog.ShowDialog();
            m_Fixer.SourceDir = folderBrowserDialog.SelectedPath;
            if (m_Fixer.ValidSourceDir())
            {
                sourceDirInput.Text = folderBrowserDialog.SelectedPath;
            }
            else
            {
                MessageBox.Show("Invalid source directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            bool Empty = (Directory.GetFiles(folderBrowserDialog.SelectedPath).Length == 0);
            Empty = Empty && (Directory.GetDirectories(folderBrowserDialog.SelectedPath).Length == 0);
            bool SelectDir = false;
            if (!Empty)
            {
                DialogResult res = MessageBox.Show("Destination directory is not empty.  Continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.OK)
                {
                    SelectDir = true;
                }
            }
            if (Empty || SelectDir)
            {
                destinationDirInput.Text = folderBrowserDialog.SelectedPath;
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
            //m_Fixer = new iPodFix(sourceDirInput.Text, destinationDirInput.Text);
            //m_Fixer = new iPodFix("E:\\my-ipod-backup", "R:\\ipod");
            if (!ValidInput() || !m_Fixer.ValidSourceDir())
            {
                //MessageBox.Show("Invalid source and/or destination directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }
            m_Fixer.OnUpdateStatus += new iPodFix.StatusUpdateHandler(m_Fixer_OnUpdateStatus);
            SetButtonsEnabled(false);
            statusLabel.Text = "Performing fix...";
            m_Fixer.StartFixAsync();
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
                    val = 100;
                    statusLabel.Text = "Fix completed!";
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
    }
}
