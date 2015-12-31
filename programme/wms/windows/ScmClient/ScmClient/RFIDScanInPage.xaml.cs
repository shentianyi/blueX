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
    /// RFIDScanPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanInPage : Page
    {
        public RFIDScanInPage()
        {
            InitializeComponent();
        }

        private void Grid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MessageBox.Show("Open");
        }

        private void Grid_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            MessageBox.Show("Close");
        }
    }
}
