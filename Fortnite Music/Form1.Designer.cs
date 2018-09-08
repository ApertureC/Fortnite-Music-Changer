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
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.label3 = new System.Windows.Forms.Label();
			this.VolumeNum = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.VictoryMusicFile = new System.Windows.Forms.RichTextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.setres = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.licensesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label5 = new System.Windows.Forms.Label();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.button5 = new System.Windows.Forms.Button();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.button2 = new System.Windows.Forms.Button();
			this.MenuMusicFile = new System.Windows.Forms.RichTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.TitleMenuFile = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.launchOnStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startMinimizedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
			// 
			// trackBar1
			// 
			resources.ApplyResources(this.trackBar1, "trackBar1");
			this.trackBar1.Maximum = 100;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Value = 100;
			this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// VolumeNum
			// 
			resources.ApplyResources(this.VolumeNum, "VolumeNum");
			this.VolumeNum.Name = "VolumeNum";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// VictoryMusicFile
			// 
			resources.ApplyResources(this.VictoryMusicFile, "VictoryMusicFile");
			this.VictoryMusicFile.Cursor = System.Windows.Forms.Cursors.Default;
			this.VictoryMusicFile.Name = "VictoryMusicFile";
			this.VictoryMusicFile.ReadOnly = true;
			// 
			// button3
			// 
			resources.ApplyResources(this.button3, "button3");
			this.button3.Name = "button3";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// checkBox1
			// 
			resources.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// setres
			// 
			resources.ApplyResources(this.setres, "setres");
			this.setres.Name = "setres";
			this.setres.UseVisualStyleBackColor = true;
			this.setres.Click += new System.EventHandler(this.button4_Click);
			// 
			// button4
			// 
			resources.ApplyResources(this.button4, "button4");
			this.button4.Name = "button4";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click_1);
			// 
			// menuStrip1
			// 
			resources.ApplyResources(this.menuStrip1, "menuStrip1");
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.startupToolStripMenuItem});
			this.menuStrip1.Name = "menuStrip1";
			// 
			// aboutToolStripMenuItem
			// 
			resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.licensesToolStripMenuItem,
            this.githubToolStripMenuItem,
            this.redditToolStripMenuItem});
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			// 
			// licensesToolStripMenuItem
			// 
			resources.ApplyResources(this.licensesToolStripMenuItem, "licensesToolStripMenuItem");
			this.licensesToolStripMenuItem.Name = "licensesToolStripMenuItem";
			this.licensesToolStripMenuItem.Click += new System.EventHandler(this.licensesToolStripMenuItem_Click);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// checkBox4
			// 
			resources.ApplyResources(this.checkBox4, "checkBox4");
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.UseVisualStyleBackColor = true;
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			// 
			// button5
			// 
			resources.ApplyResources(this.button5, "button5");
			this.button5.Name = "button5";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// linkLabel1
			// 
			resources.ApplyResources(this.linkLabel1, "linkLabel1");
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.TabStop = true;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// button2
			// 
			resources.ApplyResources(this.button2, "button2");
			this.button2.Name = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// MenuMusicFile
			// 
			resources.ApplyResources(this.MenuMusicFile, "MenuMusicFile");
			this.MenuMusicFile.Cursor = System.Windows.Forms.Cursors.Default;
			this.MenuMusicFile.Name = "MenuMusicFile";
			this.MenuMusicFile.ReadOnly = true;
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// button1
			// 
			resources.ApplyResources(this.button1, "button1");
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// TitleMenuFile
			// 
			resources.ApplyResources(this.TitleMenuFile, "TitleMenuFile");
			this.TitleMenuFile.Cursor = System.Windows.Forms.Cursors.Default;
			this.TitleMenuFile.Name = "TitleMenuFile";
			this.TitleMenuFile.ReadOnly = true;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// launchOnStartupToolStripMenuItem
			// 
			resources.ApplyResources(this.launchOnStartupToolStripMenuItem, "launchOnStartupToolStripMenuItem");
			this.launchOnStartupToolStripMenuItem.CheckOnClick = true;
			this.launchOnStartupToolStripMenuItem.Name = "launchOnStartupToolStripMenuItem";
			this.launchOnStartupToolStripMenuItem.Click += new System.EventHandler(this.launchOnStartupToolStripMenuItem_Click);
			// 
			// startMinimizedToolStripMenuItem
			// 
			resources.ApplyResources(this.startMinimizedToolStripMenuItem, "startMinimizedToolStripMenuItem");
			this.startMinimizedToolStripMenuItem.CheckOnClick = true;
			this.startMinimizedToolStripMenuItem.Name = "startMinimizedToolStripMenuItem";
			this.startMinimizedToolStripMenuItem.Click += new System.EventHandler(this.startMinimizedToolStripMenuItem_Click);
			// 
			// startupToolStripMenuItem
			// 
			resources.ApplyResources(this.startupToolStripMenuItem, "startupToolStripMenuItem");
			this.startupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchOnStartupToolStripMenuItem,
            this.startMinimizedToolStripMenuItem});
			this.startupToolStripMenuItem.Name = "startupToolStripMenuItem";
			// 
			// redditToolStripMenuItem
			// 
			resources.ApplyResources(this.redditToolStripMenuItem, "redditToolStripMenuItem");
			this.redditToolStripMenuItem.Name = "redditToolStripMenuItem";
			this.redditToolStripMenuItem.Click += new System.EventHandler(this.redditToolStripMenuItem_Click);
			// 
			// githubToolStripMenuItem
			// 
			resources.ApplyResources(this.githubToolStripMenuItem, "githubToolStripMenuItem");
			this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
			this.githubToolStripMenuItem.Click += new System.EventHandler(this.githubToolStripMenuItem_Click);
			// 
			// Form1
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.checkBox4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.setres);
			this.Controls.Add(this.checkBox1);
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
			this.MaximizeBox = false;
			this.Name = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label VolumeNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox VictoryMusicFile;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button setres;
        //private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licensesToolStripMenuItem;
        private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.ToolStripMenuItem githubToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem redditToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem startupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem launchOnStartupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem startMinimizedToolStripMenuItem;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.RichTextBox MenuMusicFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RichTextBox TitleMenuFile;
		private System.Windows.Forms.Label label1;
	}
}
