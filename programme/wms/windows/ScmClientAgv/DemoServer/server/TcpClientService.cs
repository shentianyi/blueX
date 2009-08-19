using DemoServer.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoServer.server
{
    public class TcpClientService
    {
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

        public Message<Socket> SendMessage(Socket socket, byte[] msg)
        {
            Message<Socket> rep = new Message<Socket>(false);

            int count = socket.Send(msg, msg.Length, 0);
            if (count == msg.Count())
            {
                MessageBox.Show("发送数据" + count.ToString() + ":" + Encoding.Default.GetString(msg));
                rep.result = true;
                rep.data = socket;
            } else
            {
                MessageBox.Show("发送失败！");
            }

            return rep;
        }
    }
}
