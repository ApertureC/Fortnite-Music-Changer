using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WMPLib;

namespace Fortnite_Music_WPF
{
    class Main
    {
        WindowsMediaPlayer wmp = new WindowsMediaPlayer();
        //
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public Main()
        {
            wmp.settings.setMode("Loop", true);
        }
        public void preload()
        {
            List<string> list = new List<string>() { Properties.Settings.Default.TitleMenu, Properties.Settings.Default.MainMenu, Properties.Settings.Default.Victory };
            for (int i = 0; i < 3; i++)
            {
                wmp.URL = list[i];
                if (list[i] != "")
                {
                    wmp.controls.play(); // Make each one of them play
                    while (true)
                    {
                        var b = false;
                        try
                        {
                            if (wmp.playState == WMPLib.WMPPlayState.wmppsPlaying)
                            {
                                wmp.controls.stop(); // Stop after they start playing.
                                b = true;
                            }
                        }
                        catch
                        {

                        }
                        if (b == true)
                        {
                            break;
                        }
                    }
                }
            }
        }
        public bool IsFortniteFocused()
        {
            bool focused = false;
            if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0 || Process.GetProcessesByName("GeForceNOWStreamer").Length > 0) // Check that fortnite is open
            {
                GetWindowThreadProcessId(GetForegroundWindow(), out uint pid);
                Debug.WriteLine(Process.GetProcessById((int)pid).ProcessName);
                if (Process.GetProcessById((int)pid).ProcessName == "FortniteClient-Win64-Shipping" || Process.GetProcessById((int)pid).ProcessName == "GeForceNOWStreamer")
                    focused = true;
            }
            return focused;
        }

        public bool DoColorsMatch(List<Point> points, List<Color> colors)
        {
            if (points.Count != 0) // fix it running when it shouldn't be running
            {
                for (int i = 0; i < points.Count; i++)
                {
                    if (colors[i].A==0)
                        return false;

                    if (!CompareColor(GetColorAt(new System.Drawing.Point(points[i].X, points[i].Y)), colors[i])) // Not create point because the point has already been convered to the display in config.
                    {
                        var gca = GetColorAt(points[i]);
                        Debug.WriteLine("TING" + gca);
                        Debug.WriteLine("SCRAA" + colors[i]);
                        return false;
                    }
                }
                return true;
            }
            else
                return false;
        }
        private bool CompareColor(Color c, Color compateTo)
        {
            var math = (c.R == compateTo.R && c.G == compateTo.G && c.B == compateTo.B);
            return math;
        }
        public void PlayMusic(string path)
        {
            try
            {
                if (wmp.URL != path)
                {
                    wmp.URL = path;
                    wmp.controls.play();
                }
                else if (wmp.playState == WMPPlayState.wmppsPaused || wmp.playState == WMPPlayState.wmppsTransitioning)
                {
                    wmp.controls.play();
                }
            }
            catch
            {

            }
        }
        public void PauseMusic()
        {
            if (IsFortniteFocused()==true)
                wmp.controls.pause();
            if (IsFortniteFocused() == false && Properties.Settings.Default.Obscure == false)
                wmp.controls.pause();
            Debug.WriteLine("hi");
        }
        public void ChangeVolume(int volume)
        {
            Debug.WriteLine(volume);
            wmp.settings.volume = volume+100;
        }
        public static System.Drawing.Color GetColorAt(System.Drawing.Point location)
        {
            Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

            while (true)
            {
                using (Graphics gdest = Graphics.FromImage(screenPixel))
                {
                    using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                    {
                        IntPtr hSrcDC = gsrc.GetHdc();
                        IntPtr hDC = gdest.GetHdc();
                        int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                        gdest.ReleaseHdc();
                        gsrc.ReleaseHdc();
                    }
                }
                if (screenPixel.GetPixel(0, 0).A != 0)
                {
                    return screenPixel.GetPixel(0, 0);
                }
            }
        }

        public System.Drawing.Point CreatePoint(int x, int y)
        {
            // Creates a point with fort
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(x * Properties.Settings.Default.sfx)), Convert.ToInt32(Math.Round(y * Properties.Settings.Default.sfy)));
        }
    }
}
