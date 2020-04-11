// UpdateChecker.cs
// Checks for updates
using System.IO;
using System.Net;
using System.Windows;

namespace Fortnite_Music_WPF
{
    public static class UpdateChecker
    {
        public static void CheckForUpdate(string tag) // Method that checks for updates.
        {
            string html;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/ApertureC/Fortnite-Music-Changer/releases/latest?UserAgent=MusicChangerUpdater");
            request.ContentType = "application/json";
            request.UserAgent = "MusicChangerUpdater";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                }
            }

            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(html);
            if (data.tag_name != tag)
            {
                MessageBoxResult result = MessageBox.Show("An update is available, would you like to go to the download page?", "Update available",MessageBoxButton.YesNo);
            
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://github.com/ApertureC/Fortnite-Music-Changer/releases/latest");
                }
            }
        }
    }
}
