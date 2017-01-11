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
using Brilliantech.Framwork.Utils.LogUtil;
using CanLightServiceLib;

namespace PLCLightWPFTest
{
    /// <summary>
    /// Menu.xaml 的交互逻辑
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new ImagesWindow().Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {  this.Close();
            new ShowClose1().Show();
          
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            new CanServerWindow().Show();
        }
    }
}
