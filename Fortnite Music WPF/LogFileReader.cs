using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace Fortnite_Music_WPF
{
    class LogFileReader
    {
        private Audio audio = new Audio();

        // DLL IMPORTS

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, int idProcess, int idThread, uint dwflags);

        [DllImport("user32.dll")]
        internal static extern int UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        internal delegate void WinEventProc(IntPtr hWinEventHook, uint iEvent, IntPtr hWnd, int idObject, int idChild, int dwEventThread, int dwmsEventTime);

        //

        private int lastlinecount = 0; // line count from the last time LogFileRead was run.
        private void LogFileRead(object source, FileSystemEventArgs e) // When fortnite writes to the file, run this - vastly more efficient than getting screenshots :)
        {
            using (FileStream stream = File.Open(Properties.Settings.Default.LogFileFolder + @"\FortniteGame.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            { // opens the file, but also allows fortnite to continue writing.
                using (StreamReader reader = new StreamReader(stream))
                {
                    int LineCount = 0;
                    string CurrentLine = "";
                    List<string> NewLines = new List<string>(); // holds the new lines that were just written to the file
                    reader.BaseStream.Seek(lastlinecount, SeekOrigin.Begin);
                    while ((CurrentLine = reader.ReadLine()) != null)
                    {
                        LineCount++; 
                        if (LineCount > lastlinecount && CurrentLine != "") // if the line is new, add it to the list.
                        {
                            NewLines.Add(CurrentLine); // list because it can usually be 2 or more per update.
                        }
                    }
                    foreach (string i in NewLines) // go through the new lines.
                    {
                        //Debug.WriteLine(i); // just prints log output.

                        // Title menu
                        if (i.Contains("to [UI.State.Startup.SubgameSelect]"))
                        {
                            Debug.WriteLine("State: Subgame");
                            audio.PlayMusic(Properties.Settings.Default.TitleMenu);
                        }
                        //

                        // Main Menu
                        if (i.Contains("to [UI.State.Athena.Frontend]"))
                        {
                            Debug.WriteLine("State: FrontEnd");
                            audio.PlayMusic(Properties.Settings.Default.MainMenu);
                        }
                        //

                        // Matchmaking Finished
                        if (i.Contains("NewState: Finished"))
                        {
                            Debug.WriteLine("State: MatchmakingFinished");
                            audio.StopMusic();
                        }
                        //

                        // Game end
                        if (i.Contains("current=WaitingPostMatch"))
                        {
                            audio.PlayMusic(Properties.Settings.Default.VictoryMusic);
                        }
                        //

                        // Game End
                        if (i.Contains("Preparing to exit"))
                        {
                            Debug.WriteLine("State: Exit");
                            audio.StopMusic();
                        }
                        //
                    }
                    lastlinecount = LineCount;
                }
            }
        }
        WinEventProc listener;
        IntPtr winHook;
        public LogFileReader()
        {
            // windows event hooking for getting when the foreground window changes
            listener = new WinEventProc(ForegroundWindowChanged);
            winHook = SetWinEventHook(3, 3, IntPtr.Zero, listener, 0, 0, 0);
            //

            var defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\FortniteGame\Saved\Logs";
            if (!Directory.Exists(defaultPath)) // Checks for the log file, and if it already exists.
            {
                if (Properties.Settings.Default.LogFileFolder == "")
                {
                    MessageBox.Show("Failed to find log file! You probably installed fortnite somewhere else. You'll have to find it.");
                    Properties.Settings.Default.LogFileFolder = GetLogFolderPath();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }
            }
            else
            {
                Properties.Settings.Default.LogFileFolder = defaultPath;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            Debug.WriteLine("FortniteLog");
            Debug.WriteLine(Properties.Settings.Default.LogFileFolder);

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Filter = "*.log";
            watcher.Path = Properties.Settings.Default.LogFileFolder;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += LogFileRead; // Once the file changes, run it.
            watcher.EnableRaisingEvents = true;
            object EmptyObject = new object();
            audio.ChangeVolume(0); // prevent blasting people's speakers for a second while it's still going through the logs (especially on startup)
            LogFileRead(EmptyObject, new FileSystemEventArgs(WatcherChangeTypes.Changed, "", "")); // run it to catch up if fortnite is open.
            audio.ChangeVolume(Properties.Settings.Default.Volume); // prevent blasting people's speakers while it's still going through the logs (especially on startup)
        }

        public string GetLogFolderPath()
        {
            var defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\FortniteGame\Saved\Logs";
            if (File.Exists(defaultPath))
            {
                return defaultPath; // log files are located here!
            }
            var path = BrowseFile().FileName; // find the file
            path = path.Replace("FortniteGame.log", ""); // get rid of the fortnitegame.log because we only want the directory.
            return path;
        }
        private OpenFileDialog BrowseFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Fortnite Log Files | FortniteGame.log",
                Title = "Select the Fortnite Log File"
            };
            openFileDialog1.ShowDialog();
            return openFileDialog1;
        }

        private uint ForegroundWindowName()
        {
            var window = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(window, out pid);

            return pid;
        }
        private void ForegroundWindowChanged(IntPtr hWinEventHook, uint iEvent, IntPtr hWnd, int idObject, int idChild, int dwEventThread, int dwmsEventTime)
        {
            var FortniteProcessId = ForegroundWindowName();
            Process p = Process.GetProcessById((int)FortniteProcessId);
            if (p.ProcessName != "FortniteClient-Win64-Shipping" && Properties.Settings.Default.PlayInBackground == false)
            {
                audio.ChangeVolume(0);
            }
            else
            {
                audio.ChangeVolume(Properties.Settings.Default.Volume);
            }

        }
    }
}
