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

        public AgvActionWindow()
        {
            InitializeComponent();
        }

        public AgvActionWindow(PickListWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        private void getInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            Socket socket = null;
            //TcpClientService tcs = new TcpClientService();
            ProtocolService tcs = new ProtocolService();
            byte[] msg = new byte[] {
                //0xFD, 0x13, 0x00, 0x07, 0x0F, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                0xFD, 0x13, 0x00, 0x07, 0x0F, 0x01, 0x00, 0x50, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

                //0x01, 0x00, 0x00, 0x01, 0x02, 0x01, 0xC0, 0x01, 0x00, 0x1B, 0xFF, 0xFF, 0x00
            };


            MessageBox.Show(ScaleConvertor.HexBytesToString(msg));
            //string ip = "192.168.1.254";
            //int port = 9000;
            string ip = ServerConfig.agvHost;
            int port = ServerConfig.agvPort;

            socket = tcs.ConnectServer(ip, port);

            if (socket == null)
            {
                MessageBox.Show("服务器连接已断开....");
                return;
            }

            ProtocolMessage<Socket> rep = tcs.SendMessage(socket, msg);

            if (rep.result)
            {
                //MessageBox.Show("开始接收数据...");
                //byte[] recvBytes = new byte[1024];
                //int bytes = 0;
                //bytes = rep.data.Receive(recvBytes, recvBytes.Length, 0);
                //MessageBox.Show(ScaleConvertor.HexBytesToString(recvBytes));
                //MessageBox.Show(Encoding.Default.GetString(recvBytes));
                //rep.data.Shutdown(SocketShutdown.Both);
                //rep.data.Close();
                MessageBox.Show("结束通讯...");
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
