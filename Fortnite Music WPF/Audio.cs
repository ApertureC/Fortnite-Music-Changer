using System.Diagnostics;
using WMPLib;

namespace Fortnite_Music_WPF
{
    class Audio
    {
        public static WindowsMediaPlayer wmp = new WindowsMediaPlayer();

        public Audio()
        {
            wmp.settings.setMode("Loop", true); // Make sure audio loops
        }

        /// <summary>
        /// Uses WMP to play music from a given path
        /// </summary>
        public void PlayMusic(string path) // Plays music
        {
            try // Try / catch is here to prevent crashes when WMP says it's currently in use.
            {
                wmp.URL = path; // just play it.
                if (wmp.playState == WMPPlayState.wmppsPaused || wmp.playState == WMPPlayState.wmppsTransitioning || wmp.playState == WMPPlayState.wmppsUndefined) // only play if it's not already playing
                {
                    wmp.controls.play(); // If it's currently paused or swapping tracks, play it.
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Stop playing music that it's playing.
        /// </summary>
        public void StopMusic() // Pauses the music 
        {
            while (true) // loop + try / catch because wmp sometimes doesn't want to play ball.
            {
                try
                {
                    wmp.controls.stop();
                    if (wmp.playState == WMPPlayState.wmppsStopped || wmp.playState == WMPPlayState.wmppsReady || wmp.playState == WMPPlayState.wmppsUndefined)
                    {
                        break;
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Changes the music that's playing.
        /// </summary>
        public void ChangeVolume(int volume) // Changes the volume ("wow couldn't tell!" - you) 
        { 
            wmp.settings.volume = volume;
        }
    }

}
