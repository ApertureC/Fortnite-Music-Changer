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
using Microsoft.Win32;
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
            public static string victory = "";
            public static bool party = false;
        }
        private bool mainmenumusic(double sfx, double sfy)
        {
            Color colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(428f * sfx)), Convert.ToInt32(Math.Round(548f * sfy))));
            if (int.Parse(colorAt.R.ToString()) == 11 && int.Parse(colorAt.G.ToString()) == 19 && int.Parse(colorAt.B.ToString()) == 47)
            {
                return false;
            }
            colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(512f * sfx)), Convert.ToInt32(Math.Round(36f * sfy))));
            Debug.WriteLine(colorAt);
            if (int.Parse(colorAt.R.ToString()) == 28 && int.Parse(colorAt.G.ToString()) == 34 && int.Parse(colorAt.B.ToString()) == 56)
            {
                colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(909f * sfx)), Convert.ToInt32(Math.Round(1047f * sfy))));
                Debug.WriteLine(colorAt);

                if (int.Parse(colorAt.R.ToString()) == 21 && int.Parse(colorAt.G.ToString()) == 24 && int.Parse(colorAt.B.ToString()) == 43)
                {
                    colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(20f * sfx)), Convert.ToInt32(Math.Round(1043f * sfy))));

                    if (int.Parse(colorAt.R.ToString()) >= 200 && int.Parse(colorAt.G.ToString()) >=200 && int.Parse(colorAt.B.ToString()) >= 200)
                    {
                        return true;
                    }
                    if (int.Parse(colorAt.R.ToString()) == 99 && int.Parse(colorAt.G.ToString()) == 188 && int.Parse(colorAt.B.ToString()) == 80 && Globals.party)
                    {
                        return true;
                    }
                }
            }
            colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
            if (int.Parse(colorAt.R.ToString()) >= 250 && int.Parse(colorAt.G.ToString()) >= 250 && int.Parse(colorAt.B.ToString()) >= 250)
            {
                colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                if (int.Parse(colorAt.R.ToString()) == 232 && int.Parse(colorAt.G.ToString()) == 232 && int.Parse(colorAt.B.ToString()) == 232)
                {
                    return true;
                }
            }
            return false;
        }
        private bool victorymusic(double sfx, double sfy)
        {
            Color colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(911f * sfx)), Convert.ToInt32(Math.Round(251f * sfy))));
           
            if (int.Parse(colorAt.R.ToString()) == 242 && int.Parse(colorAt.G.ToString()) == 247 && int.Parse(colorAt.B.ToString()) == 252)
            {
                colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(1087f * sfx)), Convert.ToInt32(Math.Round(271f * sfy))));
                if (int.Parse(colorAt.R.ToString()) == 255 && int.Parse(colorAt.G.ToString()) == 255 && int.Parse(colorAt.B.ToString()) == 255)
                {
                    return true;
                }
            }
            return false;
        }
        public Form1()
        {
            InitializeComponent();
            // SETTINGS LOADING
            var DPI=(int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96);
            var scale = 96 / (float)DPI;
            PointF dpi = PointF.Empty;
            using (Graphics g = this.CreateGraphics())
            {
                dpi.X = g.DpiX;
                dpi.Y = g.DpiY;
            }
            // to do: just ask for resolution
            Debug.WriteLine(Properties.Settings.Default.ResX);
            if (Properties.Settings.Default.ResX == 0) {
                while (true)
                {
                    string x = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors X Resolution", "Please enter your monitors X Resolution", "1920", 0, 0);
                    try
                    {
                        int ix = Convert.ToInt32(x);
                        Properties.Settings.Default.ResX = ix;
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.Reload();
                        break;
                    } catch
                    {
                        Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number",Microsoft.VisualBasic.MsgBoxStyle.Information,"Invalid value");
                    }
                }
                while (true)
                {
                    string y = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors Y Resolution", "Please enter your monitors Y Resolution", "1080", 0, 0);
                    try
                    {
                        int iy = Convert.ToInt32(y);
                        Properties.Settings.Default.ResY = iy;
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.Reload();
                        break;
                    }
                    catch
                    {
                        Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
                    }
                }
            }
            var sfx = Properties.Settings.Default.ResX / 1920.0;
            var sfy = Properties.Settings.Default.ResY / 1080.0;
            int currentlyplaying = 0; // 0=nothing 1=title 2=menu 3=victory
            //
            //while (true)
            //{
            //    Debug.WriteLine(System.Windows.Forms.Cursor.Position.ToString());
            //    Debug.WriteLine(GetColorAt(new Point(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y)).ToString());
            //}

            // SETTINGS

            Debug.WriteLine("Loaded");
            Globals.mainmenu = Properties.Settings.Default.MainMenu;
            Globals.victory = Properties.Settings.Default.Victory;
            Globals.party = Properties.Settings.Default.Party;
            Debug.WriteLine(Globals.mainmenu);
            Globals.titlemenu = Properties.Settings.Default.TitleMenu;
            checkBox1.Checked = Properties.Settings.Default.Obscure;
            checkBox2.Checked = Properties.Settings.Default.Party;
            trackBar1.Value = Properties.Settings.Default.Volume;

            // APPLY SETTINGS
            MenuMusicFile.Text = Globals.mainmenu;
            TitleMenuFile.Text = Globals.titlemenu;
            VictoryMusicFile.Text = Globals.victory;
            //
            new Thread(() =>
            {
                wplayer.settings.setMode("loop", true);
                wplayer.settings.setMode("autoStart", false);
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    Thread.Sleep(500);
                    MethodInvoker mouse = delegate
                    {
                        label1.Text = System.Windows.Forms.Cursor.Position.ToString();
                        //label1.Text = System.Windows.Forms.Screen.PrimaryScreen;
                        //label2.Text = GetColorAt();
                        return;
                    };
                    this.Invoke(mouse);
                    if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                    {
                        bool focused = false;
                        uint pid;
                        GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                        {
                            if (p.Id == pid)
                            {
                                if (p.ProcessName == "FortniteClient-Win64-Shipping")
                                {
                                    focused = true;
                                    break;
                                }

                            }
                        }
                        try
                        {
                            Debug.WriteLine(wplayer.playState);
                            var c = GetColorAt(new Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy))));
                            if (Int32.Parse(c.R.ToString()) > 250 && Int32.Parse(c.G.ToString()) > 250 && Int32.Parse(c.B.ToString()) > 250)
                            {
                                c = GetColorAt(new Point(Convert.ToInt32(Math.Round(985 * sfx)), Convert.ToInt32(Math.Round(780 * sfy))));
                                if (Int32.Parse(c.R.ToString()) == 230 && Int32.Parse(c.G.ToString()) == 237 && Int32.Parse(c.B.ToString()) == 247)
                                {
                                    if ((currentlyplaying != 1))
                                    {
                                        currentlyplaying = 1;
                                        wplayer.controls.stop();
                                        wplayer.URL = Globals.titlemenu;
                                    }
                                    wplayer.controls.play();
                                }
                            }
                            else if (mainmenumusic(sfx, sfy) == true)
                            {
                                if ((currentlyplaying != 2))
                                {
                                    currentlyplaying = 2;
                                    wplayer.controls.stop();
                                    wplayer.URL = Globals.mainmenu;
                                }
                                wplayer.controls.play();
                            }
                            else if (victorymusic(sfx, sfy) == true)
                            {
                                if ((currentlyplaying != 3))
                                {
                                    currentlyplaying = 3;
                                    wplayer.controls.stop();
                                    wplayer.URL = Globals.victory;
                                }
                                wplayer.controls.play();
                            }
                            else
                            {
                                if (checkBox1.Checked == false)
                                {
                                    wplayer.controls.pause();
                                    //wplayer.URL = "";
                                }
                                else if (focused == true)
                                {
                                    wplayer.controls.pause();
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        wplayer.controls.pause();
                        wplayer.URL = "";
                    }
                }
            }).Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("open");
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Globals.titlemenu = openFileDialog1.FileName;
                TitleMenuFile.Text = openFileDialog1.FileName;
                Properties.Settings.Default.TitleMenu = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
                Debug.WriteLine("Hi");
            }
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
        //
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Globals.victory = openFileDialog1.FileName;
                VictoryMusicFile.Text = openFileDialog1.FileName;
                Properties.Settings.Default.Victory = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Party = checkBox2.Checked;
            Globals.party = checkBox2.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            while (true)
            {
                string x = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors X Resolution", "Please enter your monitors X Resolution", "1920", 0, 0);
                try
                {
                    int ix = Convert.ToInt32(x);
                    Properties.Settings.Default.ResX = ix;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                    break;
                }
                catch
                {
                    Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
                }
            }
            while (true)
            {
                string y = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors Y Resolution", "Please enter your monitors Y Resolution", "1080", 0, 0);
                try
                {
                    int iy = Convert.ToInt32(y);
                    Properties.Settings.Default.ResY = iy;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                    break;
                }
                catch
                {
                    Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
                }
            }
            Microsoft.VisualBasic.Interaction.MsgBox("Please Restart the program", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart required");

        }
    }
}