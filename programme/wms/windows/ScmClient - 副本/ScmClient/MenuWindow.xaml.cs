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
using ScmClient.Enum;

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
            new RFIDScanInWindow(this,RFIDScanType.IN).Show();
            this.Close();
        }

        private void RFIDScanOutNaviBtn_Click(object sender, RoutedEventArgs e)
        {
            //new RFIDScanOutWindow(this).Show();
            new RFIDScanInWindow(this, RFIDScanType.OUT).Show();
            this.Hide();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
