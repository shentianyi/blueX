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

        PathOptimizationService pos = new PathOptimizationService();
        Socket socket = null;
        ProtocolService tcs = new ProtocolService();
        private byte[] station_msg = new byte[] {
                                                          //方向
                0xFD, 0x13, 0x00, 0x07, 0x0F, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

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

            if (rep != null && rep.result)
            {
                //MessageBox.Show("小车返回信息");
                currentAgvPoint = rep.data;
                label.Content = currentAgvPoint;
            }
            else
            {
                MessageBox.Show("发送失败...");
            }

            return;
        }

        private void sendDesStation(byte[] msg)
        {
            if (socket == null)
            {
                socket = tcs.ConnectServer(ServerConfig.agvHost, ServerConfig.agvPort);

                if (socket == null)
                {
                    MessageBox.Show("服务器连接失败....");
                    return;
                }
            }

            ProtocolMessage<Socket> rep = tcs.SendMessage(socket, msg);

            if (rep != null && rep.result)
            {
                //MessageBox.Show("开始接收数据...");
                //byte[] recvBytes = new byte[1024];
                //int bytes = 0;
                ////bytes = rep.data.Receive(recvBytes, recvBytes.Length, 0);
                ////MessageBox.Show(ScaleConvertor.HexBytesToString(recvBytes));
                ////MessageBox.Show(Encoding.Default.GetString(recvBytes));
                ////rep.data.Shutdown(SocketShutdown.Both);
                ////rep.data.Close();
                //MessageBox.Show("结束通讯...");
            }
            else
            {
                MessageBox.Show("发送失败...");
            }

            return;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            parentWindow.Activate();
            parentWindow.ScanTB.Focus();
            parentWindow.ScanTB.SelectAll();
        }

        private void readyBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] msg = station_msg;
            //cmd type
            msg[05] = 0x01;
            //direction
            msg[8] = pos.GetBestDirection(currentAgvPoint, 50);
            //point
            msg[7] = 0x32;
            //currentAgvPoint = ScaleConvertor.HexByteToDecimal(0x32);
            currentAgvPoint = 50;

            sendDesStation(msg);
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] msg = station_msg;
            //cmd type
            msg[05] = 0x01;
            //direction
            msg[8] = pos.GetBestDirection(currentAgvPoint, 41); ;
            //point
            msg[7] = 0x29;
            currentAgvPoint = 41;

            sendDesStation(msg); 
        }

        private void finishBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] msg = station_msg;
            //cmd type
            msg[05] = 0x01;
            //direction
            msg[8] = pos.GetBestDirection(currentAgvPoint, 42);
            //point
            msg[7] = 0x2A;
            currentAgvPoint = 42;

            sendDesStation(msg);
        }

        private void upBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] msg = station_msg;
            //cmd
            msg[05] = 0x06;

            sendDesStation(msg);
        }

        private void downBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] msg = station_msg;
            //cmd
            msg[05] = 0x07;

            sendDesStation(msg);
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] msg = station_msg;
            //cmd
            msg[05] = 0x02;
            msg[8] = 0x00;
            msg[7] = 0x00;

            sendDesStation(msg);
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            byte[] msg = station_msg;
            //cmd
            msg[05] = 0x01;
            msg[8] = 0x00;
            msg[7] = 0x00;

            sendDesStation(msg);
        }
    }
}
