using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fortnite_Music_WPF
{
    /// <summary>
    /// Interaction logic for ViewImage.xaml
    /// </summary>
    public partial class ViewImage : Window
    {
        BitmapImage BitmapToImageSource(Bitmap bitmap) // https://stackoverflow.com/questions/22499407/how-to-display-a-bitmap-in-a-wpf-image
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public ViewImage(string Text, Bitmap bitmap)
        {
            InitializeComponent();
            image.Source = BitmapToImageSource(bitmap);
            text.Text = Text;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Retake_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
