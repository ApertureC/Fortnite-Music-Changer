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

        public static string titlemenu = "";
        public static string mainmenu = "";
        public static string victory = "";
        public static bool stretched = false;
        public static bool writelogs = false; // enable this is for want logs to be created. Else only the initialized message will be written.
        public static double sfx = Properties.Settings.Default.ResX / 1920.0;
        public static double sfy = Properties.Settings.Default.ResY / 1080.0;
        public static bool releasebitmap = false;
        public static bool optimize = Properties.Settings.Default.optimize;
        public static bool InGame = false;
        public static int resX = Properties.Settings.Default.ResX;
        public static int resY = Properties.Settings.Default.ResY;

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
            Debug.WriteLine("Main Menu");
            /*System.Drawing.Color colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(428f * sfx)), Convert.ToInt32(Math.Round(548f * sfy))));
            WriteToLog("menu - purchase check" + colorAt.ToString(), false);
            if (int.Parse(colorAt.R.ToString()) == 11 && int.Parse(colorAt.G.ToString()) == 19 && int.Parse(colorAt.B.ToString()) == 47)
            {
                return false;
            } */                   // UNCOMMENTING THIS WILL PREVENT MUSIC PLAYING ON THE BUY SCREEN AT 0.5 BRIGHTNESS

            if (compareColor(GetColorAt(createPoint(512, 36)), Properties.Settings.Default.menu2))
            {
                if (compareColor(GetColorAt(createPoint(909, 1047)), Properties.Settings.Default.menu3))
                {
                    if (Properties.Settings.Default.PlayInParty == false)
                    {
                        if (stretched == false)
                        {
                            if (compareColor(GetColorAt(createPoint(20, 1043)), Properties.Settings.Default.menu4))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            var colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(13f * (sfx / 1440.0))), Convert.ToInt32(Math.Round(1055f * (sfy / 1080.0)))));
                            if (compareColor(colorAt, Properties.Settings.Default.menu5))
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
            if (stretched == false)
            {
                if (compareColor(GetColorAt(createPoint(1897, 10)), Properties.Settings.Default.menu5))
                {
                    if (compareColor(GetColorAt(createPoint(1825, 10)), Properties.Settings.Default.menu6))
                    {
                        return true;
                    }
                }
            }
            else
            {
                var colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1422f * (resX / 1440.0))), Convert.ToInt32(Math.Round(8f * (resY / 1080.0)))));
                if (compareColor(colorAt, Properties.Settings.Default.menu5))
                {
                    colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1370f * (resX / 1440.0))), Convert.ToInt32(Math.Round(8f * (resY / 1080.0)))));
                    if (compareColor(colorAt, Properties.Settings.Default.menu6))
                    {
                        return true;
                    }
                }
            }
            // Friends menu
            Debug.WriteLine(compareColor(GetColorAt(createPoint(783, 21)), Properties.Settings.Default.menu7));
            if (compareColor(GetColorAt(createPoint(783, 21)), Properties.Settings.Default.menu7))
            {
                if (compareColor(GetColorAt(createPoint(3, 30)), Properties.Settings.Default.menu8))
                {
                    return true;
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
            if (Properties.Settings.Default.victory1.A != 0 && Properties.Settings.Default.victory2.A != 0) // check if values aren't nothing
            {
                if (Properties.Settings.Default.stretched == false)
                {
                    if (compareColor(GetColorAt(createPoint(911, 151)), Properties.Settings.Default.victory1))
                    {
                        if (compareColor(GetColorAt(createPoint(1087, 271)), Properties.Settings.Default.victory2))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    System.Drawing.Color colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(680f * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(347f * (Properties.Settings.Default.ResY / 1080.0)))));
                    if (compareColor(colorAt, Properties.Settings.Default.victory1))
                    {
                        colorAt = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(805f * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(240f * (Properties.Settings.Default.ResY / 1080.0)))));
                        if (compareColor(colorAt, Properties.Settings.Default.victory2))
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
            if (writelogs == true)
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
            //create shortcut to file in startup
            if (IsAdministrator()) // Requires admin to start on startup
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk")) // If the file exists - Delete it
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
                    shortcut.Save(); // add shortcut to startup
                }
            }
            else
            {
                // Inform user that you need to be in startup
                launchOnStartupToolStripMenuItem.Checked = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk");
                Microsoft.VisualBasic.Interaction.MsgBox("Setting this to run on startup requires admin privileges (Close the program, right click, Run as administrator)", Microsoft.VisualBasic.MsgBoxStyle.Exclamation, "Administrator privileges required");
            }

        }
        static int GCD(int a, int b)
        {
            return b == 0 ? Math.Abs(a) : GCD(b, a % b);
        }
        /// <summary>
        /// Creates a point with specifified numbers and scales them to the user's aspect ratio (make sure the point is from 16:9)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private System.Drawing.Point createPoint(int x, int y)
        {
            /// Creates a point with fort
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(x * sfx)), Convert.ToInt32(Math.Round(y * sfy)));
        }
        /// Creates a point with fort
        private void setup()
        {
            // I apologize that you have to read this (literally can't clean it up anymore). visit /r/EyeBleach 
            Microsoft.VisualBasic.Interaction.MsgBox("The program will say it isn't responding while waiting for you to click onto fortnite, this is normal, ignore it.", Microsoft.VisualBasic.MsgBoxStyle.Exclamation, "Not responding is fine");
            int waittime = 5;
            Debug.WriteLine(Properties.Settings.Default.title1);
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the title screen (STW or BR selection) on fortnite" + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            var done = false;
            // Title menu
            while (true) // wait until the user enters a value
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0 || Process.GetProcessesByName("GeForceNOWStreamer").Length > 0) // Check that fortnite is open
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(createPoint(1058, 28)).A != 0) // Get a random position and check that the screen isn't nothing
                    {
                        if (Process.GetProcessById((int)pid).ProcessName == "FortniteClient-Win64-Shipping" || Process.GetProcessById((int)pid).ProcessName == "GeForceNOWStreamer") // Search processes for fortnite and check if it's selected
                        {
                            Thread.Sleep(waittime * 1000);
                            // set values
                            Properties.Settings.Default.title1 = GetColorAt(createPoint(1058, 28));
                            Properties.Settings.Default.title2 = GetColorAt(createPoint(985, 780));
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();
                            using (Bitmap bitmap = new Bitmap(resX, resY)) // check if the user wants to use that image
                            {
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    Rectangle bounds = Screen.GetBounds(Point.Empty);
                                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                    var result = viewImage("Is this the title menu? (If there is black space - ignore it)", bitmap); // Opens a window showing the image, asking the user if they want to use it.
                                    if (result == DialogResult.OK)
                                    {
                                        // Set to values
                                        done = true;
                                    }
                                    if (result == DialogResult.Cancel)
                                    {
                                        Environment.Exit(0); // close the program when X is clicked.
                                    }
                                }
                            }

                        }
                    }
                    if (done == true)
                    {
                        this.Activate();
                        break;
                    }
                }
            }
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the Battle Royale menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            done = false;
            // Main menu
            while (true) // wait until the user enters a value
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0 || Process.GetProcessesByName("GeforceNOWStreamer").Length > 0) // Check that fortnite is open
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(createPoint(1058, 28)).A != 0) // Get a random position and check that the screen isn't nothing
                    {
                        if (Process.GetProcessById((int)pid).ProcessName == "FortniteClient-Win64-Shipping" || Process.GetProcessById((int)pid).ProcessName == "GeForceNOWStreamer") // Search processes for fortnite and check if it's selected
                        {
                            Thread.Sleep(waittime * 1000);
                            // set values
                            Properties.Settings.Default.menu2 = GetColorAt(createPoint(512, 36));
                            Properties.Settings.Default.menu3 = GetColorAt(createPoint(909, 1047));
                            if (stretched == false)
                            {
                                Properties.Settings.Default.menu4 = GetColorAt(createPoint(20, 1043));
                            }
                            else
                            {
                                Properties.Settings.Default.menu4 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(13f * (resX / 1440.0))), Convert.ToInt32(Math.Round(1055f * (resY / 1080.0)))));
                            }
                            Properties.Settings.Default.gamemenufn = GetColorAt(createPoint(30, 16));
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();

                            using (Bitmap bitmap = new Bitmap(resX, resY))
                            {
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    Rectangle bounds = Screen.GetBounds(Point.Empty);
                                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                    var result = viewImage("Is this the main menu? (If there is black space - ignore it)", bitmap); // Opens a window showing the image, asking the user if they want to use it.
                                    if (result == DialogResult.OK)
                                    {
                                        // Set to values
                                        done = true;
                                    }
                                    if (result == DialogResult.Cancel)
                                    {
                                        Environment.Exit(0); // close the program when X is clicked.
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
            // Friends menu
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the Friends menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            done = false;
            while (true) // wait until the user enters a value
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0 || Process.GetProcessesByName("GeforceNOWStreamer").Length > 0) // Check that fortnite is open
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(createPoint(1058, 28)).A != 0) // Get a random position and check that the screen isn't nothing
                    {
                        if (Process.GetProcessById((int)pid).ProcessName == "FortniteClient-Win64-Shipping" || Process.GetProcessById((int)pid).ProcessName == "GeForceNOWStreamer") // Search processes for fortnite and check if it's selected
                        {
                            Debug.WriteLine("SetupThrough");
                            Thread.Sleep(waittime * 1000);
                            // set values
                            Properties.Settings.Default.menu7 = GetColorAt(createPoint(783, 21));
                            Properties.Settings.Default.menu8 = GetColorAt(createPoint(3, 30));
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();

                            using (Bitmap bitmap = new Bitmap(resX, resY))
                            {
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    Rectangle bounds = Screen.GetBounds(Point.Empty);
                                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                    var result = viewImage("Is this the friends menu? (If there is black space - ignore it)", bitmap); // Opens a window showing the image, asking the user if they want to use it.
                                    if (result == DialogResult.OK)
                                    {
                                        // Set to values
                                        done = true;
                                    }
                                    if (result == DialogResult.Cancel)
                                    {
                                        Environment.Exit(0); // close the program when X is clicked.
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
            // Settings menu
            Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the Settings menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
            done = false;
            while (true) // wait until the user enters a value
            {
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0 || Process.GetProcessesByName("GeforceNOWStreamer").Length > 0) // Check that fortnite is open
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(createPoint(1058, 28)).A != 0) // Get a random position and check that the screen isn't nothing
                    {
                        if (Process.GetProcessById((int)pid).ProcessName == "FortniteClient-Win64-Shipping" || Process.GetProcessById((int)pid).ProcessName == "GeForceNOWStreamer") // Search processes for fortnite and check if it's selected
                        {
                            Thread.Sleep(waittime * 1000);
                            if (stretched == false)
                            {
                                Properties.Settings.Default.menu5 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                                Properties.Settings.Default.menu6 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
                            }
                            else
                            {
                                Properties.Settings.Default.menu5 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1422f * (resX / 1440.0))), Convert.ToInt32(Math.Round(8f * (resY / 1080)))));
                                Properties.Settings.Default.menu6 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1370f * (resX / 1440.0))), Convert.ToInt32(Math.Round(8f * (resY / 1080)))));
                            }
                            Properties.Settings.Default.gamesettingsfn = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(30 * sfx)), Convert.ToInt32(Math.Round(16 * sfy))));
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();
                            using (Bitmap bitmap = new Bitmap(resX, resY))
                            {
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    Rectangle bounds = Screen.GetBounds(Point.Empty);
                                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                    var result = viewImage("Is this the Settings menu? (If there is black space - ignore it)", bitmap);
                                    if (result == DialogResult.OK)
                                    {
                                        Debug.WriteLine("STUFF2");
                                        done = true;
                                    }
                                    else if (result == DialogResult.Cancel)
                                    {
                                        Environment.Exit(0);
                                    }
                                }
                            }

                        }
                    }
                }
                if (done == true)
                {
                    this.Activate();
                    break;
                }
            }
            Microsoft.VisualBasic.Interaction.MsgBox("(OPTIONAL) After the restart, Go into 50v50 and win a game, press 'Victory Setup' after about 5 seconds of the victory royale screen coming up.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Victory Music");
            Microsoft.VisualBasic.Interaction.MsgBox("Please restart the application (no idea why but it doesn't work without a restart)", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart");
            Application.Exit();
        }

        private void setupGlobalsAndUI()
        {
            writelogs = Properties.Settings.Default.WriteLogs;
            sfx = Properties.Settings.Default.ResX / 1920.0;
            sfy = Properties.Settings.Default.ResY / 1080.0;

            // The music 
            mainmenu = Properties.Settings.Default.MainMenu;
            victory = Properties.Settings.Default.Victory;
            stretched = Properties.Settings.Default.stretched;
            titlemenu = Properties.Settings.Default.TitleMenu;

            // Tick boxes in the GUI
            checkBox1.Checked = Properties.Settings.Default.Obscure;
            launchOnStartupToolStripMenuItem.Checked = Properties.Settings.Default.Startup;

            // Volume 
            trackBar1.Value = Properties.Settings.Default.Volume;
            VolumeNum.Text = Properties.Settings.Default.Volume.ToString();
            wplayer.settings.volume = Properties.Settings.Default.Volume;

            // The path text
            MenuMusicFile.Text = mainmenu;
            TitleMenuFile.Text = titlemenu;
            VictoryMusicFile.Text = victory;

            // Startup text
            launchOnStartupToolStripMenuItem.Checked = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk");
        }
        private void getResolution()
        {
            ResolutionSetter m = new ResolutionSetter();
            DialogResult result = m.ShowDialog();
            if (result == DialogResult.OK)
            {
                Properties.Settings.Default.ResX = m.resX;
                Properties.Settings.Default.ResY = m.resY;
                resX = m.resX;
                resY = m.resY;
                Debug.WriteLine(Properties.Settings.Default.ResX.ToString() + Properties.Settings.Default.ResY.ToString());
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
            else if (result == DialogResult.Cancel)
            {
                Environment.Exit(0);
            }
        }
        private void updateCheck(string tag)
        {
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
        }
        private void preload()
        {
            List<string> list = new List<string>();
            list.Add(titlemenu);
            list.Add(mainmenu);
            list.Add(victory); // Add all music files to list
            for (int i = 0; i < 3; i++)
            {
                wplayer.URL = list[i];
                if (list[i] != "")
                {
                    wplayer.controls.play(); // Make each one of them play
                    while (true)
                    {
                        var b = false;
                        try
                        {
                            if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                            {
                                wplayer.controls.stop(); // Stop after they start playing.
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
        }
        private bool compareColor(Color c, Color compateTo)
        {
            Debug.WriteLine(c.ToString(), compateTo.ToString());
            return Int32.Parse(c.R.ToString()) == compateTo.R && Int32.Parse(c.G.ToString()) == compateTo.G && Int32.Parse(c.B.ToString()) == compateTo.B;
        }

        public Form1()
        {
            // Fixes issue with github releases check.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            InitializeComponent();

            Thread.Sleep(250);
            string version = "2.6";
            updateCheck(version);

            // Set window to minimized if the user has set it to.
            if (Properties.Settings.Default.StartMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
                startMinimizedToolStripMenuItem.Checked = true;
            }

            WriteToLog("Update availaility done", true);

            // Write to log 
            System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "\\log.txt", "Initizalized!" + System.Environment.NewLine);

            if (Properties.Settings.Default.ResX == 0) // if there is no value for the resolution then ask for the resolution
            {
                getResolution();
            }
            WriteToLog("SetRes", true);

            setupGlobalsAndUI();

            Debug.WriteLine(resX.ToString() + resY.ToString());

            // Get the user's aspect ratio from what they've entered
            int gcd = GCD(resX, resY);
            WriteToLog("Stuff", true);
            if (resX / gcd == 4 && resY / gcd == 3) // If the ratio is 4:3 then it'll do specific things diffently()
            {
                stretched = true;
                Properties.Settings.Default.stretched = true;
            }

            // if setup hasn't been done or some values are missing, setup will start.
            if (Properties.Settings.Default.title1 == System.Drawing.Color.FromArgb(0, 0, 0, 0) || Properties.Settings.Default.menu2 == System.Drawing.Color.FromArgb(0, 0, 0, 0) || Properties.Settings.Default.menu5 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
            {
                setup();
            }

            // Log writing
            WriteToLog("Setup done :D", true);
            WriteToLog("Scale factor X: " + sfx.ToString(), false);
            WriteToLog("Scale factor Y: " + sfy.ToString(), false);
            WriteToLog("Resolution X " + Properties.Settings.Default.ResX, false);
            WriteToLog("Resolution Y " + Properties.Settings.Default.ResY, false);
            WriteToLog("Menu " + mainmenu, false);
            WriteToLog("Title " + titlemenu, false);
            WriteToLog("Victory " + victory, false);

            int currentlyplaying = 0; // a value that shows which music is playing - so it won't keep trying to play the same music.

            Debug.WriteLine("Loaded");

            // Alert users that there's no MP3s added
            if (Properties.Settings.Default.TitleMenu == "" && Properties.Settings.Default.MainMenu == "" && Properties.Settings.Default.Victory == "")
            {
                Microsoft.VisualBasic.Interaction.MsgBox("The program has no loaded mp3s, mp3s need to be loaded to play music! Hit browse to load mp3s", Microsoft.VisualBasic.MsgBoxStyle.Information, "No MP3s");
            }

            // Preload
            Thread t = new Thread(() =>
            {
                wplayer.settings.setMode("loop", true);
                wplayer.settings.setMode("autoStart", false);

                preload();

                while (true)
                {
                    Debug.WriteLine("Loop");
                    Thread.Sleep(10);
                    WriteToLog("----- NEW CYCLE -----", false);
                    MethodInvoker mouse = delegate // Get the position of the mouse and set label1 as the label it will change text on (must be invoked)
                    {
                        label1.Text = Cursor.Position.ToString();
                        return;
                    };
                    WriteToLog("Fortnite check open", false);
                    if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0 || Process.GetProcessesByName("GeForceNOWStreamer").Length > 0) // Check that fortnite is open and focused
                    {
                        Debug.WriteLine("Processes");
                        bool focused = false;
                        uint pid;
                        GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                        Debug.WriteLine(Process.GetProcessById((int)pid).ProcessName);
                        if (Process.GetProcessById((int)pid).ProcessName == "FortniteClient-Win64-Shipping" || Process.GetProcessById((int)pid).ProcessName == "GeForceNOWStreamer")
                        {
                            focused = true;
                        }
                        WriteToLog("focus: " + focused.ToString(), false);
                        Debug.WriteLine(focused);
                        if (focused == true) // if fortnite is focused
                        {
                            Debug.WriteLine("Focused");
                            try
                            {
                                if (GetColorAt(new System.Drawing.Point(0, 0)).A != 0)
                                {
                                    // Title menu is like this because I didn't add it to a method and I cannot be bothered to that.
                                    Debug.WriteLine("Passed");
                                    Debug.WriteLine(GetColorAt(createPoint(1058, 28)));
                                    var c = GetColorAt(createPoint(1058, 28));
                                    Debug.WriteLine("Title menu");
                                    Debug.WriteLine(c.ToString());
                                    Debug.WriteLine(Properties.Settings.Default.title1);
                                    if (compareColor(c, Properties.Settings.Default.title1))
                                    {
                                        c = GetColorAt(createPoint(985, 780));
                                        WriteToLog(c.ToString(), false);
                                        Debug.WriteLine(c);
                                        if (compareColor(c, Properties.Settings.Default.title2))
                                        {
                                            WriteToLog("Started playing title menu", false);

                                            if ((currentlyplaying != 1 || wplayer.URL != titlemenu))
                                            {
                                                currentlyplaying = 1;
                                                wplayer.controls.pause();
                                                wplayer.URL = titlemenu;
                                            }
                                            wplayer.controls.play();
                                        }
                                    }
                                    else if (mainMenuMusic(sfx, sfy) == true) // to do stop thread on menu setup + changing resolution
                                    {
                                        WriteToLog("Started playing main menu", false);
                                        if ((currentlyplaying != 2 || wplayer.URL != mainmenu))
                                        {
                                            currentlyplaying = 2;
                                            wplayer.controls.pause();
                                            wplayer.URL = mainmenu;
                                        }
                                        wplayer.controls.play();
                                    }
                                    else if (victoryMusic(sfx, sfy) == true)
                                    {
                                        WriteToLog("Started playing victory", false);
                                        if ((currentlyplaying != 3 || wplayer.URL != victory))
                                        {
                                            currentlyplaying = 3;
                                            wplayer.controls.pause();
                                            wplayer.URL = victory;
                                        }
                                        wplayer.controls.play();
                                    }
                                    else
                                    {
                                        if (checkBox1.Checked == false) // Stop music if "play when obscured" isn't enabled. I don't see the point why this is here but I don't want to break stuff
                                        {
                                            Debug.WriteLine("STOPHERE");
                                            wplayer.controls.pause();
                                            //wplayer.URL = "";
                                        }
                                        else if (focused == true) // If fortnite is focused then stop it.
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
                        else
                        {
                            if (checkBox1.Checked == false)
                            {
                                try
                                {
                                    wplayer.controls.pause(); // Stop if "play when obscured" isn't enabled
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
            // Select title menu file
            Debug.WriteLine("open");
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                titlemenu = openFileDialog1.FileName;
                TitleMenuFile.Text = openFileDialog1.FileName;

                Properties.Settings.Default.TitleMenu = openFileDialog1.FileName; // save it
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
                Debug.WriteLine("Hi");
            }
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        public static System.Drawing.Color GetColorAt(System.Drawing.Point location)
        {
            Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

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
        private void button2_Click(object sender, EventArgs e) // Select Main menu music file
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mainmenu = openFileDialog1.FileName;
                MenuMusicFile.Text = openFileDialog1.FileName;
                Properties.Settings.Default.MainMenu = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // Play when obscured (doesn't stop music unless the user is selected on fortnite)
        {
            Properties.Settings.Default.Obscure = checkBox1.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

        }

        private void trackBar1_Scroll(object sender, EventArgs e) // Volume update
        {
            wplayer.settings.volume = trackBar1.Value;
            VolumeNum.Text = trackBar1.Value.ToString();
            Properties.Settings.Default.Volume = trackBar1.Value;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

        }

        private void button3_Click(object sender, EventArgs e) // Victory music file selector
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                victory = openFileDialog1.FileName;
                VictoryMusicFile.Text = openFileDialog1.FileName;
                Properties.Settings.Default.Victory = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            getResolution();

            Microsoft.VisualBasic.Interaction.MsgBox("Please Restart the program", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart required");

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            setup();
        }

        private void licensesToolStripMenuItem_Click(object sender, EventArgs e) // Licenses
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
            optimize = checkBox4.Checked;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
        // Victory Positions 16:9
        // 911, 251
        // 1087, 271

        private void button5_Click(object sender, EventArgs e) // VICTORY
        {
            Debug.WriteLine(Clipboard.ContainsImage());
            //bitmap.Save(@"C:\Users\Aperture\Documents\oof.bmp");
            Microsoft.VisualBasic.Interaction.MsgBox("Press OK and immediately click onto Fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Victory Setup");
            while (true)
            {
                var b = false;
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                {
                    uint pid;
                    GetWindowThreadProcessId(GetForegroundWindow(), out pid);
                    if (GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy)))).A != 0)
                    {
                        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                        {
                            if (p.Id == pid && p.ProcessName == "FortniteClient-Win64-Shipping")
                            {
                                if (stretched == false)
                                {
                                    Properties.Settings.Default.victory1 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(911f * sfx)), Convert.ToInt32(Math.Round(251f * sfy))));
                                    Properties.Settings.Default.victory2 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(1087 * sfx)), Convert.ToInt32(Math.Round(271 * sfy))));
                                }
                                else
                                {
                                    var sfx = resX / 1440.0;
                                    var sfy = resY / 1080.0;
                                    Properties.Settings.Default.victory1 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(680 * sfx)), Convert.ToInt32(Math.Round(347 * sfy))));
                                    Properties.Settings.Default.victory2 = GetColorAt(new System.Drawing.Point(Convert.ToInt32(Math.Round(805 * sfx)), Convert.ToInt32(Math.Round(240 * sfy))));
                                }
                                using (Bitmap bitmap = new Bitmap(resX, resY)) // check if the user wants to use that image
                                {
                                    using (Graphics g = Graphics.FromImage(bitmap))
                                    {
                                        Rectangle bounds = Screen.GetBounds(Point.Empty);
                                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                                        var result = viewImage("Is this the victory screen? (If there is black space - ignore it)", bitmap); // Opens a window showing the image, asking the user if they want to use it.
                                        if (result == DialogResult.OK)
                                        {
                                            // Set to values
                                            b = true;
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