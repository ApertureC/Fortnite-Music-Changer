// Main.cs
// Holds functions that are used in MainWindow.xaml.cs, keeping it cleaner.
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WMPLib;

namespace Fortnite_Music_WPF
{
    class Audio
    {
        WindowsMediaPlayer wmp = new WindowsMediaPlayer();

        public Audio()
        {
            wmp.settings.setMode("Loop", true); // Make sure audio loops
        }

        public void PlayMusic(string path) // Plays music
        {
            try // Try / catch is here to prevent crashes when WMP says it's currently in use.
            {
                wmp.URL = path; // if not, just play it.
                if (wmp.playState == WMPPlayState.wmppsPaused || wmp.playState == WMPPlayState.wmppsTransitioning || wmp.playState == WMPPlayState.wmppsUndefined)
                {
                    wmp.controls.play(); // If it's currently paused or swapping tracks, play it.
                }
            }
            catch
            {

            }
        }
        public void StopMusic() // Pauses the music 
        {
            while (true) // loop + try/catch because wmp sometimes doesn't want to play ball.
            {
                try
                {
                    Debug.WriteLine(wmp.playState);
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
        public void ChangeVolume(int volume) // Changes the volume ("wow couldn't tell!" - you) 
        {
            wmp.settings.volume = volume;
        }
    }

}
