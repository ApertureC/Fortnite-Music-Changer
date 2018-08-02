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
            public const bool writelogs = false; // enable this is for want logs to be created. Else only the initialized message will be written.
            public static double sfx = 1;
            public static double sfy = 1;
        }
        private bool mainmenumusic(double sfx, double sfy)
        {
            writetolog("----- MAIN MENU -----");
            Color colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(428f * sfx)), Convert.ToInt32(Math.Round(548f * sfy))));
            Debug.WriteLine(colorAt);
            writetolog(colorAt.ToString());
            if (int.Parse(colorAt.R.ToString()) == 11 && int.Parse(colorAt.G.ToString()) == 19 && int.Parse(colorAt.B.ToString()) == 47)
            {
                return false;
            }
            colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(512f * sfx)), Convert.ToInt32(Math.Round(36f * sfy))));
            Debug.WriteLine("2");
            writetolog(colorAt.ToString());
            if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu2.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu2.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu2.B)
            {
                colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(909f * sfx)), Convert.ToInt32(Math.Round(1047f * sfy))));
                Debug.WriteLine("3");
                writetolog(colorAt.ToString());

                if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu3.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu3.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu3.B)
                {
                    colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(20f * sfx)), Convert.ToInt32(Math.Round(1043f * sfy))));
                    Debug.WriteLine("4");
                    writetolog(colorAt.ToString());
                    if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu4.R && int.Parse(colorAt.G.ToString()) >= Properties.Settings.Default.menu4.G && int.Parse(colorAt.B.ToString()) >= Properties.Settings.Default.menu4.B)
                    {
                        return true;
                    }
                    if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu4.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu4.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu4.B && Globals.party)
                    {
                        return true;
                    }
                }
            }
            colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
            if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu5.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu5.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu5.B)
            {
                colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu6.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu6.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu6.B)
                {
                    return true;
                }
            }
            return false;
        }
        private bool victorymusic(double sfx, double sfy)
        {
            writetolog("----- VICTORY -----");
            Color colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(911f * sfx)), Convert.ToInt32(Math.Round(251f * sfy))));
            writetolog(colorAt.ToString());
            if (int.Parse(colorAt.R.ToString()) == 242 && int.Parse(colorAt.G.ToString()) == 247 && int.Parse(colorAt.B.ToString()) == 252)
            {
                colorAt = GetColorAt(new Point(Convert.ToInt32(Math.Round(1087f * sfx)), Convert.ToInt32(Math.Round(271f * sfy))));
                writetolog(colorAt.ToString());

                if (int.Parse(colorAt.R.ToString()) == 255 && int.Parse(colorAt.G.ToString()) == 255 && int.Parse(colorAt.B.ToString()) == 255)
                {
                    return true;
                }
            }
            return false;
        }
        private void writetolog(string towrite)
        {
            if (Globals.writelogs == true)
            {
                System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "\\log.txt", System.IO.File.ReadAllText(System.Environment.CurrentDirectory + "\\log.txt") + towrite + System.Environment.NewLine);
            }
        }
        public Form1()
        {
            InitializeComponent();
            // SETTINGS LOADING
            //var DPI=(int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96);
            //var scale = 96 / (float)DPI;
            System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "\\log.txt", "Initizalized!" + System.Environment.NewLine); // to do create logs //
            PointF dpi = PointF.Empty;
            // to do: just ask for resolution
            Debug.WriteLine(Properties.Settings.Default.ResX);
            if (Properties.Settings.Default.ResX == 0)
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
            }
            var sfx = Properties.Settings.Default.ResX / 1920.0;
            var sfy = Properties.Settings.Default.ResY / 1080.0;
            //
            Globals.sfx = sfx;
            Globals.sfy = sfy;
            if (Properties.Settings.Default.title1 == Color.FromArgb(0, 0, 0, 0) || Properties.Settings.Default.menu2== Color.FromArgb(0, 0, 0, 0))
            {
                if (Properties.Settings.Default.title1 == Color.FromArgb(0, 0, 0, 0))
                {
                    Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the title screen (STW or BR selection) on fortnite" + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
                    var done = false;
                    while (true)
                    {
                        if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                        {
                            uint pid;
                            GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                            {
                                if (p.Id == pid)
                                {
                                    if (p.ProcessName == "FortniteClient-Win64-Shipping")
                                    {
                                        Properties.Settings.Default.title1 = GetColorAt(new Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy))));
                                        Properties.Settings.Default.title2 = GetColorAt(new Point(Convert.ToInt32(Math.Round(985 * sfx)), Convert.ToInt32(Math.Round(780 * sfy))));
                                        Properties.Settings.Default.Save();
                                        Properties.Settings.Default.Reload();
                                        done = true;
                                    }

                                }
                            }
                        }
                        if (done == true)
                        {
                            this.Activate();

                            Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
                            break;
                        }
                    }
                }
                if (Properties.Settings.Default.menu2 == Color.FromArgb(0, 0, 0, 0))
                {
                    Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the BR menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
                    var done = false;
                    while (true)
                    {
                        if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                        {
                            uint pid;
                            GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                            {
                                if (p.Id == pid)
                                {
                                    if (p.ProcessName == "FortniteClient-Win64-Shipping")
                                    {
                                        Properties.Settings.Default.menu2 = GetColorAt(new Point(Convert.ToInt32(Math.Round(512f * sfx)), Convert.ToInt32(Math.Round(36f * sfy))));
                                        Properties.Settings.Default.menu3 = GetColorAt(new Point(Convert.ToInt32(Math.Round(909f * sfx)), Convert.ToInt32(Math.Round(1047f * sfy))));
                                        Properties.Settings.Default.menu4 = GetColorAt(new Point(Convert.ToInt32(Math.Round(20f * sfx)), Convert.ToInt32(Math.Round(1043f * sfy))));
                                        Properties.Settings.Default.menu5 = GetColorAt(new Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                                        Properties.Settings.Default.menu6 = GetColorAt(new Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                                        Properties.Settings.Default.Save();
                                        Properties.Settings.Default.Reload();
                                        done = true;
                                    }

                                }
                            }
                        }
                        if (done == true)
                        {
                            this.Activate();
                            Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
                            break;
                        }
                    }
                }
                Microsoft.VisualBasic.Interaction.MsgBox("Please restart the application (no idea why but it doesn't work without a restart)", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart");
                Application.Exit();
            }
            writetolog("Scale factor X: " + sfx.ToString());
            writetolog("Scale factor Y: " + sfy.ToString());
            //
            writetolog("Resolution X " + Properties.Settings.Default.ResX);
            writetolog("Resolution Y " + Properties.Settings.Default.ResY);
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
            Globals.titlemenu = Properties.Settings.Default.TitleMenu;
            writetolog("Menu " + Globals.mainmenu);
            writetolog("Title " + Globals.titlemenu);
            writetolog("Victory " + Globals.victory);
            writetolog("Party " + Globals.party.ToString());
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
                    writetolog("----- NEW CYCLE -----");
                    MethodInvoker mouse = delegate
                    {
                        label1.Text = System.Windows.Forms.Cursor.Position.ToString();
                        //label1.Text = System.Windows.Forms.Screen.PrimaryScreen;
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
                                if (p.ProcessName == "FortniteClient-Win64-Shipping")
                                {
                                    focused = true;
                                    break;
                                }

                            }
                        }
                        writetolog("focus: " + focused.ToString());

                        try
                        {
                            writetolog("----- TITLE MENU -----");
                            Debug.WriteLine(wplayer.playState);
                            var c = GetColorAt(new Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy))));
                            writetolog(c.ToString());
                            if (Int32.Parse(c.R.ToString()) == Properties.Settings.Default.title1.R && Int32.Parse(c.G.ToString()) == Properties.Settings.Default.title1.G && Int32.Parse(c.B.ToString()) == Properties.Settings.Default.title1.B)
                            {
                                c = GetColorAt(new Point(Convert.ToInt32(Math.Round(985 * sfx)), Convert.ToInt32(Math.Round(780 * sfy))));
                                writetolog(c.ToString());
                                if (Int32.Parse(c.R.ToString()) == Properties.Settings.Default.title2.R && Int32.Parse(c.G.ToString()) == Properties.Settings.Default.title2.G && Int32.Parse(c.B.ToString()) == Properties.Settings.Default.title2.B)
                                {
                                    writetolog("Started playing title menu");

                                    if ((currentlyplaying != 1 || wplayer.URL != Globals.titlemenu))
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
                                writetolog("Started playing main menu");
                                if ((currentlyplaying != 2 || wplayer.URL != Globals.mainmenu))
                                {
                                    currentlyplaying = 2;
                                    wplayer.controls.stop();
                                    wplayer.URL = Globals.mainmenu;
                                }
                                wplayer.controls.play();
                            }
                            else if (victorymusic(sfx, sfy) == true)
                            {
                                writetolog("Started playing victory");
                                if ((currentlyplaying != 3 || wplayer.URL != Globals.victory))
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
                                    Debug.WriteLine("STOPHERE");
                                    wplayer.controls.pause();
                                    //wplayer.URL = "";
                                }
                                else if (focused == true)
                                {
                                    Debug.WriteLine("STOPHERE2");
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
                        writetolog("Fortnite not open");
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            var sfx = Globals.sfx;
            var sfy = Globals.sfy;
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the title screen (STW or BR selection) on fortnite" + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            var done = false;
            while (true)
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                    {
                        if (p.Id == pid)
                        {
                            if (p.ProcessName == "FortniteClient-Win64-Shipping")
                            {
                                Properties.Settings.Default.title1 = GetColorAt(new Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy))));
                                Properties.Settings.Default.title2 = GetColorAt(new Point(Convert.ToInt32(Math.Round(985 * sfx)), Convert.ToInt32(Math.Round(780 * sfy))));
                                Properties.Settings.Default.Save();
                                Properties.Settings.Default.Reload();
                                done = true;
                            }

                        }
                    }
                }
                if (done == true)
                {
                    this.Activate();

                    Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
                    break;
                }
            }
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the BR menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            done = false;
            while (true)
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                    {
                        if (p.Id == pid)
                        {
                            if (p.ProcessName == "FortniteClient-Win64-Shipping")
                            {
                                Properties.Settings.Default.menu2 = GetColorAt(new Point(Convert.ToInt32(Math.Round(512f * sfx)), Convert.ToInt32(Math.Round(36f * sfy))));
                                Properties.Settings.Default.menu3 = GetColorAt(new Point(Convert.ToInt32(Math.Round(909f * sfx)), Convert.ToInt32(Math.Round(1047f * sfy))));
                                Properties.Settings.Default.menu4 = GetColorAt(new Point(Convert.ToInt32(Math.Round(20f * sfx)), Convert.ToInt32(Math.Round(1043f * sfy))));
                                Properties.Settings.Default.menu5 = GetColorAt(new Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                                Properties.Settings.Default.menu6 = GetColorAt(new Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                                Properties.Settings.Default.Save();
                                Properties.Settings.Default.Reload();
                                done = true;
                            }

                        }
                    }
                }
                if (done == true)
                {
                    this.Activate();
                    Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
                    break;
                }
            }
            Microsoft.VisualBasic.Interaction.MsgBox("Please restart the application (no idea why but it doesn't work without a restart)", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart");
            Application.Exit();
        }
    }
}