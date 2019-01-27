// Setup.cs
// Holds methods and functions for setting up from starting the program, and first time setup.

using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Security.Principal;
using System.Windows;
using System.Windows.Documents;
using System.Drawing.Imaging;

namespace Fortnite_Music_WPF
{
    class Setup
    {
        private readonly Main main = new Main();
        private Config config;
        public void SetPixelData() // Runs setup to get the pixel values at different screens.
        {
            config = new Config();
            MessageBox.Show(@"
1. Go to the Title Screen (Where you chose Save the World, Battle Royale or Creative)
2. Press 'OK' on this message once you are on that screen
3. Click back onto fortnite");

            var colors = GetColorValuesFromPoints(config.TitleMenuPoints);
            Properties.Settings.Default.title1 = colors[0];
            Properties.Settings.Default.title2 = colors[1];
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            MessageBox.Show(@"
1. Go to the Battle Royale Menu Screen
2. Press 'OK' on this message once you are on that screen
3. Click back onto fortnite");
            colors = GetColorValuesFromPoints(config.MainMenuPoints);
            Properties.Settings.Default.menu2 = colors[0];
            Properties.Settings.Default.menu3 = colors[1];

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            MessageBox.Show(@"
1. Go to the Friends menu (One that pops out from the side)
2. Press 'OK' on this message once you are on that screen
3. Click back onto fortnite");

            colors = GetColorValuesFromPoints(config.FriendsPoints);
            Properties.Settings.Default.menu7 = colors[0];
            Properties.Settings.Default.menu8 = colors[1];
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            // THIS IS NO LONGER USEFUL - CHANGES MADE THIS USELESS - KEEPING IT HERE IF FORTNITE CHANGES IN THE FUTURE.

            /*MessageBox.Show(@"
1. Go to the Settings menu (Where you adjust graphical settings etc.)
2. Press 'OK' on this message once you are on that screen
3. Click back onto fortnite");

            colors = GetColorValuesFromPoints(config.SettingPoints);

            Properties.Settings.Default.menu5 = colors[0];
            Properties.Settings.Default.menu6 = colors[1];

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload(); */
            // JUST DONE: GENERAL SETUP!!!

            MessageBox.Show("Done! Please restart the music changer");
        }

        public void VictorySetup() // Does the setup for victory
        {
            MessageBox.Show("Click back onto fortnite");
            config = new Config();

            var colors = GetColorValuesFromPoints(config.VictoryPoints);

            Properties.Settings.Default.victory1 = colors[0];
            Properties.Settings.Default.victory2 = colors[1];
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            Debug.WriteLine(Properties.Settings.Default.victory1);

            MessageBox.Show("Close and then reopen the music changer.");

        }
        public System.Drawing.Point Stretchpoint(int num, int num2) // Creates a point that is stretched
        {
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(num * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(num2 * (Properties.Settings.Default.ResY / 1080.0))));
        }
        public void GetResolution() // Creates a dialog that asks for your screen resolution.
        {
            GetResolutionWindow window = new GetResolutionWindow();
            var result = window.ShowDialog();
            if (result == true)
            {
                Properties.Settings.Default.ResX = Convert.ToInt32(window.ResX.Text);
                Properties.Settings.Default.ResY = Convert.ToInt32(window.ResY.Text);
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
                //
                Properties.Settings.Default.sfx = Properties.Settings.Default.ResX / 1920.0; // The scale factor of the current resolution from the resolution used to get the points
                Properties.Settings.Default.sfy = Properties.Settings.Default.ResY / 1080.0; // ditto
                //
                // Get the user's aspect ratio from what they've entered
                int gcd = GCD(Properties.Settings.Default.ResX, Properties.Settings.Default.ResY);
                if (Properties.Settings.Default.ResX / gcd == 4 && Properties.Settings.Default.ResY / gcd == 3) // If the ratio is 4:3 then it'll do specific things diffently()
                {
                    Properties.Settings.Default.stretched = true;
                }
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void SetTextOfRichTextBox(System.Windows.Controls.RichTextBox Box, string SetTo)
        {
            Box.Document.Blocks.Clear(); // Set the text of TitleMenuPathBox to the path of the Title music
            Box.Document.Blocks.Add(new Paragraph(new Run(SetTo)));
        }

        public void SetUIValues(MainWindow mainWindow) // Sets UI elements to what they should be (eg. Volume slider to 50 because 50 was the last volume used)
        {
            if (Properties.Settings.Default.StartMinimized)
            {
                mainWindow.WindowState = WindowState.Minimized;
                mainWindow.LaunchMinimized.IsChecked = true;
            }

            mainWindow.FortniteNotActive.IsChecked = Properties.Settings.Default.Obscure;
            mainWindow.LaunchMinimized.IsChecked = Properties.Settings.Default.StartMinimized;
            mainWindow.LaunchOnStartup.IsChecked = Properties.Settings.Default.Startup;

            mainWindow.Volume.Value = Properties.Settings.Default.Volume;

            SetTextOfRichTextBox(mainWindow.TitleMenuPathBox, Properties.Settings.Default.TitleMenu); // Set the text of TitleMenuPathBox to the path of the Title music

            SetTextOfRichTextBox(mainWindow.MainMenuPathBox, Properties.Settings.Default.MainMenu); // Set the text of MainMenuPathBox to the path of the Menu music

            SetTextOfRichTextBox(mainWindow.VictoryPathBox, Properties.Settings.Default.Victory); // Set the text of VictoryPathBox to the path of the Victory music
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        public System.Drawing.Point CreatePoint(int x, int y) // Creates a point that is scaled to the screen
        {
            // Creates a point with fort
            return new System.Drawing.Point(Convert.ToInt32(Math.Round(x * Properties.Settings.Default.sfx)), Convert.ToInt32(Math.Round(y * Properties.Settings.Default.sfy)));
        }

        private List<Color> GetColorValuesFromPoints(List<System.Drawing.Point> points)
        {
            while (true)
            {
                if (new Main().IsFortniteFocused())
                {
                    if (GetColorAt(CreatePoint(1058, 28)).A != 0) // Check if the screen is black
                    {
                        Thread.Sleep(5000); // wait 5 seconds
                        List<Color> colors = new List<Color>(); // create a new list of colors

                        foreach (System.Drawing.Point point in points) // Get the color of each point
                            colors.Add(GetColorAt(new System.Drawing.Point(point.X, point.Y))); // Not create point because the point is already converted to display size

                        using (Bitmap bitmap = new Bitmap(Properties.Settings.Default.ResX, Properties.Settings.Default.ResY)) // check if the user wants to use that image
                        {
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                Rectangle bounds = new Rectangle(0, 0, Properties.Settings.Default.ResX, Properties.Settings.Default.ResY);
                                g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                                Debug.WriteLine(colors);
                                var imageview = new ViewImage("Is this the image you want to use?", bitmap);
                                var result = imageview.ShowDialog();

                                if (result == true)
                                    return colors;
                                else
                                    bitmap.Dispose();
                            }
                        }
                    }
                }
            }
        }

        static int GCD(int a, int b) // stack overflow magic but I can't remember where it's from...
        {
            return b == 0 ? Math.Abs(a) : GCD(b, a % b);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

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
    }
}
