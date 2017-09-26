namespace Ambilight_DFMirage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gammaValueLabel = new System.Windows.Forms.Label();
            this.cpuLabel = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.hideButton = new System.Windows.Forms.Button();
            this.aboutLabel = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.scanDepthLabel = new System.Windows.Forms.Label();
            this.gammaTrackBar = new System.Windows.Forms.TrackBar();
            this.gammaLabel = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.multiThreadingCheckBox = new System.Windows.Forms.CheckBox();
            this.skipLabel = new System.Windows.Forms.Label();
            this.scanDepthTrackBar = new System.Windows.Forms.TrackBar();
            this.scanDepthValueLabel = new System.Windows.Forms.Label();
            this.SkipTrackbar = new System.Windows.Forms.TrackBar();
            this.skipValueLabel = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.portEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.portsLabel = new System.Windows.Forms.Label();
            this.delayValueLabel = new System.Windows.Forms.Label();
            this.delayTrackbar = new System.Windows.Forms.TrackBar();
            this.delayLabel = new System.Windows.Forms.Label();
            this.previewCheckbox = new System.Windows.Forms.CheckBox();
            this.previewImage = new System.Windows.Forms.PictureBox();
            this.idleTrackbar = new System.Windows.Forms.TrackBar();
            this.idleLabel = new System.Windows.Forms.Label();
            this.idleValueLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gammaTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanDepthTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SkipTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idleTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // gammaValueLabel
            // 
            this.gammaValueLabel.AutoSize = true;
            this.gammaValueLabel.Location = new System.Drawing.Point(55, 110);
            this.gammaValueLabel.Name = "gammaValueLabel";
            this.gammaValueLabel.Size = new System.Drawing.Size(73, 13);
            this.gammaValueLabel.TabIndex = 10;
            this.gammaValueLabel.Text = "Gamma value";
            // 
            // cpuLabel
            // 
            this.cpuLabel.AutoSize = true;
            this.cpuLabel.Location = new System.Drawing.Point(12, 22);
            this.cpuLabel.Name = "cpuLabel";
            this.cpuLabel.Size = new System.Drawing.Size(25, 13);
            this.cpuLabel.TabIndex = 9;
            this.cpuLabel.Text = "CPU";
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(12, 9);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(25, 13);
            this.fpsLabel.TabIndex = 8;
            this.fpsLabel.Text = "FPS";
            // 
            // hideButton
            // 
            this.hideButton.Location = new System.Drawing.Point(262, 12);
            this.hideButton.Name = "hideButton";
            this.hideButton.Size = new System.Drawing.Size(39, 23);
            this.hideButton.TabIndex = 15;
            this.hideButton.Text = "Hide";
            this.hideButton.UseVisualStyleBackColor = true;
            this.hideButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // aboutLabel
            // 
            this.aboutLabel.AutoSize = true;
            this.aboutLabel.Enabled = false;
            this.aboutLabel.Location = new System.Drawing.Point(172, 557);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(133, 13);
            this.aboutLabel.TabIndex = 13;
            this.aboutLabel.Text = "Author: Piet Vandeput";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Ambilight DFMirage v1.1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(79, 48);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(78, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(78, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // scanDepthLabel
            // 
            this.scanDepthLabel.AutoSize = true;
            this.scanDepthLabel.Location = new System.Drawing.Point(12, 170);
            this.scanDepthLabel.Name = "scanDepthLabel";
            this.scanDepthLabel.Size = new System.Drawing.Size(61, 13);
            this.scanDepthLabel.TabIndex = 16;
            this.scanDepthLabel.Text = "ScanDepth";
            // 
            // gammaTrackBar
            // 
            this.gammaTrackBar.Location = new System.Drawing.Point(12, 126);
            this.gammaTrackBar.Maximum = 100;
            this.gammaTrackBar.Name = "gammaTrackBar";
            this.gammaTrackBar.Size = new System.Drawing.Size(284, 45);
            this.gammaTrackBar.TabIndex = 18;
            this.gammaTrackBar.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // gammaLabel
            // 
            this.gammaLabel.AutoSize = true;
            this.gammaLabel.Location = new System.Drawing.Point(12, 110);
            this.gammaLabel.Name = "gammaLabel";
            this.gammaLabel.Size = new System.Drawing.Size(37, 13);
            this.gammaLabel.TabIndex = 19;
            this.gammaLabel.Text = "Gamma";
            // 
            // timer2
            // 
            this.timer2.Interval = 30000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // multiThreadingCheckBox
            // 
            this.multiThreadingCheckBox.AutoSize = true;
            this.multiThreadingCheckBox.Location = new System.Drawing.Point(15, 38);
            this.multiThreadingCheckBox.Name = "multiThreadingCheckBox";
            this.multiThreadingCheckBox.Size = new System.Drawing.Size(110, 17);
            this.multiThreadingCheckBox.TabIndex = 20;
            this.multiThreadingCheckBox.Text = "MultiThreading";
            this.multiThreadingCheckBox.UseVisualStyleBackColor = true;
            this.multiThreadingCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // skipLabel
            // 
            this.skipLabel.AutoSize = true;
            this.skipLabel.Location = new System.Drawing.Point(12, 234);
            this.skipLabel.Name = "skipLabel";
            this.skipLabel.Size = new System.Drawing.Size(157, 13);
            this.skipLabel.TabIndex = 23;
            this.skipLabel.Text = "PixelsToSkipPerCoordinate";
            // 
            // scanDepthTrackBar
            // 
            this.scanDepthTrackBar.Location = new System.Drawing.Point(12, 186);
            this.scanDepthTrackBar.Maximum = 200;
            this.scanDepthTrackBar.Minimum = 1;
            this.scanDepthTrackBar.Name = "scanDepthTrackBar";
            this.scanDepthTrackBar.Size = new System.Drawing.Size(284, 45);
            this.scanDepthTrackBar.TabIndex = 24;
            this.scanDepthTrackBar.Value = 1;
            this.scanDepthTrackBar.Scroll += new System.EventHandler(this.trackBar2_ValueChanged);
            this.scanDepthTrackBar.ValueChanged += new System.EventHandler(this.trackBar2_ValueChanged);
            // 
            // scanDepthValueLabel
            // 
            this.scanDepthValueLabel.AutoSize = true;
            this.scanDepthValueLabel.Location = new System.Drawing.Point(79, 170);
            this.scanDepthValueLabel.Name = "scanDepthValueLabel";
            this.scanDepthValueLabel.Size = new System.Drawing.Size(97, 13);
            this.scanDepthValueLabel.TabIndex = 25;
            this.scanDepthValueLabel.Text = "ScanDepth value";
            // 
            // SkipTrackbar
            // 
            this.SkipTrackbar.Location = new System.Drawing.Point(12, 250);
            this.SkipTrackbar.Maximum = 200;
            this.SkipTrackbar.Minimum = 1;
            this.SkipTrackbar.Name = "SkipTrackbar";
            this.SkipTrackbar.Size = new System.Drawing.Size(284, 45);
            this.SkipTrackbar.TabIndex = 26;
            this.SkipTrackbar.Value = 1;
            this.SkipTrackbar.Scroll += new System.EventHandler(this.trackBar3_ValueChanged);
            this.SkipTrackbar.ValueChanged += new System.EventHandler(this.trackBar3_ValueChanged);
            // 
            // skipValueLabel
            // 
            this.skipValueLabel.AutoSize = true;
            this.skipValueLabel.Location = new System.Drawing.Point(175, 234);
            this.skipValueLabel.Name = "skipValueLabel";
            this.skipValueLabel.Size = new System.Drawing.Size(67, 13);
            this.skipValueLabel.TabIndex = 27;
            this.skipValueLabel.Text = "Skip value";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 426);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(281, 69);
            this.listBox1.TabIndex = 28;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // portEnabledCheckbox
            // 
            this.portEnabledCheckbox.AutoSize = true;
            this.portEnabledCheckbox.Location = new System.Drawing.Point(12, 502);
            this.portEnabledCheckbox.Name = "portEnabledCheckbox";
            this.portEnabledCheckbox.Size = new System.Drawing.Size(92, 17);
            this.portEnabledCheckbox.TabIndex = 29;
            this.portEnabledCheckbox.Text = "Enable port";
            this.portEnabledCheckbox.UseVisualStyleBackColor = true;
            this.portEnabledCheckbox.CheckedChanged += new System.EventHandler(this.portEnabledCheckbox_CheckedChanged);
            // 
            // portsLabel
            // 
            this.portsLabel.AutoSize = true;
            this.portsLabel.Location = new System.Drawing.Point(12, 393);
            this.portsLabel.Name = "portsLabel";
            this.portsLabel.Size = new System.Drawing.Size(37, 13);
            this.portsLabel.TabIndex = 30;
            this.portsLabel.Text = "Ports";
            // 
            // delayValueLabel
            // 
            this.delayValueLabel.AutoSize = true;
            this.delayValueLabel.Location = new System.Drawing.Point(55, 294);
            this.delayValueLabel.Name = "delayValueLabel";
            this.delayValueLabel.Size = new System.Drawing.Size(73, 13);
            this.delayValueLabel.TabIndex = 33;
            this.delayValueLabel.Text = "Delay value";
            // 
            // delayTrackbar
            // 
            this.delayTrackbar.Location = new System.Drawing.Point(12, 310);
            this.delayTrackbar.Maximum = 1000;
            this.delayTrackbar.Name = "delayTrackbar";
            this.delayTrackbar.Size = new System.Drawing.Size(284, 45);
            this.delayTrackbar.TabIndex = 32;
            this.delayTrackbar.Value = 1;
            this.delayTrackbar.ValueChanged += new System.EventHandler(this.delayTrackbar_ValueChanged);
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(12, 294);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(37, 13);
            this.delayLabel.TabIndex = 31;
            this.delayLabel.Text = "Delay";
            // 
            // previewCheckbox
            // 
            this.previewCheckbox.AutoSize = true;
            this.previewCheckbox.Location = new System.Drawing.Point(15, 62);
            this.previewCheckbox.Name = "previewCheckbox";
            this.previewCheckbox.Size = new System.Drawing.Size(104, 17);
            this.previewCheckbox.TabIndex = 34;
            this.previewCheckbox.Text = "Color preview";
            this.previewCheckbox.UseVisualStyleBackColor = true;
            this.previewCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // previewImage
            // 
            this.previewImage.Location = new System.Drawing.Point(322, 38);
            this.previewImage.Name = "previewImage";
            this.previewImage.Size = new System.Drawing.Size(200, 120);
            this.previewImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.previewImage.TabIndex = 35;
            this.previewImage.TabStop = false;
            // 
            // idleTrackbar
            // 
            this.idleTrackbar.Location = new System.Drawing.Point(12, 375);
            this.idleTrackbar.Maximum = 2000;
            this.idleTrackbar.Minimum = 20;
            this.idleTrackbar.Name = "idleTrackbar";
            this.idleTrackbar.Size = new System.Drawing.Size(281, 45);
            this.idleTrackbar.TabIndex = 36;
            this.idleTrackbar.Value = 1000;
            this.idleTrackbar.Scroll += new System.EventHandler(this.idleTrackbar_Scroll);
            // 
            // idleLabel
            // 
            this.idleLabel.AutoSize = true;
            this.idleLabel.Location = new System.Drawing.Point(15, 356);
            this.idleLabel.Name = "idleLabel";
            this.idleLabel.Size = new System.Drawing.Size(85, 13);
            this.idleLabel.TabIndex = 37;
            this.idleLabel.Text = "Frame timeout";
            // 
            // idleValueLabel
            // 
            this.idleValueLabel.AutoSize = true;
            this.idleValueLabel.Location = new System.Drawing.Point(106, 356);
            this.idleValueLabel.Name = "idleValueLabel";
            this.idleValueLabel.Size = new System.Drawing.Size(121, 13);
            this.idleValueLabel.TabIndex = 38;
            this.idleValueLabel.Text = "Frame timeout value";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 578);
            this.Controls.Add(this.idleValueLabel);
            this.Controls.Add(this.idleLabel);
            this.Controls.Add(this.idleTrackbar);
            this.Controls.Add(this.previewImage);
            this.Controls.Add(this.previewCheckbox);
            this.Controls.Add(this.delayValueLabel);
            this.Controls.Add(this.delayTrackbar);
            this.Controls.Add(this.delayLabel);
            this.Controls.Add(this.portsLabel);
            this.Controls.Add(this.portEnabledCheckbox);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.skipValueLabel);
            this.Controls.Add(this.SkipTrackbar);
            this.Controls.Add(this.scanDepthValueLabel);
            this.Controls.Add(this.scanDepthTrackBar);
            this.Controls.Add(this.skipLabel);
            this.Controls.Add(this.multiThreadingCheckBox);
            this.Controls.Add(this.gammaLabel);
            this.Controls.Add(this.gammaTrackBar);
            this.Controls.Add(this.scanDepthLabel);
            this.Controls.Add(this.hideButton);
            this.Controls.Add(this.aboutLabel);
            this.Controls.Add(this.gammaValueLabel);
            this.Controls.Add(this.cpuLabel);
            this.Controls.Add(this.fpsLabel);
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HueLight+";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gammaTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanDepthTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SkipTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idleTrackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label gammaValueLabel;
        private System.Windows.Forms.Label cpuLabel;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.Button hideButton;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label scanDepthLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.TrackBar gammaTrackBar;
        private System.Windows.Forms.Label gammaLabel;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.CheckBox multiThreadingCheckBox;
        private System.Windows.Forms.Label skipLabel;
        private System.Windows.Forms.TrackBar scanDepthTrackBar;
        private System.Windows.Forms.Label scanDepthValueLabel;
        private System.Windows.Forms.TrackBar SkipTrackbar;
        private System.Windows.Forms.Label skipValueLabel;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox portEnabledCheckbox;
        private System.Windows.Forms.Label portsLabel;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.Label delayValueLabel;
        private System.Windows.Forms.TrackBar delayTrackbar;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.CheckBox previewCheckbox;
        private System.Windows.Forms.PictureBox previewImage;
        private System.Windows.Forms.TrackBar idleTrackbar;
        private System.Windows.Forms.Label idleLabel;
        private System.Windows.Forms.Label idleValueLabel;
    }
}

