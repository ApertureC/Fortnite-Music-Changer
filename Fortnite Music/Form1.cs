using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Configuration;
namespace Fortnite_Music
{
    public partial class Form1 : Form
    {
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        //
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        public static class Globals
        {
            public static string titlemenu = "";
            public static string mainmenu = "";
        }
        private bool mainmenumusic()
        {
            var c = GetColorAt(new Point(512, 36));
            if (Int32.Parse(c.R.ToString()) == 28 && Int32.Parse(c.G.ToString()) == 34 && Int32.Parse(c.B.ToString()) == 56)
            {
                c = GetColorAt(new Point(909, 1047));
                if (Int32.Parse(c.R.ToString()) == 21 && Int32.Parse(c.G.ToString()) == 24 && Int32.Parse(c.B.ToString()) == 43) {
                    c = GetColorAt(new Point(20, 1043));
                    if (Int32.Parse(c.R.ToString()) == 255 && Int32.Parse(c.G.ToString()) == 255 && Int32.Parse(c.B.ToString()) == 255) {
                        return true;
                    }
                }
            }
            return false;
        }
        public Form1()
        {
            InitializeComponent();
            // SETTINGS LOADING

            //
            //while (true)
            //{
            //    Debug.WriteLine(System.Windows.Forms.Cursor.Position.ToString());
            //    Debug.WriteLine(GetColorAt(new Point(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y)).ToString());
            //}

            // SETTINGS

            Debug.WriteLine("Loaded");
            Globals.mainmenu = Properties.Settings.Default.MainMenu;
            Debug.WriteLine(Globals.mainmenu);
            Globals.titlemenu = Properties.Settings.Default.TitleMenu;
            checkBox1.Checked = Properties.Settings.Default.Obscure;
            trackBar1.Value = Properties.Settings.Default.Volume;

            // APPLY SETTINGS
            MenuMusicFile.Text = Globals.mainmenu;
            TitleMenuFile.Text = Globals.titlemenu;

            //
            new Thread(() =>
            {
                wplayer.settings.setMode("loop", true);
                wplayer.settings.setMode("autoStart", false);
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    MethodInvoker mouse = delegate
                    {
                        label1.Text = System.Windows.Forms.Cursor.Position.ToString();
                        //label2.Text = GetColorAt();
                        return;
                    };
                    //this.Invoke(mouse);
                    if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                    {
                        bool focused = false;
                        uint pid;
                        GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                        {
                            if (p.Id == pid)
                            {
                                if (p.ProcessName== "FortniteClient-Win64-Shipping")
                                {
                                    focused = true;
                                    break;
                                }
                                
                            }
                        }
                        var c = GetColorAt(new Point(1058, 28));
                        if (Int32.Parse(c.R.ToString())>250 && Int32.Parse(c.G.ToString())>250 && Int32.Parse(c.B.ToString())>250)
                        {
                            c = GetColorAt(new Point(941, 1066));
                            if (Int32.Parse(c.R.ToString())== 220 && Int32.Parse(c.G.ToString()) == 229 && Int32.Parse(c.B.ToString())==244)
                            {
                                if ((wplayer.URL != Globals.titlemenu))
                                {
                                    wplayer.URL = Globals.titlemenu;
                                    wplayer.controls.play();
                                }
                            }
                        } else if (mainmenumusic()==true)
                        {
                            if ((wplayer.URL != Globals.mainmenu))
                            {
                                wplayer.URL = Globals.mainmenu;
                                wplayer.controls.play();
                            }
                        } else
                        {
                            if (checkBox1.Checked == false || focused==true)
                            {
                                wplayer.controls.pause();
                                wplayer.URL = "";
                            }
                        }
                    } else
                    {
                        wplayer.controls.pause();
                        wplayer.URL = "";
                    }
                }
            }).Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                Globals.titlemenu = openFileDialog1.FileName;
                TitleMenuFile.Text = openFileDialog1.FileName;
                Properties.Settings.Default.TitleMenu = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
                Debug.WriteLine("Hi");
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }
        private void PollPixel(Point location, Color color)
        {
            while (true)
            {
                var c = GetColorAt(location);

                if (c.R == color.R && c.G == color.G && c.B == color.B)
                {
                    return;
                }

                // By calling Thread.Sleep() without a parameter, we are signaling to the
                // operating system that we only want to sleep long enough for other
                // applications.  As soon as the other apps yield their CPU time, we will
                // regain control.
                Thread.Sleep(100);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Globals.mainmenu = openFileDialog1.FileName;
                MenuMusicFile.Text = openFileDialog1.FileName;
                Properties.Settings.Default.MainMenu = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Obscure = checkBox1.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            wplayer.settings.volume = trackBar1.Value;
            VolumeNum.Text = trackBar1.Value.ToString();
            Properties.Settings.Default.Volume = trackBar1.Value;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
