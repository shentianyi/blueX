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
    /// RFIDScanInPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanInPage : Page
    {
        RFIDScanInWindow parentWindow;
        public RFIDScanInPage()
        {
            InitializeComponent(); 
        }

        public RFIDScanInPage(RFIDScanInWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        private void RFIDScanInPageName_Loaded(object sender, RoutedEventArgs e)
        {
            if (parentWindow.type == RFIDScanType.IN)
            {
                this.contentLabel.Content = "RFIDIn 已准备就绪, 等待料车...";
            }
            else if (parentWindow.type == RFIDScanType.OUT)
            {

                this.contentLabel.Content = "RFIDIn 已准备就绪, 等待料车";
            }
        }


      
    }
}
