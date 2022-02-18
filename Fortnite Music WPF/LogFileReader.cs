using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace Fortnite_Music_WPF
{
    /// <summary>
    /// Reads the fortnite log file to get the fortnite game state
    /// </summary>
    public static class LogFileReader
    {
        public static FortniteState FortniteState { get; private set; } = FortniteState.None;

        // The stream reader for the fortnite log file
        private static StreamReader streamReader;

        // the default location - %localappdata%\FortniteGame\Saved\Logs
        private static readonly string fortniteLogDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\FortniteGame\Saved\Logs";

        /// <summary>
        /// Creates a new log file reader
        /// </summary>
        /// <param name="fortniteLogPath">The fortnite log file to read</param>
        static LogFileReader()
        {
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

        public delegate void GameStatusCallback(FortniteState state);

        public static void AddFortniteGameStatusCallback(GameStatusCallback function)
        {
            callbacks.Add(function);
        }

        /// <summary>
        /// Reads all of the new lines in a log file
        /// </summary>
        private static void readNewLines(object source, FileSystemEventArgs e) // When fortnite writes to the file, run this - vastly more efficient than getting screenshots :)
        {
            // Create the stream reader and file system if the file exists.
            if (streamReader == null && File.Exists(fortniteLogDirectory + @"\FortniteGame.log"))
            {
                FileStream fileStream = File.Open(fortniteLogDirectory + @"\FortniteGame.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(fileStream);
            }

            if (streamReader == null)
                return; // If still null just return, it wasn't created

            if (isLogFileEmpty()) // this is true when fortnite has started up and has cleared the existing log file 
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin); // reset log position

            FortniteState? newFortniteState = null;
            // Get all of the new lines in the log file since the last update
            string currentLine;
            while ((currentLine = streamReader.ReadLine()) != null) // go through the new lines.
            {
                // Menu
                if (currentLine.Contains("to [UI.State.Athena.Frontend]") ||
                    currentLine.Contains("to FrontEnd")) // Main Menu // "to FrontEnd" is for STW support.
                    newFortniteState = FortniteState.Menu;

                // In Game
                else if (currentLine.Contains("NewState: Finished") // Matchmaking finished 
                         || currentLine.Contains(
                             "BeginTearingDown for /Game/Maps/Frontend")) // Save the world matchmaking finish
                    newFortniteState = FortniteState.InGame;

                // Post match
                else if (currentLine.Contains("current=WaitingPostMatch") // game end for fortnitebr
                         || currentLine.Contains("End of Match (FortGameStatePvE")) // game end for stw
                    newFortniteState = FortniteState.GameEnd;

                // Game closed
                else if (currentLine.Contains("Preparing to exit")) // Fortnite closed
                    newFortniteState = FortniteState.None;
            }

            // Call back that the fortnite log file has changed
            if (newFortniteState != null)
            {
                FortniteState = (FortniteState) newFortniteState;
                foreach (GameStatusCallback callback in callbacks)
                    callback(FortniteState);
            }

            streamReader.DiscardBufferedData();
        }

        /// <summary>
        /// Checks if the fortnite log file has been reset, and tells you if fortnite has just launched
        /// </summary>
        /// <returns>Whether the log file is empty</returns>
        private static bool isLogFileEmpty()
        {
            long oldPosition = streamReader.BaseStream.Position;
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

            bool isReset = streamReader.ReadLine() == null; // check if the start of the log file is null

            streamReader.BaseStream.Seek(oldPosition, SeekOrigin.Begin); // reset position

            return isReset;
        }

        private static List<GameStatusCallback> callbacks = new List<GameStatusCallback>();
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
