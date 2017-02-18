using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Brilliantech.Framwork.Utils.ConvertUtil;
using Brilliantech.Framwork.Utils.LogUtil;
using PLCLightCL.Enum;

namespace PLCLightCL.Light
{
    public class CanLightController : LightController
    {

        const int _lightCount = 255;
         
        Socket tcpClient;
        private string serverIP;
        private int serverPort;
        private int canId;

        public CanLightController(string serverIP, int serverPort, int canId)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            this.canId = canId;
            this.lightCount = _lightCount;
            this.Open();
        }
        Thread ClientRecieveThread;
        protected override void Open(bool throwEx = true)
        {
            tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse(serverIP), serverPort));


            ClientRecieveThread = new Thread(Listen);
            ClientRecieveThread.IsBackground = true;
            ClientRecieveThread.Start();
        }

        public override void Play(LightCmdType cmdType, List<int> indexes = null)
        {
            //try
            //{
            indexes = this.ValidateIndexes(cmdType, indexes);

            byte cmdT = 0x00;
            switch (cmdType)
            {
                case LightCmdType.OFF:
                    cmdT = 0x00;
                    break;
                case LightCmdType.ON:
                    cmdT = 0x01;
                    break;
                case LightCmdType.ALL_ON:
                    cmdT = 0x01;
                    if (indexes == null)
                    {
                        indexes = new List<int>();
                    }
                    indexes.Clear();
                    for (var i = 0; i < lightCount; i++)
                    {
                        indexes.Add(i);
                    }
                    break;
                case LightCmdType.ALL_OFF:
                    cmdT = 0x00;
                    if (indexes == null)
                    {
                        indexes = new List<int>();
                    }
                    indexes.Clear();
                    for (var i = 0; i < lightCount; i++)
                    {
                        indexes.Add(i);
                    }
                    break;
                case LightCmdType.ALL_OFF_BEFORE_ON:
                    this.Play(LightCmdType.ALL_OFF);
                    Thread.Sleep(100);
                    this.Play(LightCmdType.ON, indexes);
                    return;
                default:
                    break;
            }

            byte[] msg = new byte[5 + indexes.Count];
            msg[0] = 0xAB;
            msg[1] = cmdT;
            msg[2] = ScaleConvertor.DecimalToByte((ushort)this.canId)[1];
            msg[3] = ScaleConvertor.DecimalToByte((ushort)indexes.Count)[1];
            msg[msg.Length - 1] = 0xFF;
            for (var i = 0; i < indexes.Count; i++)
            {
                msg[4 + i] = ScaleConvertor.DecimalToByte((ushort)indexes[i])[1];
            }

            tcpClient.Send(msg, msg.Length, SocketFlags.None);
            //}
            //catch (Exception ex)
            //{
            //    LogUtil.Logger.Error(ex.Message, ex);
            //}
        }


        public override void Close()
        {
            this.tcpClient.Close();
        }

        bool runflag = true;

        private void Listen()
        {

            while (runflag)
            {
                try
                {
                    //固定字节数13
                    byte[] result = new byte[1000];
                    int dataLength = tcpClient.Receive(result);
                     
                    tcpClient.ReceiveTimeout = -1;
                    byte[] msg = result.Take(dataLength).ToArray();

                    LogUtil.Logger.InfoFormat("【【接到服务器消息】】 内容:{0}",
                        ScaleConvertor.HexBytesToString(msg)); 

                }
                catch (SocketException ex)
                {
                    LogUtil.Logger.Error(ex.Message, ex);
                }
                catch (Exception ex)
                {
                  
                    runflag = false;
                }

            }
        }


    }
}
