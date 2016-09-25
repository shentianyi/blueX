using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PLCLightWPFTest.Properties;

namespace PLCLightWPFTest
{
    /// <summary>
    /// ImagesWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImagesWindow : Window
    {
       public List<string> images = new List<string>();
       public int currentImageIndex = 0;
        public ImagesWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            images = "1.JPG,2.JPG,3.JPG,4.JPG,5.JPG,6.JPG".Split(',').ToList();
             
           
            currentImageIndex = 0;
            this.image_wi.Source = new BitmapImage(new Uri(GetImagePath(images.First()), UriKind.RelativeOrAbsolute));
            this.image_wi.Visibility = Visibility.Visible;
            SetImageNextPrevVisi();
        }

        public void image_button_MouseUp(object sender, MouseButtonEventArgs e)
        {

            Label l = sender as Label;
            if (l.Name.Equals("next_image_button"))
            {
                currentImageIndex += 1;
            }
            else
            {
                currentImageIndex -= 1;
            }

            SetImageNextPrevVisi();
            this.image_wi.Source = new BitmapImage(new Uri(GetImagePath(images[currentImageIndex]), UriKind.RelativeOrAbsolute));
            
        }

        public string GetImagePath(string fileName)
        {
            string imagePath = string.Empty;
            if (Settings.Default.IsLocalImage)
            {
                imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Routines\Img\" + fileName);
            }
            else
            {
                imagePath = Settings.Default.FTPServer + fileName;
            }
            return imagePath;
        }
        public void SetImageNextPrevVisi()
        {
            if (currentImageIndex <= 0)
            {
                currentImageIndex = 0;
                prev_image_button.Visibility = Visibility.Collapsed;
                next_image_button.Visibility = Visibility.Collapsed;
                if (images.Count > 1)
                {
                    next_image_button.Visibility = Visibility.Visible;
                }
            }
            else if (currentImageIndex >= images.Count - 1)
            {
                currentImageIndex = images.Count - 1;
                next_image_button.Visibility = Visibility.Collapsed;
                prev_image_button.Visibility = Visibility.Collapsed;
                if (images.Count > 1)
                {
                    prev_image_button.Visibility = Visibility.Visible;
                }
            }
            else if(currentImageIndex<images.Count)
            {
                if (images.Count > 1)
                {

                    next_image_button.Visibility = Visibility.Visible;
                    prev_image_button.Visibility = Visibility.Visible;
                }
            }

        }

        private void image_wi_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new ImageFullWindow(this).ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new Menu().Show();
        }
    }
}
