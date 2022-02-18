using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
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
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, int idProcess, int idThread, uint dwflags); // Used to see when the currently foreground window changes

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        internal delegate void WinEventProc(IntPtr hWinEventHook, uint iEvent, IntPtr hWnd, int idObject, int idChild, int dwEventThread, int dwmsEventTime);

        // Used for play in background
        private WinEventProc foregroundWindowChangedListener;

        public MainWindow()
        {
            // Used to detect when the user swaps window to stop music if they request it
            foregroundWindowChangedListener = new WinEventProc(onForegroundWindowChanged);
            SetWinEventHook(3, 3, IntPtr.Zero, foregroundWindowChangedListener, 0, 0, 0);

            InitializeComponent();

            string version = "4.5";
            UpdateChecker.CheckForUpdate(version);

            if (Properties.Settings.Default.StartMinimized) // check if we should minimize
                WindowState = WindowState.Minimized;

            // Load the saved values into the UI
            LaunchMinimized.IsChecked = Properties.Settings.Default.StartMinimized; // set the LaunchMinimized checkbox
            LaunchOnStartup.IsChecked = Properties.Settings.Default.Startup; // set the LaunchOnStarup checkbox
            PlayInBackground.IsChecked = Properties.Settings.Default.PlayInBackground; // set the Obscure
            Volume.Value = Properties.Settings.Default.Volume;

            SetTextOfRichTextBox(MainMenuPathBox, Path.GetFileName(Properties.Settings.Default.MainMenu)); // Set the text of MainMenuPathBox to the path of the Menu music
            SetTextOfRichTextBox(VictoryPathBox, Path.GetFileName(Properties.Settings.Default.VictoryMusic)); // Set the text of VictoryPathBox to the path of the Victory music
            SetTextOfRichTextBox(InGamePathBox, Path.GetFileName(Properties.Settings.Default.InGameMusic)); // Set the text of InGamePathBox to the path of the In game music
        }

        /// <summary>
        /// Creates a file dialog to allow the user to select a log file
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Called when the Main Menu "Browse" button is pressed, creates a dialog to find the audio file to play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseMenu_Click(object sender, RoutedEventArgs e)
        {
            var file = BrowseFile().FileName;
            ChangeProperty("MainMenu", file);

            MainMenuPathBox.Document.Blocks.Clear();
            MainMenuPathBox.Document.Blocks.Add(new Paragraph(new Run(Path.GetFileName(file))));
            AudioPlayer.RefreshPlayingMusic();
        }

        /// <summary>
        /// Called when the Victory Menu "Browse" button is pressed, creates a dialog to find the audio file to play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseVictory_Click(object sender, RoutedEventArgs e)
        {
            var file = BrowseFile().FileName;
            ChangeProperty("VictoryMusic", file);

            VictoryPathBox.Document.Blocks.Clear();
            VictoryPathBox.Document.Blocks.Add(new Paragraph(new Run(Path.GetFileName(file))));
            AudioPlayer.RefreshPlayingMusic();
        }

        private void BrowseInGame_Click(object sender, RoutedEventArgs e)
        {
            var file = BrowseFile().FileName;
            ChangeProperty("InGameMusic", file);

            InGamePathBox.Document.Blocks.Clear();
            InGamePathBox.Document.Blocks.Add(new Paragraph(new Run(Path.GetFileName(file))));
            AudioPlayer.RefreshPlayingMusic();
        }

        /// <summary>
        /// Returns if the thing is ran in admin mode
        /// </summary>
        /// <returns>If the program is run in admin mode</returns>
        private bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Called when the "Launch on startup" check box is clicked, and sets up/removes the starting up on startup functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called when the "Launch minimized" check box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LaunchMinimized_Clicked(object sender, RoutedEventArgs e)
        {
            ChangeProperty("StartMinimized", LaunchMinimized.IsChecked ?? false);
        }

        /// <summary>
        /// Called when the credit hyperlink in the bottom left corner is clicked, opens a web page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Credit_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.github.com/ApertureC");
        }

        /// <summary>
        /// Called when the volume slider changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeProperty("Volume", (int)Volume.Value);
            AudioPlayer.ChangeVolume((int)Volume.Value);
        }

        /// <summary>
        /// Called when the github link in the menu bar is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GithubLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ApertureC/Fortnite-Music-Changer");
        }

        /// <summary>
        /// Called when the Play in background checkbox is cicked, and will change whether the music plays when fortnite is in the background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayInBackground_Click(object sender, RoutedEventArgs e)
        {
            ChangeProperty("PlayInBackground", PlayInBackground.IsChecked ?? false);

            if (PlayInBackground.IsChecked ?? false)
            {
                AudioPlayer.ChangeVolume(Properties.Settings.Default.Volume);
            }
            else
            {
                AudioPlayer.ChangeVolume(0); // to click the button they have to be in the background of fortnite, so just set the volume to 0
            }
        }

        /// <summary>
        /// Changes a persistent property
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The new value</param>
        private void ChangeProperty(string name, object value)
        {
            Properties.Settings.Default[name] = value;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        /// <summary>
        /// Sets the text of a rich text box
        /// </summary>
        /// <param name="Box">The box to change</param>
        /// <param name="SetTo">The new text</param>
        private static void SetTextOfRichTextBox(System.Windows.Controls.RichTextBox Box, string SetTo)
        {
            Box.Document.Blocks.Clear(); // Set the text of TitleMenuPathBox to the path of the Title music
            Box.Document.Blocks.Add(new Paragraph(new Run(SetTo)));
        }

        /// <summary>
        /// Gets the current foreground process
        /// </summary>
        /// <returns>Process Id for the current foreground window</returns>
        private uint getForegroundProcess()
        {
            var window = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(window, out pid);

            return pid;
        }

        /// <summary>
        /// Called when the foreground window changes e.g you click on another window
        /// </summary>
        private void onForegroundWindowChanged(IntPtr hWinEventHook, uint iEvent, IntPtr hWnd, int idObject, int idChild, int dwEventThread, int dwmsEventTime)
        {
            var processId = getForegroundProcess();
            Process process = Process.GetProcessById((int)processId);
            if (process.ProcessName != "FortniteClient-Win64-Shipping" && Properties.Settings.Default.PlayInBackground == false)
            {
                AudioPlayer.ChangeVolume(0);
            }
            else
            {
                AudioPlayer.ChangeVolume(Properties.Settings.Default.Volume);
            }

        }
    }
}
