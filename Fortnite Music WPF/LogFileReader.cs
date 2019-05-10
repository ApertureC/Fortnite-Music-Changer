using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fortnite_Music_WPF
{
    class LogFileReader
    {
        private Audio audio = new Audio();

        private int lastlinecount = 0; // line count from the last time LogFileRead was run.
        private void LogFileRead(object source, FileSystemEventArgs e) // When fortnite writes to the file, run this - vastly more efficient than getting screenshots :)
        {
            using (FileStream stream = File.Open(Properties.Settings.Default.LogFileFolder + @"\FortniteGame.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            { // opens the file, but also allows fortnite to continue writing.
                using (StreamReader reader = new StreamReader(stream))
                {
                    int linecount = 0;
                    string line = "";
                    List<string> newlines = new List<string>();
                    reader.BaseStream.Seek(lastlinecount, SeekOrigin.Begin);
                    while ((line = reader.ReadLine()) != null)
                    {
                        linecount++; // counts number of lines
                        if (linecount > lastlinecount && line != "") // if the line is new, add it to the list.
                        {
                            newlines.Add(line); // list because it can usually be 2 or more per update.
                        }
                    }
                    foreach (string i in newlines) // go through the new lines.
                    {
                        //Debug.WriteLine(i); // just prints log output.

                        if (i.Contains("to SubgameSelect")) // Title menu
                        {
                            Debug.WriteLine("State: Subgame");
                            audio.PlayMusic(Properties.Settings.Default.TitleMenu);
                        }

                        if (i.Contains("to FrontEnd")) // Main Menu
                        {
                            Debug.WriteLine("State: FrontEnd");
                            audio.PlayMusic(Properties.Settings.Default.MainMenu);
                        }

                        if (i.Contains("NewState: Finished")) // Matchmaking Finished
                        {
                            Debug.WriteLine("State: MatchmakingFinished");
                            audio.StopMusic();
                        }
                        if (i.Contains("current=WaitingPostMatch")) // the game has ended, hopefully a victory royale, but it's hard to tell
                        {
                            audio.PlayMusic(Properties.Settings.Default.VictoryMusic);
                        }

                        if (i.Contains("Preparing to exit")) // the game has ended, hopefully a victory royale, but it's hard to tell
                        {
                            Debug.WriteLine("State: Exit");
                            audio.StopMusic();
                        }
                    }
                    lastlinecount = linecount;
                }
            }
        }
        public LogFileReader()
        {
            Debug.WriteLine(Properties.Settings.Default.LogFileFolder == "");
            var defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\FortniteGame\Saved\Logs";
            Debug.WriteLine(defaultPath);
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
            object uh = new object();
            LogFileRead(uh, new FileSystemEventArgs(WatcherChangeTypes.Changed, "doesn't", "matter")); // run it to catch up if fortnite is open.
        }

        public string GetLogFolderPath()
        {
            var defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\FortniteGame\Saved\Logs";
            if (File.Exists(defaultPath))
            {
                return defaultPath; // log files are located here!
            }
            var path = BrowseFile().FileName;
            path = path.Replace("FortniteGame.log", "");
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


    }
}
