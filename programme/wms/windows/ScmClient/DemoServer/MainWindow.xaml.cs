using DemoServer.message;
using DemoServer.server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ServerBtn_Click(object sender, RoutedEventArgs e)
        {
            Socket socket = null;
            string ip = "192.168.1.68";
            int port = 8085;
            TcpClientService tcs = new TcpClientService();
            byte[] msg = null;

            socket = tcs.ConnectServer(ip, port);

            if (socket == null)
            {
                MessageBox.Show("服务器连接失败....");
                return;
            }

            msg = Encoding.Default.GetBytes("hello world");
            Message<Socket> rep =  tcs.SendMessage(socket, msg);

            if(rep.result)
            {
                MessageBox.Show("开始接收数据...");
                byte[] recvBytes = new byte[1024];
                int bytes = 0;
                bytes = rep.data.Receive(recvBytes, recvBytes.Length, 0);
                MessageBox.Show(Encoding.Default.GetString(recvBytes));
                rep.data.Shutdown(SocketShutdown.Both);
                rep.data.Close();
                MessageBox.Show("结束通讯...");
                //while (true)
                //{
                //    bytes = rep.data.Receive(recvBytes, recvBytes.Length, 0);
                //    MessageBox.Show(Encoding.Default.GetString(recvBytes));
                //    if (bytes <= 0)
                //        break;
                //}
            }
            else
            {
                MessageBox.Show("发送失败...");
            }

            return;
        }

        private void MainWindow_Load(object sender, RoutedEventArgs e)
        { }

    }
}
