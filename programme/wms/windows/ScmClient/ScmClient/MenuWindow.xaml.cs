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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScmClient
{
    /// <summary>
    /// MenuWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void RFIDScanInNaviBtn_Click(object sender, RoutedEventArgs e)
        {
            new RFIDScanInWindow(this).Show();
            this.Hide();
        }

        private void RFIDScanOutNaviBtn_Click(object sender, RoutedEventArgs e)
        {
            new RFIDScanOutWindow(this).Show();
            this.Hide();
        }
    }
}
