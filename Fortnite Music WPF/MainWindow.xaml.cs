using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
        private Audio main = new Audio();
        private LogFileReader logFileReader = new LogFileReader();
        public MainWindow()
        {
            InitializeComponent();

            string version = "4.0";
            new UpdateChecker().Check(version, this);
            setup.SetUIValues(this);

        }
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
            Properties.Settings.Default.VictoryMusic = file;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            VictoryPathBox.Document.Blocks.Clear();
            VictoryPathBox.Document.Blocks.Add(new Paragraph(new Run(file)));
        }


        private bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void LaunchOnStartup_Clicked(object sender, RoutedEventArgs e)
        {
            if (IsAdministrator()) // Requires admin to set it to start on startup
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

        private void LaunchMinimized_Clicked(object sender, RoutedEventArgs e)
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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ApertureC/Fortnite-Music-Changer/releases/latest");
        }

        private void LogFileUpdate_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LogFileFolder = logFileReader.GetLogFolderPath(); ;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
    }
}
