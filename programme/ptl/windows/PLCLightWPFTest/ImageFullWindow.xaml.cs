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
    /// ImageFullWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageFullWindow : Window
    {
        public ImagesWindow parentWindow;
        public ImageFullWindow()
        {
            InitializeComponent();
        }

        public ImageFullWindow(ImagesWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            try {
                this.image.Source = parentWindow.image_wi.Source;
            }catch(Exception e) { }
        }


    }
}
