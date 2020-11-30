using System.Diagnostics;
using WMPLib;

namespace Fortnite_Music_WPF
{
    public static class AudioPlayer
    {
        static AudioPlayer()
        {
            mediaPlayer.settings.setMode("Loop", true);
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
