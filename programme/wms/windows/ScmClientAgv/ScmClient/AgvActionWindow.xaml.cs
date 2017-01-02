using Brilliantech.Framwork.Utils.ConvertUtil;
using ScmClient.service;
using ScmWcfService;
using ScmWcfService.Config;
using ScmWcfService.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
    /// AgvActionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AgvActionWindow : Window
    {
        PickListWindow parentWindow;
        int currentAgvPoint;

        Socket socket = null;
        ProtocolService tcs = new ProtocolService();

        public AgvActionWindow()
        {
            InitializeComponent();
        }

        public AgvActionWindow(PickListWindow parentWindow, int point)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.currentAgvPoint = point;
        }

        private void getInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            //string ip = "192.168.1.254";
            //int port = 9000;
            string ip = ServerConfig.agvHost;
            int port = ServerConfig.agvPort;

            if (socket == null)
            {
                socket = tcs.ConnectServer(ip, port);

                if (socket == null)
                {
                    MessageBox.Show("服务器连接失败....");
                    return;
                }
            }

            ProtocolMessage<int> rep = tcs.GetAgvInfo(socket);

            if (rep.result)
            {
                currentAgvPoint = rep.data;
            }
            else
            {
                MessageBox.Show("发送失败...");
            }

            return;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parentWindow.Activate();
            parentWindow.ScanTB.Focus();
            parentWindow.ScanTB.SelectAll();
        }
    }
}
