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

namespace ScmClient
{
    /// <summary>
    /// RFIDScanWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanInWindow : Window
    {
        private MenuWindow menuWindow;
        public RFIDScanInWindow()
        {
            InitializeComponent();
        }
        public RFIDScanInWindow(MenuWindow menuWindow)
        {
            InitializeComponent();
            this.menuWindow = menuWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NaviFrame.NavigationService.Navigate(new RFIDScanInPage());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.menuWindow != null && !this.menuWindow.IsActive) {
                this.menuWindow.Activate();
                this.menuWindow.Show();
            }
            this.Close();
        }
         
    }
}
