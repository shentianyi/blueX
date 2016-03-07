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
using ScmClient.RFID;

namespace ScmClient
{
    /// <summary>
    /// MenuWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MenuWindow : MetroWindow
    {
        bool shutdown = true;
        public MenuWindow()
        {
            InitializeComponent();
            LogUtil.Logger.Info("WMS Client 启动！");
            //List<RFIDMessage> messages = Parser.StringToList("A101");
            // int c = messages.Count;
        }

        private void RFIDScanInNaviBtn_Click(object sender, RoutedEventArgs e)
        {
            new RFIDScanInWindow(this, RFIDScanType.IN).Show();
            shutdown = false;
            this.Close();
        }

        private void RFIDScanOutNaviBtn_Click(object sender, RoutedEventArgs e)
        {
            //new RFIDScanOutWindow(this).Show();
            new RFIDScanInWindow(this, RFIDScanType.OUT).Show();
            shutdown = false;
            this.Close();
        }

        //private void Close(object sender, RoutedEventArgs e)
        //{
        //   // this.Close();
        //    this.Show();
        //    App.Current.Shutdown();
        //}

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
            shutdown = false;
            this.Close();
        }

 
        private void FirstMenuWindow_Closed(object sender, EventArgs e)
        {
  if (shutdown)
            {
                App.Current.Shutdown();
            }
        }
    }
}
