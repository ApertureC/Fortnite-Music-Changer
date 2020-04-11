using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using Path = System.IO.Path;

namespace Fortnite_Music_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Setup setup = new Setup();
        private LogFileReader logFileReader = new LogFileReader();
        public MainWindow()
        {
            InitializeComponent();

            string version = "4.3";
            UpdateChecker.CheckForUpdate(version, this);
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
            ChangeProperty("TitleMenu", file);

            TitleMenuPathBox.Document.Blocks.Clear();
            TitleMenuPathBox.Document.Blocks.Add(new Paragraph(new Run(Path.GetFileName(file))));
        }

        private void BrowseMenu_Click(object sender, RoutedEventArgs e)
        {
            var file = BrowseFile().FileName;
            ChangeProperty("MainMenu", file);

            MainMenuPathBox.Document.Blocks.Clear();
            MainMenuPathBox.Document.Blocks.Add(new Paragraph(new Run(Path.GetFileName(file))));
        }

        private void BrowseVictory_Click(object sender, RoutedEventArgs e)
        {
            var file = BrowseFile().FileName;
            ChangeProperty("VictoryMusic", file);

            VictoryPathBox.Document.Blocks.Clear();
            VictoryPathBox.Document.Blocks.Add(new Paragraph(new Run(Path.GetFileName(file))));
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
                if (Properties.Settings.Default.Startup == false)
                {
                    var RegKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true); // sets it in registery
                    RegKey.SetValue("Fortnite_Music", System.Reflection.Assembly.GetExecutingAssembly().Location, RegistryValueKind.String);
                    ChangeProperty("Startup", true);
                }
                else
                {
                    var RegKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true); // deletes it in registery
                    RegKey.DeleteValue("Fortnite_Music");
                    ChangeProperty("Startup", false);
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
            ChangeProperty("StartMinimized", LaunchMinimized.IsChecked ?? false);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.reddit.com/user/ApertureCoder");
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeProperty("Volume", (int)Volume.Value);
            AudioPlayer.ChangeVolume((int)Volume.Value);
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
            ChangeProperty("LogFileFolder", logFileReader.GetLogFolderPath());
        }

        private void Obscure_Click(object sender, RoutedEventArgs e)
        {
            ChangeProperty("PlayInBackground", Obscure.IsChecked ?? false);
        }

        private void ChangeProperty(string name, object value)
        {
            Properties.Settings.Default[name] = value;
            Debug.WriteLine(Properties.Settings.Default.StartMinimized);
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
    }
}
