namespace Fortnite_Music // TO DO: Remove "OpenFileDialog1" from the auto complete thing
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.button1 = new System.Windows.Forms.Button();
			this.TitleMenuFile = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.MenuMusicFile = new System.Windows.Forms.RichTextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.label3 = new System.Windows.Forms.Label();
			this.VolumeNum = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.VictoryMusicFile = new System.Windows.Forms.RichTextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.setres = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.licensesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.launchOnStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startMinimizedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label5 = new System.Windows.Forms.Label();
			this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Filter = "Audio Files | *.mp3;*.wav";
			this.openFileDialog1.Title = "Select an Audio File";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(397, 52);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Browse";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// TitleMenuFile
			// 
			this.TitleMenuFile.Cursor = System.Windows.Forms.Cursors.Default;
			this.TitleMenuFile.Location = new System.Drawing.Point(12, 52);
			this.TitleMenuFile.Multiline = false;
			this.TitleMenuFile.Name = "TitleMenuFile";
			this.TitleMenuFile.ReadOnly = true;
			this.TitleMenuFile.Size = new System.Drawing.Size(379, 23);
			this.TitleMenuFile.TabIndex = 1;
			this.TitleMenuFile.Text = "";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Title menu (STW or BR selection screen)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Main Menu";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// MenuMusicFile
			// 
			this.MenuMusicFile.Cursor = System.Windows.Forms.Cursors.Default;
			this.MenuMusicFile.Location = new System.Drawing.Point(12, 105);
			this.MenuMusicFile.Multiline = false;
			this.MenuMusicFile.Name = "MenuMusicFile";
			this.MenuMusicFile.ReadOnly = true;
			this.MenuMusicFile.Size = new System.Drawing.Size(379, 23);
			this.MenuMusicFile.TabIndex = 5;
			this.MenuMusicFile.Text = "";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(397, 105);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 4;
			this.button2.Text = "Browse";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(302, 187);
			this.trackBar1.Maximum = 100;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(170, 45);
			this.trackBar1.TabIndex = 8;
			this.trackBar1.Value = 100;
			this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(251, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Volume:";
			// 
			// VolumeNum
			// 
			this.VolumeNum.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.VolumeNum.AutoSize = true;
			this.VolumeNum.Location = new System.Drawing.Point(271, 200);
			this.VolumeNum.Name = "VolumeNum";
			this.VolumeNum.Size = new System.Drawing.Size(25, 13);
			this.VolumeNum.TabIndex = 10;
			this.VolumeNum.Text = "100";
			this.VolumeNum.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 131);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(356, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "Victory (May not work below .50 brightness - stretched may not work at all)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// VictoryMusicFile
			// 
			this.VictoryMusicFile.Cursor = System.Windows.Forms.Cursors.Default;
			this.VictoryMusicFile.Location = new System.Drawing.Point(12, 158);
			this.VictoryMusicFile.Multiline = false;
			this.VictoryMusicFile.Name = "VictoryMusicFile";
			this.VictoryMusicFile.ReadOnly = true;
			this.VictoryMusicFile.Size = new System.Drawing.Size(379, 23);
			this.VictoryMusicFile.TabIndex = 12;
			this.VictoryMusicFile.Text = "";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(397, 158);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 11;
			this.button3.Text = "Browse";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(135, 196);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(126, 17);
			this.checkBox2.TabIndex = 14;
			this.checkBox2.Text = "Stretched Mode (4:3)";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(12, 196);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(122, 17);
			this.checkBox1.TabIndex = 15;
			this.checkBox1.Text = "Play when obscured";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// setres
			// 
			this.setres.Location = new System.Drawing.Point(375, 21);
			this.setres.Name = "setres";
			this.setres.Size = new System.Drawing.Size(101, 23);
			this.setres.TabIndex = 16;
			this.setres.Text = "Set Resolution";
			this.setres.UseVisualStyleBackColor = true;
			this.setres.Click += new System.EventHandler(this.button4_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(270, 21);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(101, 23);
			this.button4.TabIndex = 18;
			this.button4.Text = "Menu setup";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click_1);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.startupToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(484, 24);
			this.menuStrip1.TabIndex = 19;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.licensesToolStripMenuItem,
            this.githubToolStripMenuItem,
            this.redditToolStripMenuItem});
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// licensesToolStripMenuItem
			// 
			this.licensesToolStripMenuItem.Name = "licensesToolStripMenuItem";
			this.licensesToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.licensesToolStripMenuItem.Text = "Licenses";
			this.licensesToolStripMenuItem.Click += new System.EventHandler(this.licensesToolStripMenuItem_Click);
			// 
			// githubToolStripMenuItem
			// 
			this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
			this.githubToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.githubToolStripMenuItem.Text = "Github (For reporting bugs)";
			this.githubToolStripMenuItem.Click += new System.EventHandler(this.githubToolStripMenuItem_Click);
			// 
			// redditToolStripMenuItem
			// 
			this.redditToolStripMenuItem.Name = "redditToolStripMenuItem";
			this.redditToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.redditToolStripMenuItem.Text = "Reddit";
			this.redditToolStripMenuItem.Click += new System.EventHandler(this.redditToolStripMenuItem_Click);
			// 
			// startupToolStripMenuItem
			// 
			this.startupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchOnStartupToolStripMenuItem,
            this.startMinimizedToolStripMenuItem});
			this.startupToolStripMenuItem.Name = "startupToolStripMenuItem";
			this.startupToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.startupToolStripMenuItem.Text = "Startup";
			// 
			// launchOnStartupToolStripMenuItem
			// 
			this.launchOnStartupToolStripMenuItem.CheckOnClick = true;
			this.launchOnStartupToolStripMenuItem.Name = "launchOnStartupToolStripMenuItem";
			this.launchOnStartupToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.launchOnStartupToolStripMenuItem.Text = "Launch on startup";
			this.launchOnStartupToolStripMenuItem.Click += new System.EventHandler(this.launchOnStartupToolStripMenuItem_Click);
			// 
			// startMinimizedToolStripMenuItem
			// 
			this.startMinimizedToolStripMenuItem.CheckOnClick = true;
			this.startMinimizedToolStripMenuItem.Name = "startMinimizedToolStripMenuItem";
			this.startMinimizedToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.startMinimizedToolStripMenuItem.Text = "Start minimized";
			this.startMinimizedToolStripMenuItem.Click += new System.EventHandler(this.startMinimizedToolStripMenuItem_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 260);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(283, 13);
			this.label5.TabIndex = 20;
			this.label5.Text = "By /u/ApertureCoder (Click About -> Github to report bugs)";
			// 
			// axWindowsMediaPlayer1
			// 
			this.axWindowsMediaPlayer1.Enabled = true;
			this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(251, 118);
			this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
			this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
			this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(0, 0);
			this.axWindowsMediaPlayer1.TabIndex = 17;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Location = new System.Drawing.Point(10, 219);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(446, 17);
			this.checkBox3.TabIndex = 21;
			this.checkBox3.Text = "Fullscreen Stutter Reducer (Slows program down - takes about 3 sec for music to a" +
    "ppear)";
			this.checkBox3.UseVisualStyleBackColor = true;
			this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Location = new System.Drawing.Point(10, 242);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(85, 17);
			this.checkBox4.TabIndex = 22;
			this.checkBox4.Text = "Play In Party";
			this.checkBox4.UseVisualStyleBackColor = true;
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(484, 282);
			this.Controls.Add(this.checkBox4);
			this.Controls.Add(this.checkBox3);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.axWindowsMediaPlayer1);
			this.Controls.Add(this.setres);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.VictoryMusicFile);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.VolumeNum);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.MenuMusicFile);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TitleMenuFile);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Fortnite Music Changer";
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox TitleMenuFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox MenuMusicFile;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label VolumeNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox VictoryMusicFile;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button setres;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licensesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem githubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redditToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem startupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem launchOnStartupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMinimizedToolStripMenuItem;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
	}
}
