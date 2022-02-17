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

        // The file stream for the fortnite log file
        private FileStream fileStream;

        // The stream reader for the fortnite log file
        private StreamReader streamReader;

        private readonly string fortniteLogDirectory;

        /// <summary>
        /// Creates a new log file reader
        /// </summary>
        /// <param name="fortniteLogPath">The fortnite log file to read</param>
        public LogFileReader(string t_fortniteLogDirectory)
        {
            fortniteLogDirectory = t_fortniteLogDirectory;

            Debug.WriteLine("Using Fortnite log file: " + fortniteLogDirectory);

            // Watch for file system events for the fortnite log file
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Filter = "FortniteGame.log",
                Path = fortniteLogDirectory,
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += readNewLines; // Once the file changes, read the new lines
            watcher.EnableRaisingEvents = true;

            AudioPlayer.ChangeVolume(0); // prevent blasting people's speakers for a second while it's still going through the logs (especially on startup), so set the volume to 0
            readNewLines(new object(), new FileSystemEventArgs(WatcherChangeTypes.Changed, "", "")); // run it to catch up if fortnite is open.
            AudioPlayer.ChangeVolume(Properties.Settings.Default.Volume);
        }

        /// <summary>
        /// Refreshes the current playing music and restarts it
        /// </summary>
        public void RefreshPlayingMusic()
        {
            if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length == 0)
            {
                AudioPlayer.StopMusic();
                return;
            }

            switch (FortniteState)
            {
                case FortniteState.Menu:
                    AudioPlayer.PlayMusic(Properties.Settings.Default.MainMenu);
                    break;
                case FortniteState.GameEnd:
                    AudioPlayer.PlayMusic(Properties.Settings.Default.VictoryMusic); // This is called "Victory music" because a very long time ago it used to be on victory (before I used log files, it used to use pixel colours!! Unfortunately victories aren't logged)
                    break;
                case FortniteState.InGame:
                    AudioPlayer.PlayMusic(Properties.Settings.Default.InGameMusic);
                    break;
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
            // Create the stream reader and file system if the file exists.
            if (fileStream == null && streamReader == null && File.Exists(fortniteLogDirectory + @"\FortniteGame.log"))
            {
                fileStream = File.Open(fortniteLogDirectory + @"\FortniteGame.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(fileStream);
            }

            if (fileStream == null && streamReader == null)
                return; // If still null just return, it wasn't created


            if (isLogFileEmpty()) // this is true when fortnite has started up and has cleared the existing log file 
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin); // reset log position

            // Get all of the new lines in the log file since the last update
            List<string> newLines = new List<string>();
            string currentLine;
            while ((currentLine = streamReader.ReadLine()) != null) // This crashes some times and idk why (some encoding problem) only happens once in a while so probably isn't a problem. Just relaunch the program!
                if (currentLine != "")
                    newLines.Add(currentLine);

            foreach (string i in newLines) // go through the new lines.
            { 
                if (i.Contains("to [UI.State.Athena.Frontend]") || i.Contains("to FrontEnd")) // Main Menu // "to FrontEnd" is for STW support.
                {
                    Debug.WriteLine("State: FrontEnd");
                    FortniteState = FortniteState.Menu;
                    RefreshPlayingMusic();
                }
                else if (i.Contains("NewState: Finished") // Matchmaking finished 
                         || i.Contains("BeginTearingDown for /Game/Maps/Frontend")) // Save the world matchmaking finish
                {
                    Debug.WriteLine("State: MatchmakingFinished");
                    FortniteState = FortniteState.InGame;
                    RefreshPlayingMusic();
                }
                else if (i.Contains("current=WaitingPostMatch") // game end for fortnitebr
                         || i.Contains("End of Match (FortGameStatePvE")) // game end for stw
                {
                    FortniteState = FortniteState.GameEnd;
                    RefreshPlayingMusic();
                }
                else if (i.Contains("Preparing to exit")) // Fortnite closed
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
        // Title, // Title screen was removed in v19.30 https://www.epicgames.com/fortnite/en-US/news/load-into-the-lobby-in-fortnite-game-mode-select-screen-removed
        Menu,
        InGame,
        GameEnd
    }
}
