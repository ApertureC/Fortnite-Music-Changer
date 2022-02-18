using System.Diagnostics;
using WMPLib;

namespace Fortnite_Music_WPF
{
    public static class AudioPlayer
    {
        static AudioPlayer()
        {
            mediaPlayer.settings.setMode("Loop", true);
            LogFileReader.AddFortniteGameStatusCallback(OnGameStateUpdated);
        }

        private static void OnGameStateUpdated(FortniteState state)
        {
            RefreshPlayingMusic();
        }

        /// <summary>
        /// Refreshes the current playing music and restarts it
        /// </summary>
        public static void RefreshPlayingMusic()
        {
            if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length == 0)
            {
                StopMusic();
                return;
            }

            switch (LogFileReader.FortniteState)
            {
                case FortniteState.Menu:
                    PlayMusic(Properties.Settings.Default.MainMenu);
                    break;
                case FortniteState.GameEnd:
                    PlayMusic(Properties.Settings.Default.VictoryMusic); // This is called "Victory music" because a very long time ago it used to be on victory (before I used log files, it used to use pixel colours!! Unfortunately victories aren't logged)
                    break;
                case FortniteState.InGame:
                    PlayMusic(Properties.Settings.Default.InGameMusic);
                    break;
                case FortniteState.None:
                    StopMusic();
                    break;
            }
        }

        /// <summary>
        /// Uses WMP to play music from a given path
        /// </summary>
        public static void PlayMusic(string path) // Plays music
        {
            try // Try / catch is here to prevent crashes when WMP says it's currently in use.
            {
                mediaPlayer.URL = path; // just play it.
                if (mediaPlayer.playState == WMPPlayState.wmppsPaused || mediaPlayer.playState == WMPPlayState.wmppsTransitioning || mediaPlayer.playState == WMPPlayState.wmppsUndefined) // only play if it's not already playing
                    mediaPlayer.controls.play(); // If it's currently paused or swapping tracks, play it.
            }
            catch
            {

            }
        }

        /// <summary>
        /// Stop playing music that it's playing.
        /// </summary>
        public static void StopMusic() // Pauses the music 
        {
            while (true) // loop + try / catch because wmp sometimes throws random errors
            {
                try
                {
                    mediaPlayer.controls.stop();
                    if (mediaPlayer.playState == WMPPlayState.wmppsStopped || mediaPlayer.playState == WMPPlayState.wmppsReady || mediaPlayer.playState == WMPPlayState.wmppsUndefined)
                        break;
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Changes the volume.
        /// </summary>
        public static void ChangeVolume(int volume)
        {
            mediaPlayer.settings.volume = volume;
        }

        private static WindowsMediaPlayer mediaPlayer = new WindowsMediaPlayer();
    }

}
