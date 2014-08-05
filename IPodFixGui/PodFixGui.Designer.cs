namespace IPodFixGui
{
    partial class PodFixGui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PodFixGui));
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.sourceDirLabel = new System.Windows.Forms.Label();
            this.sourceDirInput = new System.Windows.Forms.TextBox();
            this.destinationDirLabel = new System.Windows.Forms.Label();
            this.destinationDirInput = new System.Windows.Forms.TextBox();
            this.sourceDirButton = new System.Windows.Forms.Button();
            this.destinationDirButton = new System.Windows.Forms.Button();
            this.fixButton = new System.Windows.Forms.Button();
            this.fixProgressBar = new System.Windows.Forms.ProgressBar();
            this.cancelButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.openButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sourceDirLabel
            // 
            this.sourceDirLabel.AutoSize = true;
            this.sourceDirLabel.Location = new System.Drawing.Point(12, 15);
            this.sourceDirLabel.Name = "sourceDirLabel";
            this.sourceDirLabel.Size = new System.Drawing.Size(89, 13);
            this.sourceDirLabel.TabIndex = 0;
            this.sourceDirLabel.Text = "Source Directory:";
            // 
            // sourceDirInput
            // 
            this.sourceDirInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceDirInput.Enabled = false;
            this.sourceDirInput.Location = new System.Drawing.Point(123, 12);
            this.sourceDirInput.Name = "sourceDirInput";
            this.sourceDirInput.Size = new System.Drawing.Size(164, 20);
            this.sourceDirInput.TabIndex = 1;
            // 
            // destinationDirLabel
            // 
            this.destinationDirLabel.AutoSize = true;
            this.destinationDirLabel.Location = new System.Drawing.Point(12, 47);
            this.destinationDirLabel.Name = "destinationDirLabel";
            this.destinationDirLabel.Size = new System.Drawing.Size(108, 13);
            this.destinationDirLabel.TabIndex = 2;
            this.destinationDirLabel.Text = "Destination Directory:";
            // 
            // destinationDirInput
            // 
            this.destinationDirInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationDirInput.Enabled = false;
            this.destinationDirInput.Location = new System.Drawing.Point(123, 44);
            this.destinationDirInput.Name = "destinationDirInput";
            this.destinationDirInput.Size = new System.Drawing.Size(164, 20);
            this.destinationDirInput.TabIndex = 3;
            // 
            // sourceDirButton
            // 
            this.sourceDirButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceDirButton.Image = ((System.Drawing.Image)(resources.GetObject("sourceDirButton.Image")));
            this.sourceDirButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sourceDirButton.Location = new System.Drawing.Point(293, 10);
            this.sourceDirButton.Name = "sourceDirButton";
            this.sourceDirButton.Size = new System.Drawing.Size(60, 23);
            this.sourceDirButton.TabIndex = 4;
            this.sourceDirButton.Text = "Set...";
            this.sourceDirButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sourceDirButton.UseVisualStyleBackColor = true;
            this.sourceDirButton.Click += new System.EventHandler(this.sourceDirButton_Click);
            // 
            // destinationDirButton
            // 
            this.destinationDirButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationDirButton.Image = ((System.Drawing.Image)(resources.GetObject("destinationDirButton.Image")));
            this.destinationDirButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.destinationDirButton.Location = new System.Drawing.Point(293, 42);
            this.destinationDirButton.Name = "destinationDirButton";
            this.destinationDirButton.Size = new System.Drawing.Size(60, 23);
            this.destinationDirButton.TabIndex = 5;
            this.destinationDirButton.Text = "Set...";
            this.destinationDirButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.destinationDirButton.UseVisualStyleBackColor = true;
            this.destinationDirButton.Click += new System.EventHandler(this.destinationDirButton_Click);
            // 
            // fixButton
            // 
            this.fixButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.fixButton.Image = ((System.Drawing.Image)(resources.GetObject("fixButton.Image")));
            this.fixButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fixButton.Location = new System.Drawing.Point(12, 78);
            this.fixButton.Name = "fixButton";
            this.fixButton.Size = new System.Drawing.Size(75, 23);
            this.fixButton.TabIndex = 6;
            this.fixButton.Text = "Fix!";
            this.fixButton.UseVisualStyleBackColor = true;
            this.fixButton.Click += new System.EventHandler(this.fixButton_Click);
            // 
            // fixProgressBar
            // 
            this.fixProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fixProgressBar.Location = new System.Drawing.Point(12, 129);
            this.fixProgressBar.Name = "fixProgressBar";
            this.fixProgressBar.Size = new System.Drawing.Size(341, 23);
            this.fixProgressBar.TabIndex = 7;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cancelButton.Location = new System.Drawing.Point(93, 78);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusLabel.Location = new System.Drawing.Point(9, 111);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(344, 13);
            this.statusLabel.TabIndex = 9;
            this.statusLabel.Text = "Select source and destination directories.";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openButton
            // 
            this.openButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.openButton.Location = new System.Drawing.Point(174, 78);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(98, 23);
            this.openButton.TabIndex = 10;
            this.openButton.Text = "Open Directory";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Visible = false;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // PodFixGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 163);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.fixProgressBar);
            this.Controls.Add(this.fixButton);
            this.Controls.Add(this.destinationDirButton);
            this.Controls.Add(this.sourceDirButton);
            this.Controls.Add(this.destinationDirInput);
            this.Controls.Add(this.destinationDirLabel);
            this.Controls.Add(this.sourceDirInput);
            this.Controls.Add(this.sourceDirLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(386, 201);
            this.Name = "IPodFixGui";
            this.Text = "iPod Directory Fixer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label sourceDirLabel;
        private System.Windows.Forms.TextBox sourceDirInput;
        private System.Windows.Forms.Label destinationDirLabel;
        private System.Windows.Forms.TextBox destinationDirInput;
        private System.Windows.Forms.Button sourceDirButton;
        private System.Windows.Forms.Button destinationDirButton;
        private System.Windows.Forms.Button fixButton;
        private System.Windows.Forms.ProgressBar fixProgressBar;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button openButton;
    }
}

