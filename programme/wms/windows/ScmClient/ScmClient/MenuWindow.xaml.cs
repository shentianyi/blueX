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
using MahApps.Metro.Controls;
using Brilliantech.Framwork.Utils.LogUtil;

namespace ScmClient
{
    /// <summary>
    /// MenuWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MenuWindow : MetroWindow
    {
        public MenuWindow()
        {
            InitializeComponent();
            LogUtil.Logger.Info("WMS Client 启动！");
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

        private void PickBtn_Click(object sender, RoutedEventArgs e)
        {
            new PickScanWindow().Show();
            this.Close();
        }
    }
}
