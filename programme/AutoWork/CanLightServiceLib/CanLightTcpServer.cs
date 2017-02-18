
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
        public static int MaxSendCount=0;
        public ClientLightCmdData()
        {
            SentCount = 0;
        }
        public string ClientIP { get; set; }
        public byte[] ClientCmd { get; set; }
        public int SentCount { get; set; }
        public bool ShouldResend
        {
            get
            {
                return this.SentCount < MaxSendCount;
            }
        }
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

        public Action<string> AcceptNewClientAction;
        public Action<string> LostClientAction;
        public Action<string> ReceiveClientMessageAction;
        //public Action
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

                server.Listen(100);
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
                    string ipp = GetSocketIPAndPort(client);
                    bool isCan = this.IsCanClientByIp(tempIp);
                    if (isCan)
                    {
                        RemoveClient(ipp);
                        // client.ReceiveTimeout = 10000;
                        LogUtil.Logger.Info("请求连接的【CAN】控制器IP&Port：" + ipp);
                        // 将ip作为key,socket作为value存入字典中
                        clients.Add(ipp, client);
                        FindCanClientByKey(ipp).OnLine = true;
                    }
                    else
                    {
                        RemoveClient(ipp);
                        LogUtil.Logger.Info("请求连接的客户端IP：" + ipp);
                        // 将ip作为key,socket作为value存入字典中
                        clients.Add(ipp, client);
                    }
                    //  LogUtil.Logger.Info("移除字典item成功！");
                    //}


                    LogUtil.Logger.Info(string.Format("【{0}】已连接!", ipp));

                    // 启动监听消息线程 
                    Thread listenMsgTH = new Thread(ReceiveMsg);
                    receiveMessageQueue = Queue.Synchronized(comDataQ);

                    listenMsgTH.IsBackground = true;
                    listenMsgTH.Start(client);
                    if (isCan) {
                        clientThreads.Add(ipp, listenMsgTH);
                    } else {
                      //  clientThreads.Add(tempIp, listenMsgTH);

                        clientThreads.Add(ipp, listenMsgTH);
                        // 将子线程加入线程池中
                    }
                    AcceptClient(ipp);
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

                LogUtil.Logger.Info("【停止服务】");
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
            string ipp = GetSocketIPAndPort(client);
            bool isCan = IsCanClientByIp(clientIP);

            while (true && clientIP!=null)
            {
                try
                {
                    if ((!clients.Keys.Contains(ipp)) || (!client.Connected))
                    {
                        break;
                    }

                    //if ((!clients.Keys.Contains(clientIP)) || (!client.Connected))
                    //{
                    //    break;
                    //}
                    byte[] result = new byte[1000];
                    int dataLength = client.Receive(result);

                    byte[] message = result.Take(dataLength).ToArray();
                    string messageStr = ScaleConvertor.HexBytesToString(message);

                    if (dataLength > 0)
                    {
                        if (!this.IsCanClientByKey(ipp))
                        {
                            LogUtil.Logger.InfoFormat("【接收到客户端:{0} 信息】{1}", ipp, ScaleConvertor.HexBytesToString(message));
                            LogUtil.Logger.Info("【" + ipp + "】" + "来自【请求灯控制】的消息");
                            ReceiveMessage(string.Format("【接收到客户端:{0} 信息】{1}", ipp, ScaleConvertor.HexBytesToString(message)));
                            // 控制灯灭或灯亮
                            if (message.Length > 5)
                            {
                                if (message[0] == 0xAB && message[message.Length - 1] == 0xFF)
                                {
                                    ClientLightCmdData cmd = new ClientLightCmdData()
                                    {
                                        ClientIP = ipp,
                                        ClientCmd = message
                                    };
                                    this.receiveMessageQueue.Enqueue(cmd);
                                    this.receivedEvent.Set();

                                }
                                else
                                {
                                    LogUtil.Logger.Info("【" + ipp + "】" + "来自【请求灯控制】的消息--头尾不符合");
                                }
                            }
                            else
                            {
                                LogUtil.Logger.Info("【" + ipp + "】" + "来自【请求灯控制】的消息--长度不符合");
                            }
                        }
                        else
                        {
                            FindCanClientByKey(ipp).OnLine = true;
                            LogUtil.Logger.InfoFormat("【接收到 CAN:{0} 信息】{1}", ipp, ScaleConvertor.HexBytesToString(message));
                            // 暂时不处理
                        }

                    }

                }
                catch (ObjectDisposedException exx)
                {
                    RemoveClient(ipp);
                    //if (isCan)
                    //{
                    //    RemoveClient(ipp);
                    //}
                    //else
                    //{
                    //    RemoveClient(clientIP);
                    //}
                    LogUtil.Logger.Error(exx.Message, exx);
                    break;
                }
                catch (SocketException exxx)
                {
                    RemoveClient(ipp);
                    //if (isCan)
                    //{
                    //    RemoveClient(ipp);
                    //}
                    //else
                    //{
                    //    RemoveClient(clientIP);
                    //}
                    LogUtil.Logger.Error(exxx.Message, exxx);
                    break;
                }
                catch (Exception ee)
                {
                    RemoveClient(ipp);
                    //if (isCan)
                    //{
                    //    RemoveClient(ipp);
                    //}
                    //else
                    //{
                    //    RemoveClient(clientIP);
                    //}
                    LogUtil.Logger.Error(ee.Message, ee);
                    break;
                }
            }
        }

        private void LostClient(string clientIP)
        {
            try
            {
                if (LostClientAction != null)
                {
                    LostClientAction(clientIP);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        private void AcceptClient(string clientIp)
        {
            try
            {
                if (AcceptNewClientAction != null)
                {
                    AcceptNewClientAction(clientIp);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        public void ReceiveMessage(string msg)
        {
            try
            {
                if (ReceiveClientMessageAction != null)
                {
                    ReceiveClientMessageAction(msg);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
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
                    Socket client = this.clients[key];
                    LostClient(this.GetSocketIPAndPort(client));
                    client.Close();
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex.Message, ex);
                }
                // }
                LogUtil.Logger.Info(key + "已经断开连接！");
                this.clients.Remove(key);
            }

            if (IsCanClientByKey(key))
            {
                FindCanClientByKey(key).OnLine = false;
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


      

        private Socket GetClientByKey(string key)
        {
            if (this.clients.Keys.Contains(key))
            {
                return this.clients[key];
            }
            return null;
        }

        public string GetSocketIP(Socket socket)
        {
            try {
                return socket.RemoteEndPoint.ToString().Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetSocketIPAndPort(Socket socket)
        {
            try
            {
                return socket.RemoteEndPoint.ToString();

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool IsCanClientByIp(string ip)
        {
            return this.canClients.FirstOrDefault(s=>s.IP== ip) !=null;
        }
        public bool IsCanClientByKey(string key)
        {
            return this.canClients.FirstOrDefault(s => s.UniqKey == key) != null;
        }

        public CanModel FindCanClientById(string canId)
        {
            return this.canClients.Where(s => s.Id == canId).FirstOrDefault();
        }



        public CanModel FindCanClientByKey(string key)
        {
            return this.canClients.Where(s => s.UniqKey == key).FirstOrDefault();
        }


        public bool IsCanClientAlive(string canId)
        {
            CanModel can = FindCanClientById(canId);
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
                                CanModel can = FindCanClientById(canId);
                        byte cmdType = cmdData.ClientCmd[1];
                                int lightNum = ScaleConvertor.HexByteToDecimal(cmdData.ClientCmd[3]);
                                string cmdTypeStr = cmdType == 0x01 ? "亮灯" : "灭灯";
                               
                                LogUtil.Logger.InfoFormat("【第{5}次】【请求CAN ID:{0} - IP:{1}】【{2}】{3}数：{4}", canId, can.IP, cmdData.ClientIP, cmdTypeStr, lightNum, cmdData.SentCount);

                        if (this.IsCanClientAlive(canId))
                        {
                            try
                            {
                                Socket canClient = this.GetClientByKey(can.UniqKey);
                               
                                if (canClient != null)
                                {
                                   bool sendOk = true;

                                    for (int i = 0; i < lightNum; i++)
                                    {
                                        byte id = cmdData.ClientCmd[4 + i];
                                        LogUtil.Logger.InfoFormat("【{0}】{1}数：{2}第 {3},ID: {4}", cmdData.ClientIP, cmdTypeStr, lightNum, i + 1, ScaleConvertor.HexByteToDecimal(id));

                                        byte[] lightMsg = cmdType == 0x01 ? new byte[] { 0x88, 0x00, 0x00, 0x01, 0x64, 0x01, 0xC0, 0x01, 0x00, 0x00, 0xFF, 0xFF, 0xFF } : new byte[] { 0x88, 0x00, 0x00, 0x01, 0x64, 0x01, 0xC0, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };

                                        lightMsg[4] = id;
                                        //   Thread.Sleep(500);
                                        if (!this.SendMsgToClient(can.UniqKey, lightMsg))
                                        {
                                            sendOk = false;
                                        }

                                    }
                                    if (sendOk == false && cmdData.ShouldResend)
                                    {
                                        cmdData.SentCount++;
                                        receiveMessageQueue.Enqueue(cmdData);
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
                            if (cmdData.ShouldResend)
                            {
                                cmdData.SentCount++;
                                receiveMessageQueue.Enqueue(cmdData);
                            }
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


        private bool SendMsgToClient(string key, byte[] msg)
        {
            bool ok = false;
            try
            {
                Socket client = this.GetClientByKey(key);
                if (client != null)
                {
                    try {
                        LogUtil.Logger.InfoFormat("【发送消息】给:{0} 内容:{1}", key, ScaleConvertor.HexBytesToString(msg));
                        client.SendTimeout = 1000;
                        client.Send(msg, msg.Length, SocketFlags.None);
                        ok = true;
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        client.Close();
                        LogUtil.Logger.Error(ex.Message, ex);
                    }
                }
                else
                {
                    ok = false;
                    this.RemoveClient(key);
                    if (this.IsCanClientByKey(key))
                    { 
                        LogUtil.Logger.Error("【【CAN】控制器发送消息时未连接】");
                    }
                    else
                    {
                        ok = false;
                        LogUtil.Logger.Error("【客户端发送消息时未连接】");
                    }
                }
            }
            catch (Exception ex)
            {
                ok = false;
                LogUtil.Logger.Error(ex.Message, ex);
            }
            return ok;
        }

    }
}
