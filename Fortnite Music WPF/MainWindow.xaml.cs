using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Fortnite_Music_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // CONFIG
        //
        private Setup setup = new Setup();


        public MainWindow()
        {
            Config config = new Config();

            // Fixes issue with github releases check.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            InitializeComponent();

            Thread.Sleep(250);
            string version = "3.0";
            new UpdateChecker().Check(version);


            setup.SetUIValues(this);

            if (Properties.Settings.Default.ResX == 0 || Properties.Settings.Default.ResY == 0)
                setup.GetResolution();

            if (Properties.Settings.Default.title1 == System.Drawing.Color.FromArgb(0, 0, 0, 0) || // Does it have empty values? If so then re-do setup.
                Properties.Settings.Default.menu2 == System.Drawing.Color.FromArgb(0, 0, 0, 0) ||
                Properties.Settings.Default.menu5 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
            {
                setup.SetPixelData();
            }

            Main main = new Main();

            //
            Thread MainThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100); // Memory leak fix - Time:
                    if (main.DoColorsMatch(config.TitleMenuPoints, config.TitleMenuColors))
                        main.PlayMusic(Properties.Settings.Default.TitleMenu);
                    else
                    {
                        List<System.Drawing.Color> MainListColor = new List<System.Drawing.Color>();
                        MainListColor = MainListColor.Concat(config.MainMenuColors).ToList();
                        MainListColor = MainListColor.Concat(config.FriendsColors).ToList();
                        MainListColor = MainListColor.Concat(config.SettingColors).ToList();

                        List<System.Drawing.Point> MainListPoints = new List<System.Drawing.Point>();
                        MainListPoints = MainListPoints.Concat(config.MainMenuPoints).ToList();
                        MainListPoints = MainListPoints.Concat(config.FriendsPoints).ToList();
                        MainListPoints = MainListPoints.Concat(config.SettingPoints).ToList();

                        if (main.DoColorsMatch(MainListPoints, MainListColor))
                            main.PlayMusic(Properties.Settings.Default.MainMenu);
                        else

                        if (main.DoColorsMatch(config.VictoryPoints, config.VictoryColors))
                            main.PlayMusic(Properties.Settings.Default.Victory);
                        else
                            main.PauseMusic();
                    }
                }
            });
            MainThread.Start();
            //
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private System.Drawing.Point createPoint(int x, int y)
        {
            // Creates a point with fort
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(x * Properties.Settings.Default.sfx)), Convert.ToInt32(Math.Round(y * Properties.Settings.Default.sfy)));
        }
        public System.Drawing.Point stretchpoint(int num, int num2)
        {
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(num * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(num2 * (Properties.Settings.Default.ResY / 1080.0))));
        }
        private OpenFileDialog BrowseFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Audio Files | *.mp3;*.wav";
            openFileDialog1.Title = "Select an Audio File";

            openFileDialog1.ShowDialog();
            return openFileDialog1;
        }
        private void BrowseTitle_Click(object sender, RoutedEventArgs e)
        {
            var file = BrowseFile().FileName;
            Properties.Settings.Default.TitleMenu = file;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            TitleMenuPathBox.Document.Blocks.Clear();
            TitleMenuPathBox.Document.Blocks.Add(new Paragraph(new Run(file)));
        }

        private void BrowseMenu_Click(object sender, RoutedEventArgs e)
        {
            var file = BrowseFile().FileName;
            Properties.Settings.Default.MainMenu = file;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            MainMenuPathBox.Document.Blocks.Clear();
            MainMenuPathBox.Document.Blocks.Add(new Paragraph(new Run(file)));
        }

        private void BrowseVictory_Click(object sender, RoutedEventArgs e)
        {
            var file = BrowseFile().FileName;
            Properties.Settings.Default.Victory = file;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            VictoryPathBox.Document.Blocks.Clear();
            VictoryPathBox.Document.Blocks.Add(new Paragraph(new Run(file)));
        }

        private void FortniteNotActive_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Obscure = FortniteNotActive.IsChecked ?? false;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void VictorySetup_Click(object sender, RoutedEventArgs e)
        {
            setup.VictorySetup();
        }

        private void MenuSetup_Click(object sender, RoutedEventArgs e)
        {
            setup.SetPixelData();
        }

        private void ChangeResolution_Click(object sender, RoutedEventArgs e)
        {
            setup.GetResolution();
        }

        private bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void LaunchOnStartup_Checked(object sender, RoutedEventArgs e)
        {
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
                LaunchOnStartup.IsChecked = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk");
                MessageBox.Show("Setting this to run on startup requires admin (Close the program, right click, Run as administrator)");
            }
        }

        private void LaunchMinimized_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.StartMinimized = LaunchMinimized.IsChecked ?? false;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.reddit.com/user/ApertureCoder");
        }
    }
}
