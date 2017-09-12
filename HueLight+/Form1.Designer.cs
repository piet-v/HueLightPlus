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
            this.label6 = new System.Windows.Forms.Label();
            this.gammaValueLabel = new System.Windows.Forms.Label();
            this.cpuLabel = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.scanDepthLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.gammaTrackBar = new System.Windows.Forms.TrackBar();
            this.gammaLabel = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.multiThreadingCheckBox = new System.Windows.Forms.CheckBox();
            this.skipLabel = new System.Windows.Forms.Label();
            this.scanDepthTrackBar = new System.Windows.Forms.TrackBar();
            this.scanDepthValueLabel = new System.Windows.Forms.Label();
            this.SkipTrackbar = new System.Windows.Forms.TrackBar();
            this.skipValueLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gammaTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanDepthTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SkipTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Hue config";
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
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(257, 343);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(39, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Hide";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(12, 348);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Author: Piet Vandeput";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Ambilight DFMirage v1.1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(79, 26);
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(176, 343);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Capture SC";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.multiThreadingCheckBox.Location = new System.Drawing.Point(12, 90);
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
            this.scanDepthTrackBar.Name = "scanDepthTrackBar";
            this.scanDepthTrackBar.Size = new System.Drawing.Size(284, 45);
            this.scanDepthTrackBar.TabIndex = 24;
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
            this.SkipTrackbar.Name = "SkipTrackbar";
            this.SkipTrackbar.Size = new System.Drawing.Size(284, 45);
            this.SkipTrackbar.TabIndex = 26;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 376);
            this.Controls.Add(this.skipValueLabel);
            this.Controls.Add(this.SkipTrackbar);
            this.Controls.Add(this.scanDepthValueLabel);
            this.Controls.Add(this.scanDepthTrackBar);
            this.Controls.Add(this.skipLabel);
            this.Controls.Add(this.multiThreadingCheckBox);
            this.Controls.Add(this.gammaLabel);
            this.Controls.Add(this.gammaTrackBar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.scanDepthLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
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
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gammaTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanDepthTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SkipTrackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label gammaValueLabel;
        private System.Windows.Forms.Label cpuLabel;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label scanDepthLabel;
        private System.Windows.Forms.Button button1;
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
    }
}

