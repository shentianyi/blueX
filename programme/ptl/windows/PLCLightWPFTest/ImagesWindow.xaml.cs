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

namespace PLCLightWPFTest
{
    /// <summary>
    /// ImagesWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImagesWindow : Window
    {
        List<string> images = new List<string>();
        int currentImageIndex = 0;
        public ImagesWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            images = "1.JPG,2.JPG,3.JPG,4.JPG,5.JPG,6.JPG".Split(',').ToList();

            string imgPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Routines\Img\" + images.First());
            currentImageIndex = 0;
            this.image_wi.Source = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute));
            this.image_wi.Visibility = Visibility.Visible;
            SetImageNextPrevVisi();
        }

        private void image_button_MouseUp(object sender, MouseButtonEventArgs e)
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
            string imgPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Routines\Img\" + images[currentImageIndex]);
            this.image_wi.Source = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute));
            
        }

        private void SetImageNextPrevVisi()
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
    }
}
