using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Security.Principal;
// This release's updates:
// Fixed startup
// Cleaned up install
// Issue checker
// Message box telling people that it not responding is fine
namespace Fortnite_Music
{
    public partial class Form1 : Form
    {
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        public static class Globals
        {
            public static string titlemenu = "";
            public static string mainmenu = "";
            public static string victory = "";
            public static bool stretched = false;
            public static bool writelogs = false; // enable this is for want logs to be created. Else only the initialized message will be written.
            public static double sfx = 1;
            public static double sfy = 1;
            public static bool releasebitmap = false;
            public static bool optimize = Properties.Settings.Default.optimize;
            public static bool InGame = false;
            public static int resX = Properties.Settings.Default.ResX;
            public static int resY = Properties.Settings.Default.ResY;
        }

        // P/Invoke declarations
        //[DllImport("user32.dll")]
        //static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        //[DllImport("gdi32.dll")]
        //static extern IntPtr DeleteDC(IntPtr hDc);
        //[DllImport("gdi32.dll")]
        //static extern IntPtr DeleteObject(IntPtr hDc);
        //[DllImport("gdi32.dll")]
        //static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        //[DllImport("gdi32.dll")]
        //static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        //[DllImport("gdi32.dll")]
        //static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        //[DllImport("user32.dll")]
        //public static extern IntPtr GetWindowDC(IntPtr ptr);

        [DllImport("user32.dll")]
        static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        //static Adapter adapter = new Factory1().GetAdapter(0);
        //static SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device(adapter)
        /*private DesktopDuplicator desktopDuplication;
		private int test;
		private bool togglerunning;
		private System.Drawing.Bitmap GetBitmap(int resX, int resY)
		{
			DesktopFrame frame = new DesktopFrame();
			if (!togglerunning)
			{
				short keyState = GetAsyncKeyState(0x48);
				bool prntScrnIsPressed = ((keyState >> 15) & 0x0001) == 0x0001;
				Debug.WriteLine(prntScrnIsPressed);
				if (!prntScrnIsPressed)
				{
					frame = null;
					while (true)
					{
						Debug.WriteLine("!!!!" + desktopDuplication);
						Debug.WriteLine(desktopDuplication == null);
						if (desktopDuplication == null)
						{
							test++;
							Debug.WriteLine("Attempting to create!!");
							desktopDuplication = new DesktopDuplicator(0, resX, resY);
							Thread.Sleep(50);
						}
						Debug.WriteLine("Created: " + test);
						try
						{
							frame = desktopDuplication.GetLatestFrame();
						}
						catch
						{
							frame = null;
						}
						Debug.WriteLine("FRAME IS:" + frame);
						Debug.WriteLine(frame == null);
						if (frame == null)
						{
							Debug.WriteLine("Null");
							desktopDuplication.Dispose();
							Debug.WriteLine("Successfully Disposed");
							desktopDuplication = null;
							break;
						}
						else
						{
							//frame.Save(@"C:\Users\Aperture\Documents\idiot.bmp");
							break;
						}
					}
				}
			}
			else
			{
				if (desktopDuplication == null)
				{
					desktopDuplication.Dispose();
					desktopDuplication = null;
				}
			}
			if (frame.DesktopImage!=null)
			{
				return frame.DesktopImage;
			} else
			{
				return null;
			}
		}
		*/
        private DialogResult viewImage(string botText, Bitmap bitmap)
        {
            ImageViewer m = new ImageViewer();
            m.label1.Text = botText;
            m.pictureBox1.Image = bitmap;
            DialogResult thingy = m.ShowDialog();
            return thingy;
        }
        private bool mainMenuMusic(double sfx, double sfy)
        {
            WriteToLog("----- MAIN MENU -----", false);
            System.Drawing.Color colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(428f * sfx)), Convert.ToInt32(Math.Round(548f * sfy))));
            WriteToLog("menu - purchase check" + colorAt.ToString(), false);
            if (int.Parse(colorAt.R.ToString()) == 11 && int.Parse(colorAt.G.ToString()) == 19 && int.Parse(colorAt.B.ToString()) == 47)
            {
                //return false;
            }
            colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(512f * sfx)), Convert.ToInt32(Math.Round(36f * sfy))));
            Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu2.R + " " + Properties.Settings.Default.menu2.G + " " + Properties.Settings.Default.menu2.B);

            WriteToLog("menu2" + colorAt.ToString() + " FOR: " + Properties.Settings.Default.menu2.ToString(), false);
            if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu2.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu2.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu2.B)
            {
                colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(909f * sfx)), Convert.ToInt32(Math.Round(1047f * sfy))));
                WriteToLog("menu3" + colorAt.ToString() + " FOR: " + Properties.Settings.Default.menu3.ToString(), false);
                Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu3.R + " " + Properties.Settings.Default.menu3.G + " " + Properties.Settings.Default.menu3.B);
                if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu3.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu3.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu3.B)
                {
                    if (Properties.Settings.Default.PlayInParty == false)
                    {
                        if (Globals.stretched == false)
                        {
                            colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(20f * sfx)), Convert.ToInt32(Math.Round(1043f * sfy))));
                            WriteToLog("menu4" + colorAt.ToString() + " FOR: " + Properties.Settings.Default.menu4.ToString(), false);
                            Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu4.R + " " + Properties.Settings.Default.menu4.G + " " + Properties.Settings.Default.menu4.B);
                            if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu4.R && int.Parse(colorAt.G.ToString()) >= Properties.Settings.Default.menu4.G && int.Parse(colorAt.B.ToString()) >= Properties.Settings.Default.menu4.B)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(13f * (Globals.sfx / 1440.0))), Convert.ToInt32(Math.Round(1055f * (Globals.sfy / 1080.0)))));
                            WriteToLog("menu4" + colorAt.ToString() + " FOR: " + Properties.Settings.Default.menu4.ToString(), false);
                            Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu4.R + " " + Properties.Settings.Default.menu4.G + " " + Properties.Settings.Default.menu4.B);
                            if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu4.R && int.Parse(colorAt.G.ToString()) >= Properties.Settings.Default.menu4.G && int.Parse(colorAt.B.ToString()) >= Properties.Settings.Default.menu4.B)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            WriteToLog("------ SETTINGS -------", false);
            Debug.WriteLine("----------------------------------- SETTINGS");
            if (Globals.stretched == false)
            {
                colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu5.R + " " + Properties.Settings.Default.menu5.G + " " + Properties.Settings.Default.menu5.B);
                WriteToLog("menu5" + colorAt.ToString() + " FOR: " + Properties.Settings.Default.menu5.ToString(), false);
                if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu5.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu5.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu5.B)
                {
                    colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                    WriteToLog("menu6" + colorAt.ToString() + " FOR: " + Properties.Settings.Default.menu6.ToString(), false);
                    Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu6.R + " " + Properties.Settings.Default.menu6.G + " " + Properties.Settings.Default.menu6.B);
                    if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu6.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu6.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu6.B)
                    {
                        return true;
                    }
                }
            }
            else
            {
                colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1422f * (Globals.resX / 1440.0))), Convert.ToInt32(Math.Round(8f * (Globals.resY / 1080.0)))));
                if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu5.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu5.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu5.B)
                {
                    colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1370f * (Globals.resX / 1440.0))), Convert.ToInt32(Math.Round(8f * (Globals.resY / 1080.0)))));
                    WriteToLog("menu6" + colorAt.ToString() + " FOR: " + Properties.Settings.Default.menu6.ToString(), false);
                    Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu6.R + " " + Properties.Settings.Default.menu6.G + " " + Properties.Settings.Default.menu6.B);
                    if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu6.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu6.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu6.B)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Victory Positions 16:9
        // 911, 251
        // 1087, 271
        private bool victoryMusic(double sfx, double sfy)
        {
            WriteToLog("----- VICTORY -----", false);
            if (Properties.Settings.Default.victory1.A != 0 && Properties.Settings.Default.victory2.A != 0)
            {
                if (Properties.Settings.Default.stretched == false)
                {
                    System.Drawing.Color colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(911f * sfx)), Convert.ToInt32(Math.Round(251f * sfy))));
                    WriteToLog(colorAt.ToString(), false);
                    if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.victory1.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.victory1.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.victory1.B)
                    {
                        colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1087f * sfx)), Convert.ToInt32(Math.Round(271f * sfy))));
                        WriteToLog(colorAt.ToString(), false);

                        if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.victory2.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.victory2.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.victory2.B)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    System.Drawing.Color colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(680f * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(347f * (Properties.Settings.Default.ResY / 1080.0)))));
                    WriteToLog(colorAt.ToString(), false);
                    Debug.WriteLine("_----------- VICTORY ---------");
                    Debug.WriteLine(colorAt + "          " + Properties.Settings.Default.victory1);
                    if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.victory1.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.victory1.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.victory1.B)
                    {
                        colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(805f * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(240f * (Properties.Settings.Default.ResY / 1080.0)))));
                        WriteToLog(colorAt.ToString(), false);
                        Debug.WriteLine(colorAt + "          " + Properties.Settings.Default.victory2);
                        if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.victory2.R && int.Parse(colorAt.G.ToString()) >= Properties.Settings.Default.victory2.G && int.Parse(colorAt.B.ToString()) >= Properties.Settings.Default.victory2.B)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void WriteToLog(string towrite, bool overrde)
        {
            if (Globals.writelogs == true)
            {
                while (true)
                {
                    try
                    {
                        Debug.WriteLine(File.Exists(System.Environment.CurrentDirectory + "\\log.txt"));
                        if (!File.Exists(System.Environment.CurrentDirectory + "\\log.txt"))
                        {
                            File.Create(System.Environment.CurrentDirectory + "\\log.txt").Dispose();
                        }
                        System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "\\log.txt", System.IO.File.ReadAllText(System.Environment.CurrentDirectory + "\\log.txt") + towrite + System.Environment.NewLine);
                        break;
                    }
                    catch
                    {

                    }
                }
            }
        }
        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }
        private void SetStartup()
        {
            /*RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (launchOnStartupToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.Startup = true;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
                rk.SetValue("Fortnite Music", Application.ExecutablePath);
            }
            else
            {
                Properties.Settings.Default.Startup = false;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
                rk.DeleteValue("Fortnite Music", false);
            }*/
            //create shortcut to file in startup
            if (IsAdministrator())
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk"))
                {
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk");
                }
                else
                {

                    IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
                    IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
                        Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk") as IWshRuntimeLibrary.IWshShortcut;
                    shortcut.Arguments = "";
                    shortcut.TargetPath = Environment.CurrentDirectory + @"\Fortnite Music.exe";
                    shortcut.WindowStyle = 1;
                    shortcut.Description = "Fortnite Music Changer";
                    shortcut.WorkingDirectory = Environment.CurrentDirectory + @"\";
                    //shortcut.IconLocation = "specify icon location";
                    shortcut.Save();
                }
            }
            else
            {
                launchOnStartupToolStripMenuItem.Checked = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk");
                Microsoft.VisualBasic.Interaction.MsgBox("Setting this to run on startup requires admin privileges (Close the program, right click, Run as administrator)", Microsoft.VisualBasic.MsgBoxStyle.Exclamation, "Administrator privileges required");

            }

        }
        private void HandleHotkey()
        {
            Debug.WriteLine("W pressed");
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);
        public static byte GetVirtualKeyCode(Keys key)
        {
            int value = (int)key;
            return (byte)(value & 0xFF);
        }
        private int wkeycode = GetVirtualKeyCode(Keys.W);
        static int GCD(int a, int b)
        {
            return b == 0 ? Math.Abs(a) : GCD(b, a % b);
        }
        private void setup()
        {
            Microsoft.VisualBasic.Interaction.MsgBox("The program will say it isn't responding while waiting for you to click onto fortnite, this is normal, ignore it.", Microsoft.VisualBasic.MsgBoxStyle.Exclamation, "Not responding is fine");
            int waittime = 5;
            Debug.WriteLine(Properties.Settings.Default.title1);
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the title screen (STW or BR selection) on fortnite" + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            var done = false;
            while (true)
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                {
                    //System.Drawing.Bitmap BitMap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp"));
                    //System.Drawing.Bitmap BitMap = GetBitmap();
                    //BitMap.Save(@"C:\Users\Aperture\source\repos\Fortnite-Music-Changer\Fortnite Music\bin\Debug\SaveImage2.bmp");
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * Globals.sfx)), Convert.ToInt32(Math.Round(28 * Globals.sfy)))).A != 0)
                    {
                        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                        {
                            if (p.Id == pid)
                            {
                                Debug.WriteLine("CURRENTLY FOREGROUND: " + p.ProcessName);
                                if (p.ProcessName == "FortniteClient-Win64-Shipping")
                                {
                                    Thread.Sleep(waittime * 1000);
                                    Debug.WriteLine("Passed2");
                                    using (Bitmap bitmap = new Bitmap(Globals.resX, Globals.resY))
                                    {
                                        using (Graphics g = Graphics.FromImage(bitmap))
                                        {
                                            Rectangle bounds = Screen.GetBounds(Point.Empty);
                                            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                            var result = viewImage("Is this the title menu?", bitmap);
                                            if (result == DialogResult.OK)
                                            {
                                                Properties.Settings.Default.title1 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * Globals.sfx)), Convert.ToInt32(Math.Round(28 * Globals.sfy))), bitmap);
                                                Properties.Settings.Default.title2 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(985 * Globals.sfx)), Convert.ToInt32(Math.Round(780 * Globals.sfy))), bitmap);
                                                Properties.Settings.Default.Save();
                                                Properties.Settings.Default.Reload();
                                                if (GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * Globals.sfx)), Convert.ToInt32(Math.Round(28 * Globals.sfy)))).A != 0)
                                                {
                                                    done = true;
                                                }
                                            }
                                            if (result == DialogResult.Cancel)
                                            {
                                                Environment.Exit(0);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    if (done == true)
                    {
                        this.Activate();

                        //Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information);
                        break;
                    }
                }
            }
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the Battle Royale menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            done = false;
            while (true)
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * Globals.sfx)), Convert.ToInt32(Math.Round(28 * Globals.sfy)))).A != 0)
                    {

                        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                        {
                            if (p.Id == pid)
                            {
                                if (p.ProcessName == "FortniteClient-Win64-Shipping")
                                {
                                    Thread.Sleep(waittime * 1000);
                                    using (Bitmap bitmap = new Bitmap(Globals.resX, Globals.resY))
                                    {
                                        using (Graphics g = Graphics.FromImage(bitmap))
                                        {
                                            Rectangle bounds = Screen.GetBounds(Point.Empty);
                                            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                            var result = viewImage("Is this the Battle Royale menu?", bitmap);
                                            if (result == DialogResult.OK)
                                            {
                                                Properties.Settings.Default.menu2 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(512f * Globals.sfx)), Convert.ToInt32(Math.Round(36f * Globals.sfy))), bitmap);
                                                Properties.Settings.Default.menu3 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(909f * Globals.sfx)), Convert.ToInt32(Math.Round(1047f * Globals.sfy))), bitmap);
                                                if (Globals.stretched == false)
                                                {
                                                    Properties.Settings.Default.menu4 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(20f * Globals.sfx)), Convert.ToInt32(Math.Round(1043f * Globals.sfy))), bitmap);
                                                }
                                                else
                                                {
                                                    Properties.Settings.Default.menu4 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(13f * (Globals.sfx / 1440.0))), Convert.ToInt32(Math.Round(1055f * (Globals.sfy / 1080.0)))), bitmap);
                                                }
                                                Properties.Settings.Default.gamemenufn = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(30 * Globals.sfx)), Convert.ToInt32(Math.Round(16 * Globals.sfy))), bitmap);
                                                Properties.Settings.Default.Save();
                                                Properties.Settings.Default.Reload();
                                                if (GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(512f * Globals.sfx)), Convert.ToInt32(Math.Round(36f * Globals.sfy))), bitmap).A != 0)
                                                {
                                                    done = true;
                                                }
                                            }
                                            if (result == DialogResult.Cancel)
                                            {
                                                Environment.Exit(0);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                if (done == true)
                {
                    this.Activate();
                    //Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");

                    break;
                }
            }
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the Settings menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            done = false;
            while (true)
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                {
                    Debug.WriteLine("STUFF1");
                    //System.Drawing.Bitmap BitMap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp"));
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * Globals.sfx)), Convert.ToInt32(Math.Round(28 * Globals.sfy)))).A != 0)
                    {
                        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                        {
                            if (p.Id == pid)
                            {
                                if (p.ProcessName == "FortniteClient-Win64-Shipping")
                                {
                                    Thread.Sleep(waittime * 1000);
                                    using (Bitmap bitmap = new Bitmap(Globals.resX, Globals.resY))
                                    {
                                        using (Graphics g = Graphics.FromImage(bitmap))
                                        {
                                            Rectangle bounds = Screen.GetBounds(Point.Empty);
                                            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                            var result = viewImage("Is this the Settings menu?", bitmap);
                                            if (result == DialogResult.OK)
                                            {
                                                Debug.WriteLine("STUFF2");
                                                Debug.WriteLine(GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1897f * Globals.sfx)), Convert.ToInt32(Math.Round(10f * Globals.sfy)))));
                                                if (Globals.stretched == false)
                                                {
                                                    Properties.Settings.Default.menu5 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(1897f * Globals.sfx)), Convert.ToInt32(Math.Round(10f * Globals.sfy))), bitmap);
                                                    Properties.Settings.Default.menu6 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(1825f * Globals.sfx)), Convert.ToInt32(Math.Round(10f * Globals.sfy))), bitmap);
                                                }
                                                else
                                                {
                                                    Properties.Settings.Default.menu5 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(1422f * (Globals.resX / 1440.0))), Convert.ToInt32(Math.Round(8f * (Globals.resY / 1080)))), bitmap);
                                                    Properties.Settings.Default.menu6 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(1370f * (Globals.resX / 1440.0))), Convert.ToInt32(Math.Round(8f * (Globals.resY / 1080)))), bitmap);
                                                }
                                                Properties.Settings.Default.gamesettingsfn = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(30 * Globals.sfx)), Convert.ToInt32(Math.Round(16 * Globals.sfy))), bitmap);
                                                Properties.Settings.Default.Save();
                                                Properties.Settings.Default.Reload();
                                                if (GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(30 * Globals.sfx)), Convert.ToInt32(Math.Round(16 * Globals.sfy))), bitmap).A != 0)
                                                {
                                                    done = true;
                                                }
                                                if (result == DialogResult.Cancel)
                                                {
                                                    Environment.Exit(0);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                if (done == true)
                {
                    this.Activate();
                    //Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
                    break;
                }
            }
            Microsoft.VisualBasic.Interaction.MsgBox("(OPTIONAL) After the restart, Go into 50v50 and win a game, press 'Victory Setup' after about 5 seconds of the victory royale screen coming up.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Victory Music");
            Microsoft.VisualBasic.Interaction.MsgBox("Please restart the application (no idea why but it doesn't work without a restart)", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart");
            Application.Exit();
        }
        public Form1()
        {
            Globals.stretched = false;

            Debug.WriteLine(GetVirtualKeyCode(Keys.W));
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Globals.writelogs = Properties.Settings.Default.WriteLogs;
            //Adapter adapter = new Factory1().GetAdapter(0);
            //var output = adapter.GetOutput(0);
            //var output1 = output.QueryInterface<Output1>();
            //SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device(adapter);
            InitializeComponent();
            Thread.Sleep(250);
            //
            //frame.
            //ibf=new 
            //dd.Capture();
            string tag = "2.3";
            // minimized
            if (Properties.Settings.Default.StartMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
                startMinimizedToolStripMenuItem.Checked = true;
            }
            // Auto update
            string html;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/ApertureC/Fortnite-Music-Changer/releases/latest?UserAgent=hi");
            request.ContentType = "application/json";
            request.UserAgent = "e";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(html);
            if (data.tag_name != tag)
            {
                if (Microsoft.VisualBasic.Interaction.MsgBox("An update is available (" + data.name + "), would you like to view the latest version?", Microsoft.VisualBasic.MsgBoxStyle.YesNo, "Update") == Microsoft.VisualBasic.MsgBoxResult.Yes)
                {
                    Debug.WriteLine("going");
                    System.Diagnostics.Process.Start("https://github.com/ApertureC/Fortnite-Music-Changer/releases/latest");
                }
            }
            WriteToLog("Update availaility done", true);
            Debug.WriteLine(html);
            // SETTINGS LOADING
            //var DPI=(int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96);
            //var scale = 96 / (float)DPI;
            System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "\\log.txt", "Initizalized!" + System.Environment.NewLine); // to do create logs //
                                                                                                                                          // to do: just ask for resolution
            Debug.WriteLine(Properties.Settings.Default.ResX);
            if (Properties.Settings.Default.ResX == 0)
            {
                while (true)
                {
                    string x = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors OR FULLSCREEN (STRETCHED) X Resolution", "Please enter your monitors OR FULLSCREEN (STRETCHED) X Resolution", "", 0, 0);

                    try
                    {
                        int ix = Convert.ToInt32(x);
                        Properties.Settings.Default.ResX = ix;
                        Globals.resX = ix;
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
                    string y = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors OR FULLSCREEN (STRETCHED) Y Resolution", "Please enter your monitors OR FULLSCREEN (STRETCHED) Y Resolution", "", 0, 0);
                    try
                    {
                        int iy = Convert.ToInt32(y);
                        Properties.Settings.Default.ResY = iy;
                        Globals.resY = iy;
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
            WriteToLog("SetRes", true);
            var sfx = Properties.Settings.Default.ResX / 1920.0;
            var sfy = Properties.Settings.Default.ResY / 1080.0;
            //
            Globals.sfx = sfx;
            Globals.sfy = sfy;
            // AUTO STRETCHED
            Debug.WriteLine(Globals.resX.ToString() + Globals.resY.ToString());
            int gcd = GCD(Globals.resX, Globals.resY);
            WriteToLog("Stuff", true);
            Debug.WriteLine(string.Format("{0}:{1}", Globals.resX / gcd, Globals.resY / gcd));
            if (Globals.resX / gcd == 4 && Globals.resY / gcd == 3)
            {
                Globals.stretched = true;
                Properties.Settings.Default.stretched = true;
            }
            if (Properties.Settings.Default.title1 == System.Drawing.Color.FromArgb(0, 0, 0, 0) || Properties.Settings.Default.menu2 == System.Drawing.Color.FromArgb(0, 0, 0, 0) || Properties.Settings.Default.menu5 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
            {
                setup();
            }
            WriteToLog("Setup done :D", true);
            WriteToLog("Scale factor X: " + sfx.ToString(), false);
            WriteToLog("Scale factor Y: " + sfy.ToString(), false);
            //
            WriteToLog("Resolution X " + Properties.Settings.Default.ResX, false);
            WriteToLog("Resolution Y " + Properties.Settings.Default.ResY, false);
            int currentlyplaying = 0; // 0=nothing 1=title 2=menu 3=victory
                                      //
                                      //while (true)
                                      //{
                                      //    Debug.WriteLine(System.Windows.Forms.Cursor.Position.ToString());
                                      //    Debug.WriteLine(GetColorAt(BitMap,new Point(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y)).ToString());
                                      //}
                                      // SETTINGS

            Debug.WriteLine("Loaded");
            Globals.mainmenu = Properties.Settings.Default.MainMenu;
            Globals.victory = Properties.Settings.Default.Victory;
            Globals.stretched = Properties.Settings.Default.stretched;
            Globals.titlemenu = Properties.Settings.Default.TitleMenu;
            if (Properties.Settings.Default.TitleMenu == "" && Properties.Settings.Default.MainMenu == "" && Properties.Settings.Default.Victory == "")
            {
                Microsoft.VisualBasic.Interaction.MsgBox("The program has no loaded mp3s, mp3s need to be loaded to play music! Hit browse to load mp3s", Microsoft.VisualBasic.MsgBoxStyle.Information, "No MP3s");
            }
            Globals.optimize = Properties.Settings.Default.optimize;
            WriteToLog("Menu " + Globals.mainmenu, false);
            WriteToLog("Title " + Globals.titlemenu, false);
            WriteToLog("Victory " + Globals.victory, false);
            checkBox1.Checked = Properties.Settings.Default.Obscure;
            launchOnStartupToolStripMenuItem.Checked = Properties.Settings.Default.Startup;

            trackBar1.Value = Properties.Settings.Default.Volume;
            VolumeNum.Text = Properties.Settings.Default.Volume.ToString();
            wplayer.settings.volume = Properties.Settings.Default.Volume;
            // APPLY SETTINGS
            MenuMusicFile.Text = Globals.mainmenu;
            TitleMenuFile.Text = Globals.titlemenu;
            VictoryMusicFile.Text = Globals.victory;
            launchOnStartupToolStripMenuItem.Checked = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk");
            // Preload
            Thread t = new Thread(() =>
            {
                wplayer.settings.setMode("loop", true);
                wplayer.settings.setMode("autoStart", false);

                List<string> list = new List<string>();
                list.Add(Globals.titlemenu);
                list.Add(Globals.mainmenu);
                list.Add(Globals.victory);
                for (int i = 0; i < 3; i++)
                {
                    wplayer.URL = list[i];
                    if (list[i] != "")
                    {
                        wplayer.controls.play();
                        while (true)
                        {
                            var b = false;
                            try
                            {
                                if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                                {
                                    wplayer.controls.stop();
                                    b = true;
                                }
                            }
                            catch
                            {

                            }
                            if (b == true)
                            {
                                break;
                            }
                        }
                    }
                }

                while (true)
                {
                    Debug.WriteLine("Starting Loop");
                    Thread.Sleep(500);//250

                    //
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    //
                    WriteToLog("----- NEW CYCLE -----", false);
                    MethodInvoker mouse = delegate
                    {
                        label1.Text = Cursor.Position.ToString();
                        //using (System.Drawing.Bitmap BitMap = GetBitmap(adapter, device))
                        //{

                        //label1.Text = System.Windows.Forms.Cursor.Position.ToString() + " " + GetColorAt(BitMap, new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y)).ToString();
                        //label1.Text = System.Windows.Forms.Screen.PrimaryScreen;
                        //label2.Text = GetColorAt(BitMap,);
                        return;
                        //}
                    };
                    //this.Invoke(mouse);
                    /*short keyState = GetAsyncKeyState(0x48);
                    bool prntScrnIsPressed = ((keyState >> 15) & 0x0001) == 0x0001;
                    if (prntScrnIsPressed)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            // close the form on the forms thread
                            this.Close();
                        });

                        Debug.WriteLine("Awaiting release of button!!!");
                        while (true)
                        {
                            keyState = GetAsyncKeyState(0x48);
                            prntScrnIsPressed = ((keyState >> 15) & 0x0001) == 0x0001;
                            if (!prntScrnIsPressed)
                            {
                                Debug.WriteLine("released");
                                break;
                            }
                        }

                    } 
                    keyState = GetAsyncKeyState(0x4A);
                    prntScrnIsPressed = ((keyState >> 15) & 0x0001) == 0x0001;
                    if (prntScrnIsPressed)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            // close the form on the forms thread
                            this.Close();
                        });

                    }
                    */
                    WriteToLog("Fortnite check open", false);
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
                        WriteToLog("focus: " + focused.ToString(), false);
                        Debug.WriteLine("Changing BitMap");
                        if (focused == true)
                        {
                            if (Globals.releasebitmap == false)
                            {
                                try
                                {

                                    //System.Drawing.Bitmap BitMap = new System.Drawing.Bitmap(1920, 1080);
                                    // REPLACE ERRORS WITH THIS Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp")
                                    if (GetColorAt(new System.Drawing.Point(0, 0)).A != 0)
                                    {
                                        Debug.WriteLine("Passed");
                                        var c = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy))));
                                        WriteToLog(c.ToString(), false);
                                        Debug.WriteLine(c);
                                        Debug.WriteLine(c + Properties.Settings.Default.title1.R.ToString() + Properties.Settings.Default.title1.G.ToString() + Properties.Settings.Default.title1.B.ToString());
                                        if (Int32.Parse(c.R.ToString()) == Properties.Settings.Default.title1.R && Int32.Parse(c.G.ToString()) == Properties.Settings.Default.title1.G && Int32.Parse(c.B.ToString()) == Properties.Settings.Default.title1.B)
                                        {
                                            c = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(985 * sfx)), Convert.ToInt32(Math.Round(780 * sfy))));
                                            WriteToLog(c.ToString(), false);
                                            Debug.WriteLine(c);
                                            if (Int32.Parse(c.R.ToString()) == Properties.Settings.Default.title2.R && Int32.Parse(c.G.ToString()) == Properties.Settings.Default.title2.G && Int32.Parse(c.B.ToString()) == Properties.Settings.Default.title2.B)
                                            {

                                                WriteToLog("Started playing title menu", false);

                                                if ((currentlyplaying != 1 || wplayer.URL != Globals.titlemenu))
                                                {
                                                    currentlyplaying = 1;
                                                    wplayer.controls.pause();
                                                    wplayer.URL = Globals.titlemenu;
                                                }
                                                wplayer.controls.play();
                                            }
                                        }
                                        else if (mainMenuMusic(sfx, sfy) == true) // to do stop thread on menu setup + changing resolution
                                        {
                                            WriteToLog("Started playing main menu", false);
                                            if ((currentlyplaying != 2 || wplayer.URL != Globals.mainmenu))
                                            {
                                                currentlyplaying = 2;
                                                wplayer.controls.pause();
                                                wplayer.URL = Globals.mainmenu;
                                            }
                                            wplayer.controls.play();
                                        }
                                        else if (victoryMusic(sfx, sfy) == true)
                                        {
                                            WriteToLog("Started playing victory", false);
                                            if ((currentlyplaying != 3 || wplayer.URL != Globals.victory))
                                            {
                                                currentlyplaying = 3;
                                                wplayer.controls.pause();
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
                                }
                                catch
                                {
                                    Debug.WriteLine("Hi");
                                }
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Pausing1");
                            if (checkBox1.Checked == false)
                            {
                                try
                                {
                                    wplayer.controls.pause();
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    else
                    {
                        WriteToLog("Fortnite not open", false);
                        wplayer.controls.pause();
                        wplayer.URL = "";
                    }
                }
            });
            t.IsBackground = true;
            t.Name = "MainThread";
            t.Start();
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
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Rect
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        }

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hdc, uint nFlags);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr handle, out System.Drawing.Rectangle rect);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
        //public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        public static System.Drawing.Color GetColorAt(System.Drawing.Point location)
        {
            Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            ////
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(Globals.resX, Globals.resY))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                //bitmap.Save("test.bmp", ImageFormat.Bmp);
            }
            //
            while (true)
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
                if (screenPixel.GetPixel(0, 0).A != 0)
                {
                    return screenPixel.GetPixel(0, 0);
                }
            }
        }
        public static System.Drawing.Color GetColorAtProvided(System.Drawing.Point location, Bitmap bitmap)
        {
            if (bitmap.GetPixel(location.X, location.Y).A != 0)
            {
                return bitmap.GetPixel(location.X, location.Y);
            }
            else
            {
                return new Color();
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


        private void button4_Click(object sender, EventArgs e)
        {
            while (true)
            {
                string x = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors OR FULLSCREEN (STRETCHED) X Resolution", "Please enter your monitors OR FULLSCREEN (STRETCHED) X Resolution", "", 0, 0);
                try
                {
                    int ix = Convert.ToInt32(x);
                    Properties.Settings.Default.ResX = ix;
                    Globals.resX = ix;
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
                string y = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors OR FULLSCREEN (STRETCHED) Y Resolution", "Please enter your monitors OR FULLSCREEN (STRETCHED) Y Resolution", "", 0, 0);
                try
                {
                    int iy = Convert.ToInt32(y);
                    Properties.Settings.Default.ResY = iy;
                    Globals.resY = iy;
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
            setup();
        }

        private void licensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.VisualBasic.Interaction.MsgBox(@"MIT License

Copyright (c) 2018 ApertureC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the 'Software'), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

            The above copyright notice and this permission notice shall be included in all
            copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Original source code here: https://github.com/ApertureC/Fortnite-Music-Changer", Microsoft.VisualBasic.MsgBoxStyle.Information, "Fortnite Music Changer");
            Microsoft.VisualBasic.Interaction.MsgBox(@"The MIT License (MIT)

Copyright (c) 2007 James Newton-King

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the 'Software'), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and / or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

            The above copyright notice and this permission notice shall be included in all
            copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Newtonsoft.Json");
        }
        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ApertureC/Fortnite-Music-Changer");
        }

        private void redditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://reddit.com/u/ApertureCoder");
        }


        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PlayInParty = checkBox4.Checked;
            Globals.optimize = checkBox4.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
        // Victory Positions 16:9
        // 911, 251
        // 1087, 271

        private void button5_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(Clipboard.ContainsImage());
            //bitmap.Save(@"C:\Users\Aperture\Documents\oof.bmp");
            Microsoft.VisualBasic.Interaction.MsgBox("Press OK and immediately click onto Fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Victory Setup");
            Thread.Sleep(3000);
            while (true)
            {
                var b = false;
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * Globals.sfx)), Convert.ToInt32(Math.Round(28 * Globals.sfy)))).A != 0)
                    {
                        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                        {
                            if (p.Id == pid)
                            {
                                if (p.ProcessName == "FortniteClient-Win64-Shipping")
                                {
                                    using (Bitmap bitmap = new Bitmap(Globals.resX, Globals.resY))
                                    {
                                        using (Graphics g = Graphics.FromImage(bitmap))
                                        {
                                            Rectangle bounds = Screen.GetBounds(Point.Empty);
                                            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                            var result = viewImage("Is this the victory image you wish to use menu?", bitmap);
                                            if (result == DialogResult.OK)
                                            {
                                                if (Globals.stretched == false)
                                                {
                                                    Properties.Settings.Default.victory1 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(911f * Globals.sfx)), Convert.ToInt32(Math.Round(251f * Globals.sfy))), bitmap);
                                                    Properties.Settings.Default.victory2 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(1087 * Globals.sfx)), Convert.ToInt32(Math.Round(271 * Globals.sfy))), bitmap);
                                                }
                                                else
                                                {
                                                    var sfx = Globals.resX / 1440.0;
                                                    var sfy = Globals.resY / 1080.0;
                                                    Properties.Settings.Default.victory1 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(680 * sfx)), Convert.ToInt32(Math.Round(347 * sfy))), bitmap);
                                                    Properties.Settings.Default.victory2 = GetColorAtProvided(new System.Drawing.Point(Convert.ToInt32(Math.Round(805 * sfx)), Convert.ToInt32(Math.Round(240 * sfy))), bitmap);
                                                }
                                                if (Properties.Settings.Default.victory1.A != 0)
                                                {
                                                    //Microsoft.VisualBasic.Interaction.MsgBox("Done!", Microsoft.VisualBasic.MsgBoxStyle.Information, "Victory Setup Done!");
                                                    b = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (b == true)
                {
                    break;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://reddit.com/u/aperturecoder");
        }


        private void launchOnStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetStartup();
        }

        private void startMinimizedToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartMinimized = startMinimizedToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // check for 0,0,0,0 values
            if (Properties.Settings.Default.title1 == Color.FromArgb(0, 0, 0, 0) ||
                Properties.Settings.Default.title2 == Color.FromArgb(0, 0, 0, 0) ||
                Properties.Settings.Default.menu2 == Color.FromArgb(0, 0, 0, 0) ||
                Properties.Settings.Default.menu3 == Color.FromArgb(0, 0, 0, 0) ||
                Properties.Settings.Default.menu4 == Color.FromArgb(0, 0, 0, 0) ||
                Properties.Settings.Default.menu5 == Color.FromArgb(0, 0, 0, 0) ||
                Properties.Settings.Default.menu6 == Color.FromArgb(0, 0, 0, 0))
            {
                Microsoft.VisualBasic.Interaction.MsgBox(@"Some pixel values aren't set. Do menu setup again but increase the wait time.", Microsoft.VisualBasic.MsgBoxStyle.Critical, "0,0,0,0 values");
            }
            if (Properties.Settings.Default.stretched)
            {
                Microsoft.VisualBasic.Interaction.MsgBox(@"Stretched detected. Stretched is a bit weird with how it works, try using CRU (Custom Resolution Utility) to set your fortnite fullscreen resolution", Microsoft.VisualBasic.MsgBoxStyle.Exclamation, "Stretched");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
// Source Repository: https://github.com/ApertureC/Fortnite-Music-Changer