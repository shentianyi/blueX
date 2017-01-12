
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Brilliantech.Framwork.Utils.ConvertUtil;
using Brilliantech.Framwork.Utils.LogUtil;

namespace CanLightServiceLib
{
    public class ClientLightCmdData
    {
        public string ClientIP { get; set; }
        public byte[] ClientCmd { get; set; }
    }

    public class CanLightTcpServer
    {
        private string ip;
        private string port;
        private Socket server;
        private Thread serverListenThread;


        Boolean isRun = true;
        /// <summary>
        /// can转换器连接，ID和IP
        /// </summary>
        //   Dictionary<string, string> canClients = new Dictionary<string, string>();
        List<CanModel> canClients = new List<CanModel>();
        Dictionary<string, Socket> clients = new Dictionary<string, Socket>();
        Dictionary<string, Thread> clientThreads = new Dictionary<string, Thread>();

        // 处理的消息队列
        private Queue comDataQ = new Queue();
        private Queue receiveMessageQueue;
        private Thread receiveMessageThread;
        private ManualResetEvent receivedEvent = new ManualResetEvent(false);

        //
        public CanLightTcpServer(string ip, string port, List<CanModel> canClients)
        {
            this.ip = ip;
            this.port = port;
            this.canClients = canClients;

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port)));
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            try
            {

                receiveMessageThread = new Thread(this.ReceiveMessageThread);
                receiveMessageQueue = Queue.Synchronized(comDataQ);
                receiveMessageThread.IsBackground = true;
                receiveMessageThread.Start();

                server.Listen(50);
                serverListenThread = new Thread(this.ListenClientContect);
                serverListenThread.IsBackground = true;
                serverListenThread.Start();



                LogUtil.Logger.InfoFormat("【开启服务】{0}:{1}",ip,port);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }


        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private void ListenClientContect()
        {
            while (isRun)
            {
                Socket client = server.Accept();
                
                try
                {
                    string tempIp = GetSocketIP(client);
                    RemoveClient(tempIp);
                    if (this.IsCanClient(tempIp))
                    {
                       // client.ReceiveTimeout = 10000;
                        FindCanClientByIp(tempIp).OnLine = true;
                        LogUtil.Logger.Info("请求连接的【CAN】控制器IP：" + tempIp);
                    }
                    else
                    {
                        LogUtil.Logger.Info("请求连接的客户端IP：" + tempIp);
                    }
                    //  LogUtil.Logger.Info("移除字典item成功！");
                    //}

                    // 将ip作为key,socket作为value存入字典中
                    clients.Add(tempIp, client);

                    LogUtil.Logger.Info(string.Format("【{0}】已连接!", client.RemoteEndPoint.ToString()));

                    // 启动监听消息线程 
                    Thread listenMsgTH = new Thread(ReceiveMsg);
                    receiveMessageQueue = Queue.Synchronized(comDataQ);

                    listenMsgTH.IsBackground = true;
                    listenMsgTH.Start(client);

                    // 将子线程加入线程池中
                    clientThreads.Add(tempIp, listenMsgTH);
                }
                catch (Exception e)
                {
                    LogUtil.Logger.Error(string.Format("客户端【{0}】连接服务器失败：", client.RemoteEndPoint) + e.Message, e);
                }
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            try
            {
                server.Close();

                serverListenThread.Abort();
                receiveMessageThread.Abort();

                LogUtil.Logger.Info("【开启服务】");
            }
            catch (Exception ex)
            {

                LogUtil.Logger.Error(ex.Message, ex);
            }
        }



        /// <summary>
        /// 消息转发机制
        /// </summary>
        private void ReceiveMsg(object clientTmp)
        {
            Socket client = clientTmp as Socket;

            string clientIP = GetSocketIP(client);

            while (true)
            {
                try
                {
                    if ((!clients.Keys.Contains(clientIP)) || (!client.Connected))
                    {
                        break;
                    }
                    byte[] result = new byte[1000];
                    int dataLength = client.Receive(result);

                    byte[] message = result.Take(dataLength).ToArray();
                    string messageStr = ScaleConvertor.HexBytesToString(message);

                    if (dataLength > 0)
                    {
                        if (!this.IsCanClient(clientIP))
                        {
                            LogUtil.Logger.InfoFormat("【接收到客户端:{0} 信息】{1}", clientIP, ScaleConvertor.HexBytesToString(message));
                            LogUtil.Logger.Info("【" + clientIP + "】" + "来自【请求灯控制】的消息");
                            // 控制灯灭或灯亮
                            if (message.Length > 5)
                            {
                                if (message[0] == 0xAB && message[message.Length - 1] == 0xFF)
                                { 
                                    ClientLightCmdData cmd = new ClientLightCmdData()
                                    {
                                        ClientIP = clientIP,
                                        ClientCmd = message
                                    };
                                    this.receiveMessageQueue.Enqueue(cmd);
                                    this.receivedEvent.Set();
                                
                                }
                                else
                                {
                                    LogUtil.Logger.Info("【" + clientIP + "】" + "来自【请求灯控制】的消息--头尾不符合");
                                }
                            }
                            else
                            {
                                LogUtil.Logger.Info("【" + clientIP + "】" + "来自【请求灯控制】的消息--长度不符合");
                            }
                        }
                        else
                        {
                            FindCanClientByIp(clientIP).OnLine = true;
                            LogUtil.Logger.InfoFormat("【接收到 CAN:{0} 信息】{1}", clientIP, ScaleConvertor.HexBytesToString(message));
                            // 暂时不处理
                        }

                    }

                }
                catch (ObjectDisposedException exx)
                {
                    RemoveClient(clientIP);
                   
                    LogUtil.Logger.Error(exx.Message, exx);
                    break;
                }
                catch (SocketException exxx)
                {
                    RemoveClient(clientIP);
                  
                    LogUtil.Logger.Error(exxx.Message, exxx);
                    break;
                }
                catch (Exception ee)
                {
                    RemoveClient(clientIP);
                   
                    LogUtil.Logger.Error(ee.Message, ee);
                    // RemoveClient(client);
                }
            }
        }


        /// <summary>
        /// 移除客户端
        /// </summary>
        /// <param name="key"></param>
        private void RemoveClient(string key)
        {
            if (this.clients.Keys.Contains(key))
            {
                //if (this.clients[key].Connected)
                //{
                try
                {
                    this.clients[key].Close();
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex.Message, ex);
                }
                // }
                LogUtil.Logger.Info(key + "已经断开连接！");
                this.clients.Remove(key);
            }

            if (IsCanClient(key))
            {
                FindCanClientByIp(key).OnLine = false;
            }
            if (this.clientThreads.Keys.Contains(key))
            {

                if (this.clientThreads[key].IsAlive)
                {
                    try
                    {
                        this.clientThreads[key].Abort();
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Logger.Error(ex.Message, ex);
                    }
                }
                LogUtil.Logger.Info(key + "已经断开连接！");
                this.clientThreads.Remove(key);
            }
        }


      

        private Socket GetClientByIp(string ip)
        {
            if (this.clients.Keys.Contains(ip))
            {
                return this.clients[ip];
            }
            return null;
        }

        public string GetSocketIP(Socket socket)
        {
            return socket.RemoteEndPoint.ToString().Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public bool IsCanClient(string ip)
        {
            return this.canClients.FirstOrDefault(s=>s.IP==ip)!=null;
        }


        public CanModel FindCanClient(string canId)
        {
            return this.canClients.Where(s => s.Id == canId).FirstOrDefault();
        }



        public CanModel FindCanClientByIp(string ip)
        {
            return this.canClients.Where(s => s.IP == ip).FirstOrDefault();
        }


        public bool IsCanClientAlive(string canId)
        {
            CanModel can = FindCanClient(canId);
            if(can==null)
            {
                throw new ArgumentException("CanId:"+canId+" 错误，不在配置中！");
            }
            else
            {
                return can.OnLine;
            }
           // return this.clients.Keys.Contains(this.canClients[canId]);
        }

        /// <summary>
        /// 处理收到的信息
        /// </summary>
        private void ReceiveMessageThread()
        {
            while (true)
            {
                while (receiveMessageQueue.Count > 0)
                {
                    try
                    {
                        ClientLightCmdData cmdData = receiveMessageQueue.Dequeue() as ClientLightCmdData;
                        string canId = ScaleConvertor.HexByteToDecimal(cmdData.ClientCmd[2]).ToString();
                        if (this.IsCanClientAlive(canId))
                        {
                            try
                            {
                                CanModel can = FindCanClient(canId);
                                Socket canClient = this.GetClientByIp(can.IP);

                                if (canClient != null)
                                {
                                    byte cmdType = cmdData.ClientCmd[1];
                                    int lightNum = ScaleConvertor.HexByteToDecimal(cmdData.ClientCmd[3]);
                                    string cmdTypeStr = cmdType == 0x01 ? "亮灯" : "灭灯";

                                    LogUtil.Logger.InfoFormat("【请求CAN ID:{0} - IP:{1}】【{2}】{3}数：{4}", canId, can.IP, cmdData.ClientIP, cmdTypeStr, lightNum);

                                    for (int i = 0; i < lightNum; i++)
                                    {
                                        byte id = cmdData.ClientCmd[4 + i];
                                        LogUtil.Logger.InfoFormat("【{0}】{1}数：{2}第 {3},ID: {4}", cmdData.ClientIP, cmdTypeStr, lightNum, i + 1, ScaleConvertor.HexByteToDecimal(id));

                                        byte[] lightMsg = cmdType == 0x01 ? new byte[] { 0x88, 0x00, 0x00, 0x01, 0x64, 0x01, 0xC0, 0x01, 0x00, 0x00, 0xFF, 0xFF, 0xFF } : new byte[] { 0x88, 0x00, 0x00, 0x01, 0x64, 0x01, 0xC0, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };

                                        lightMsg[4] = id;
                                     //   Thread.Sleep(500);
                                      this.SendMsgToClient(can.IP, lightMsg);

                                    }
                                    this.SendMsgToClient(cmdData.ClientIP, new byte[] { 0xAB, 0x02, 0x00, 0xFF });
                                }
                                else
                                {
                                    this.SendMsgToClient(cmdData.ClientIP, new byte[] { 0xAB, 0x03, 0x01, 0xFF });
                                }
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Logger.Error(ex.Message, ex);
                                // 反馈can 发送错误
                                this.SendMsgToClient(cmdData.ClientIP, new byte[] { 0xAB, 0x03, 0x02, 0xFF });
                            }
                        }
                        else
                        {
                            // 反馈can不在线
                            LogUtil.Logger.InfoFormat("【" + cmdData.ClientIP + "】" + "【请求灯控制CAN:{0} 不在线】", canId);
                            this.SendMsgToClient(cmdData.ClientIP, new byte[] { 0xAB, 0x03, 0x01, 0xFF });
                        }

                    }
                    catch (Exception ex)
                    {
                        LogUtil.Logger.Error(ex.Message, ex);

                    }
                }

                receivedEvent.WaitOne();
                receivedEvent.Reset();
            }
        }


        private void SendMsgToClient(string ip, byte[] msg)
        {
            try
            {
                Socket client = this.GetClientByIp(ip);
                if (client != null)
                {
                    try {
                        LogUtil.Logger.InfoFormat("【发送消息】给:{0} 内容:{1}", ip, ScaleConvertor.HexBytesToString(msg));
                        client.SendTimeout = 1000;
                        client.Send(msg, msg.Length, SocketFlags.None);
                    }
                    catch (Exception ex)
                    {
                        client.Close();
                        LogUtil.Logger.Error(ex.Message, ex);
                    }
                }
                else
                { 
                    this.RemoveClient(ip);
                    if (this.IsCanClient(ip))
                    { 
                        LogUtil.Logger.Error("【【CAN】控制器发送消息时未连接】");
                    }
                    else
                    {
                        LogUtil.Logger.Error("【客户端发送消息时未连接】");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

    }
}
