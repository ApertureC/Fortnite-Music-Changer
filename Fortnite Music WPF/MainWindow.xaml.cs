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
        private Main main = new Main();


        public MainWindow()
        {
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
                Properties.Settings.Default.menu2 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
                //Properties.Settings.Default.menu5 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
            {
                setup.SetPixelData();
            }

            Config config = new Config();
            //
            Thread MainThread = new Thread(() =>
            {
                main.preload();
                while (true)
                {
                    Thread.Sleep(100); // Memory leak fix - Time:
                    if (main.IsFortniteFocused())
                    {
                        if (main.DoColorsMatch(config.TitleMenuPoints, config.TitleMenuColors))
                        {
                            main.PlayMusic(Properties.Settings.Default.TitleMenu);
                        }
                        else
                        {
                            if (main.DoColorsMatch(config.MainMenuPoints, config.MainMenuColors)
                                || main.DoColorsMatch(config.FriendsPoints, config.FriendsColors))
                                //|| main.DoColorsMatch(config.SettingPoints, config.SettingColors))
                            {
                                main.PlayMusic(Properties.Settings.Default.MainMenu);
                            }
                            else
                            {
                                Debug.WriteLine("VICTORY TIME BOIIIS");
                                Debug.WriteLine(main.DoColorsMatch(config.VictoryPoints, config.VictoryColors));
                                if (main.DoColorsMatch(config.VictoryPoints, config.VictoryColors))
                                    main.PlayMusic(Properties.Settings.Default.Victory);
                                else
                                    main.PauseMusic();
                            }
                        }
                    }
                    else
                    {
                        main.PauseMusic();
                    }
                }
            });
            MainThread.IsBackground = true;
            MainThread.Start();
            //
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private OpenFileDialog BrowseFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Audio Files | *.mp3;*.wav",
                Title = "Select an Audio File"
            };

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
                    Properties.Settings.Default.Startup = false;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }
                else
                {
                    IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
                    IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
                        Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\Fortnite Music Changer.lnk") as IWshRuntimeLibrary.IWshShortcut;
                    shortcut.Arguments = "";
                    shortcut.TargetPath = Environment.CurrentDirectory + @"\Fortnite Music WPF.exe";
                    shortcut.WindowStyle = 1;
                    shortcut.Description = "Fortnite Music Changer";
                    shortcut.WorkingDirectory = Environment.CurrentDirectory + @"\";
                    shortcut.Save(); // add shortcut to startup
                    Properties.Settings.Default.Startup = true;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
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

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Properties.Settings.Default.Volume = (int)Volume.Value;
            main.ChangeVolume((int)Volume.Value);
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ApertureC/Fortnite-Music-Changer");
        }
    }
}
