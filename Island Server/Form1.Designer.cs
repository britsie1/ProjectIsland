namespace IslandServer
{
    partial class Form1
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
            this.pnlBrowser = new System.Windows.Forms.Panel();
            this.tbxLog = new System.Windows.Forms.TextBox();
            this.btnHostServer = new System.Windows.Forms.Button();
            this.btnWatchServer = new System.Windows.Forms.Button();
            this.tbxWatchURL = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBrowser.Location = new System.Drawing.Point(12, 52);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.Size = new System.Drawing.Size(760, 388);
            this.pnlBrowser.TabIndex = 0;
            // 
            // tbxLog
            // 
            this.tbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxLog.Location = new System.Drawing.Point(12, 446);
            this.tbxLog.Multiline = true;
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.Size = new System.Drawing.Size(760, 103);
            this.tbxLog.TabIndex = 1;
            // 
            // btnHostServer
            // 
            this.btnHostServer.Enabled = false;
            this.btnHostServer.Location = new System.Drawing.Point(12, 23);
            this.btnHostServer.Name = "btnHostServer";
            this.btnHostServer.Size = new System.Drawing.Size(84, 23);
            this.btnHostServer.TabIndex = 2;
            this.btnHostServer.Text = "Host Server";
            this.btnHostServer.UseVisualStyleBackColor = true;
            this.btnHostServer.Click += new System.EventHandler(this.btnHostServer_Click);
            // 
            // btnWatchServer
            // 
            this.btnWatchServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWatchServer.Location = new System.Drawing.Point(688, 23);
            this.btnWatchServer.Name = "btnWatchServer";
            this.btnWatchServer.Size = new System.Drawing.Size(84, 23);
            this.btnWatchServer.TabIndex = 3;
            this.btnWatchServer.Text = "Watch Server";
            this.btnWatchServer.UseVisualStyleBackColor = true;
            // 
            // tbxWatchURL
            // 
            this.tbxWatchURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxWatchURL.Location = new System.Drawing.Point(466, 25);
            this.tbxWatchURL.Name = "tbxWatchURL";
            this.tbxWatchURL.Size = new System.Drawing.Size(216, 20);
            this.tbxWatchURL.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tbxWatchURL);
            this.Controls.Add(this.btnWatchServer);
            this.Controls.Add(this.btnHostServer);
            this.Controls.Add(this.tbxLog);
            this.Controls.Add(this.pnlBrowser);
            this.Name = "Form1";
            this.Text = "Island Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBrowser;
        private System.Windows.Forms.TextBox tbxLog;
        private System.Windows.Forms.Button btnHostServer;
        private System.Windows.Forms.Button btnWatchServer;
        private System.Windows.Forms.TextBox tbxWatchURL;
    }
}

