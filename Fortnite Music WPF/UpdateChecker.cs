// UpdateChecker.cs
// Checks for updates
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fortnite_Music_WPF
{
    class UpdateChecker
    {
        public void Check(string tag) // Method that checks for updates.
        {
            string html;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/ApertureC/Fortnite-Music-Changer/releases/latest?UserAgent=hi");
            request.ContentType = "application/json";
            request.UserAgent = "e";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(html);
            if (data.tag_name != tag)
            {
                System.Windows.MessageBox.Show("An update is available (" + data.name + ")" + Environment.NewLine + " Check the Github. " + Environment.NewLine + "https://github.com/ApertureC/Fortnite-Music-Changer/releases/latest");
            }
        }
    }
}
