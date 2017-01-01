﻿using ScmWcfService.Config;
using ScmWcfService.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows;

namespace ScmWcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“ProtocolService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 ProtocolService.svc 或 ProtocolService.svc.cs，然后开始调试。
    public class ProtocolService : IProtocolService
    {
        public void DoWork()
        {
        }

        public Socket ConnectServer(string ip, int port)
        {
            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                if (tcpClient == null)
                {
                    tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                if (tcpClient.Connected)
                {
                    return tcpClient;
                }
                //   Socket SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcpClient.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

                MessageBox.Show("连接服务器成功");
                return tcpClient;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        public ProtocolMessage<Socket> SendMessage(Socket socket, byte[] msg)
        {
            ProtocolMessage<Socket> rep = new ProtocolMessage<Socket>(false);

            int count = socket.Send(msg, msg.Length, 0);
            if (count == msg.Count())
            {
                //MessageBox.Show("发送数据(长度" + count.ToString() + "):" + ScaleConvertor.HexBytesToString(msg));
                rep.result = true;
                rep.data = socket;
            }
            else
            {
                MessageBox.Show("发送失败！");
            }

            return rep;
        }
    }
}
