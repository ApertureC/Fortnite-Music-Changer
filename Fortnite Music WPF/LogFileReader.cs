using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace Fortnite_Music_WPF
{
    public class LogFileReader
    {
        public FortniteState FortniteState { get; set; } = FortniteState.None;

        /// <summary>
        /// The file stream for the fortnite log file
        /// </summary>
        private FileStream fileStream;

        /// <summary>
        /// The stream reader for the fortnite log file
        /// </summary>
        private StreamReader streamReader;

        /// <summary>
        /// Creates a new log file reader
        /// </summary>
        /// <param name="fortniteLogPath">The fortnite log file to read</param>
        public LogFileReader(string fortniteLogDirectory)
        {
            fileStream = File.Open(fortniteLogDirectory + @"\FortniteGame.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            streamReader = new StreamReader(fileStream);

            Debug.WriteLine("Using Fortnite log file: " + fortniteLogDirectory);

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Filter = "FortniteGame.log";
            watcher.Path = fortniteLogDirectory;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += readNewLines; // Once the file changes, read the new lines
            watcher.EnableRaisingEvents = true;

            AudioPlayer.ChangeVolume(0); // prevent blasting people's speakers for a second while it's still going through the logs (especially on startup)
            readNewLines(new object(), new FileSystemEventArgs(WatcherChangeTypes.Changed, "", "")); // run it to catch up if fortnite is open.
            AudioPlayer.ChangeVolume(Properties.Settings.Default.Volume); // prevent blasting people's speakers while it's still going through the logs (especially on startup)
        }

        /// <summary>
        /// Refreshes the current playing music and restarts it
        /// </summary>
        public void RefreshPlayingMusic()
        {
            switch (FortniteState)
            {
                case FortniteState.Title:
                    AudioPlayer.PlayMusic(Properties.Settings.Default.TitleMenu);
                    break;
                case FortniteState.Menu:
                    AudioPlayer.PlayMusic(Properties.Settings.Default.MainMenu);
                    break;
                case FortniteState.GameEnd:
                    AudioPlayer.PlayMusic(Properties.Settings.Default.VictoryMusic);
                    break;
                case FortniteState.InGame:
                case FortniteState.None:
                    AudioPlayer.StopMusic();
                    break;
            }
        }

        /// <summary>
        /// Reads all of the new lines in a log file
        /// </summary>
        private void readNewLines(object source, FileSystemEventArgs e) // When fortnite writes to the file, run this - vastly more efficient than getting screenshots :)
        {
            string CurrentLine = "";
            List<string> NewLines = new List<string>(); // holds the new lines that were just written to the file

            if (isLogFileEmpty()) // this is true when fortnite has started up and has cleared the existing log file 
            {
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin); // reset log position
            }

            while ((CurrentLine = streamReader.ReadLine()) != null)
            {
                if (CurrentLine != "") // if the line is new, add it to the list.
                {
                    NewLines.Add(CurrentLine); // list because it can usually be 2 or more per update.
                }
            }

            foreach (string i in NewLines) // go through the new lines.
            {
                if (i.Contains("to [UI.State.Startup.SubgameSelect]")) // Title menu
                {
                    Debug.WriteLine("State: Subgame");
                    FortniteState = FortniteState.Title;
                    RefreshPlayingMusic();
                }

                if (i.Contains("to [UI.State.Athena.Frontend]") || i.Contains("to FrontEnd")) // Main Menu // "to FrontEnd" is for STW support.
                {
                    Debug.WriteLine("State: FrontEnd");
                    FortniteState = FortniteState.Menu;
                    RefreshPlayingMusic();
                }

                if (i.Contains("NewState: Finished")) // Matchmaking finished
                {
                    Debug.WriteLine("State: MatchmakingFinished");
                    FortniteState = FortniteState.InGame;
                    RefreshPlayingMusic();
                }

                if (i.Contains("current=WaitingPostMatch")) // game end
                {
                    FortniteState = FortniteState.GameEnd;
                    RefreshPlayingMusic();
                }

                if (i.Contains("Preparing to exit")) // Fortnite closed
                {
                    Debug.WriteLine("State: Exit");
                    FortniteState = FortniteState.None;
                    RefreshPlayingMusic();
                }
            }
        }

        
        /// <summary>
        /// Checks if the fortnite log file has been reset, and tells you if fortnite has just launched
        /// </summary>
        /// <returns>Whether the log file is empty</returns>
        private bool isLogFileEmpty()
        {
            long oldPosition = streamReader.BaseStream.Position;
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

            bool isReset = streamReader.ReadLine() == null; // check if the start of the log file is null

            streamReader.BaseStream.Seek(oldPosition, SeekOrigin.Begin); // reset position

            return isReset;
        }
    }

    public enum FortniteState
    {
        None,
        Title,
        Menu,
        InGame,
        GameEnd
    }
}
