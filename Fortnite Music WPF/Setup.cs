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
        private static void SetTextOfRichTextBox(System.Windows.Controls.RichTextBox Box, string SetTo)
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

            mainWindow.LaunchMinimized.IsChecked = Properties.Settings.Default.StartMinimized;
            mainWindow.LaunchOnStartup.IsChecked = Properties.Settings.Default.Startup;

            mainWindow.Volume.Value = Properties.Settings.Default.Volume;

            SetTextOfRichTextBox(mainWindow.TitleMenuPathBox, Path.GetFileName(Properties.Settings.Default.TitleMenu)); // Set the text of TitleMenuPathBox to the path of the Title music

            SetTextOfRichTextBox(mainWindow.MainMenuPathBox, Path.GetFileName(Properties.Settings.Default.MainMenu)); // Set the text of MainMenuPathBox to the path of the Menu music

            SetTextOfRichTextBox(mainWindow.VictoryPathBox, Path.GetFileName(Properties.Settings.Default.VictoryMusic)); // Set the text of VictoryPathBox to the path of the Victory music
        }
    }
}