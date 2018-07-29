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
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.label4 = new System.Windows.Forms.Label();
            this.VictoryMusicFile = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.setres = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
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
            this.button1.Location = new System.Drawing.Point(596, 55);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TitleMenuFile
            // 
            this.TitleMenuFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.TitleMenuFile.Location = new System.Drawing.Point(18, 55);
            this.TitleMenuFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TitleMenuFile.Multiline = false;
            this.TitleMenuFile.Name = "TitleMenuFile";
            this.TitleMenuFile.ReadOnly = true;
            this.TitleMenuFile.Size = new System.Drawing.Size(566, 33);
            this.TitleMenuFile.TabIndex = 1;
            this.TitleMenuFile.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(431, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Title Menu (Save the world or battle royale selection screen):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 95);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Main Menu";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MenuMusicFile
            // 
            this.MenuMusicFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.MenuMusicFile.Location = new System.Drawing.Point(18, 137);
            this.MenuMusicFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MenuMusicFile.Multiline = false;
            this.MenuMusicFile.Name = "MenuMusicFile";
            this.MenuMusicFile.ReadOnly = true;
            this.MenuMusicFile.Size = new System.Drawing.Size(566, 33);
            this.MenuMusicFile.TabIndex = 5;
            this.MenuMusicFile.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(596, 137);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 4;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(453, 263);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(255, 69);
            this.trackBar1.TabIndex = 8;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 263);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Volume:";
            // 
            // VolumeNum
            // 
            this.VolumeNum.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.VolumeNum.AutoSize = true;
            this.VolumeNum.Location = new System.Drawing.Point(406, 283);
            this.VolumeNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VolumeNum.Name = "VolumeNum";
            this.VolumeNum.Size = new System.Drawing.Size(36, 20);
            this.VolumeNum.TabIndex = 10;
            this.VolumeNum.Text = "100";
            this.VolumeNum.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(476, 152);
            this.axWindowsMediaPlayer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(0, 0);
            this.axWindowsMediaPlayer1.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 177);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Victory";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // VictoryMusicFile
            // 
            this.VictoryMusicFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.VictoryMusicFile.Location = new System.Drawing.Point(18, 218);
            this.VictoryMusicFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.VictoryMusicFile.Multiline = false;
            this.VictoryMusicFile.Name = "VictoryMusicFile";
            this.VictoryMusicFile.ReadOnly = true;
            this.VictoryMusicFile.Size = new System.Drawing.Size(566, 33);
            this.VictoryMusicFile.TabIndex = 12;
            this.VictoryMusicFile.Text = "";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(596, 218);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 35);
            this.button3.TabIndex = 11;
            this.button3.Text = "Browse";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(202, 277);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(119, 24);
            this.checkBox2.TabIndex = 14;
            this.checkBox2.Text = "Play in party";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(18, 277);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(176, 24);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "Play when obscured";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // setres
            // 
            this.setres.Location = new System.Drawing.Point(562, 7);
            this.setres.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setres.Name = "setres";
            this.setres.Size = new System.Drawing.Size(151, 35);
            this.setres.TabIndex = 16;
            this.setres.Text = "Set Resolution";
            this.setres.UseVisualStyleBackColor = true;
            this.setres.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 325);
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
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Fortnite Music Changer";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox TitleMenuFile;
        private System.Windows.Forms.Label label1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
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
    }
}

