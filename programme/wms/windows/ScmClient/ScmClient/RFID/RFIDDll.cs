using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ScmClient.RFID
{
    public class RFIDDll
    {
        public const int RESULT_NULL = 0;
        public const int RESULT_COM = 1;
        public const int RESULT_SOCKET = 2;


        public const int CHOOSE_COM = 0;
        public const int CHOOSE_CLIENT = 1;
        public const int CHOOSE_SERVER = 2;

        //宏定义
        public const int COM_OPEN_SUCCESS = 3;
        public const int CLOSE_COM_SUCCESS = 4;
        public const int COM_COMMAND_SUCCESS = 5;
        public const int START_SERVER_SUCCESS = 10;
        public const int CLOSE_SERVER_SUCCESS = 11;
        public const int START_CLIENT_SUCCESS = 12;
        public const int CLOSE_CLIENT_SUCCESS = 13;
        public const int CLIENT_RESET_SUCCESS = 14;
        public const int READ_SINGLE_TAG_SUCCESS = 15;
        public const int STOP_READ_TAG_SUCCESS = 16;
        public const int READ_MULIT_TAG_SUCCESS = 17;
        public const int START_READ_TAG_SUCCESS = 18;
        public const int GET_TAG_DATA_SUCCESS = 19;
        public const int STOP_READ_MULITTAG_SUCCESS = 20;
        public const int READ_VERSION_SUCCESS = 21;
        public const int STOP_WORKSET_SUCCESS = 22;
        public const int QUERY_SINGLE_PAR_SUCCESS = 23;
        public const int QUERY_MULIT_PAR_SUCCESS = 24;
        public const int SET_SINGLE_PAR_SUCCESS = 25;
        public const int SET_MULIT_PAR_SUCCESS = 26;
        public const int INIT_TAG_SUCCESS = 27;
        public const int TAG_LOCK_SUCCESS = 28;
        public const int TAG_KILL_SUCCESS = 29;
        public const int TAG_WRITE_WORD_SUCCESS = 30;
        public const int TAG_READ_CONTENT_SUCCESS = 31;
        public const int AFRESH_IDENTIFY_SUCCESS = 32;
        public const int CHECK_PASSWORD_SUCCESS = 33;
        public const int OPERATION_PWD_SUCCESS = 34;
        public const int DEL_AUTH_ID_SUCCESS = 35;
        public const int LOADER_RESET_SUCCESS = 36;
        public const int LOADER_BAUD_SUCCESS = 37;
        public const int LOADER_ALLLEN_SUCCESS = 38;
        public const int LOADER_SENDDATA_SUCCESS = 39;
        public const int LOADER_END_SUCCESS = 40;
        public const int TAG_6B_WRITE_SUCCESS = 41;
        public const int TAG_6B_READ_SUCCESS = 42;
        public const int SET_IP_PORT_SUCCESS = 43;
        public const int SET_DATA_SUCCESS = 44;
        public const int SET_TIME_SUCCESS = 45;
        public const int READ_TIME_SUCCESS = 46;
        public const int SET_MAC_SUCCESS = 47;
        public const int SET_DHCP_SUCCESS = 48;
        public const int SET_RELAY_SUCCESS = 49;
        public const int SET_RELAY_TIME_SUCCESS = 50;
        public const int GPIO_STATE_SUCCESS = 51;
        public const int QUERY_ARRAY_SUCCESS = 52;
        public const int ACTIVE_RESPONE_SUCCESS = 53;
        public const int DATA_ACQ_SUCCESS = 54;
        public const int DEL_DATA_ACQ_SUCCESS = 55;
        public const int TAG_ACCESS_TWO_SUCCESS = 56;
        public const int TAG_6B_LOCK_SUCCESS = 57;
        public const int LOADER_AUTH_ID_SUCCESS = 58;

        //Error
        public const int START_SERVER_ERROR = -10;
        public const int CLOSE_SERVER_ERROR = -11;
        public const int START_CLIENT_ERROR = -12;
        public const int CLOSE_CLIENT_ERROR = -13;
        public const int CLIENT_RESET_ERROR = -14;
        public const int READ_SINGLE_TAG_ERROR = -15;
        public const int STOP_READ_TAG_ERROR = -16;
        public const int READ_MULIT_TAG_ERROR = -17;
        public const int START_READ_TAG_ERROR = -18;
        public const int GET_TAG_DATA_ERROR = -19;
        public const int STOP_READ_MULITTAG_ERROR = -20;
        public const int READ_VERSION_ERROR = -21;
        public const int STOP_WORKSET_ERROR = -22;
        public const int QUERY_SINGLE_PAR_ERROR = -23;
        public const int QUERY_MULIT_PAR_ERROR = -24;
        public const int SET_SINGLE_PAR_ERROR = -25;
        public const int SET_MULIT_PAR_ERROR = -26;
        public const int INIT_TAG_ERROR = -27;
        public const int TAG_LOCK_ERROR = -28;
        public const int TAG_KILL_ERROR = -29;
        public const int TAG_WRITE_WORD_ERROR = -30;
        public const int TAG_READ_CONTENT_ERROR = -31;
        public const int AFRESH_IDENTIFY_ERROR = -32;
        public const int CHECK_PASSWORD_ERROR = -33;
        public const int OPERATION_PWD_ERROR = -34;
        public const int DEL_AUTH_ID_ERROR = -35;
        public const int LOADER_RESET_ERROR = -36;
        public const int LOADER_BAUD_ERROR = -37;
        public const int LOADER_ALLLEN_ERROR = -38;
        public const int LOADER_SENDDATA_ERROR = -39;
        public const int LOADER_END_ERROR = -40;
        public const int TAG_6B_WRITE_ERROR = -41;
        public const int TAG_6B_READ_ERROR = -42;
        public const int SET_IP_PORT_ERROR = -43;
        public const int SET_DATA_ERROR = -44;
        public const int SET_TIME_ERROR = -45;
        public const int READ_TIME_ERROR = -46;
        public const int SET_MAC_ERROR = -47;
        public const int SET_DHCP_ERROR = -48;
        public const int SET_RELAY_ERROR = -49;
        public const int SET_RELAY_TIME_ERROR = -50;
        public const int GPIO_STATE_ERROR = -51;
        public const int QUERY_ARRAY_ERROR = -52;
        public const int ACTIVE_RESPONE_ERROR = -53;
        public const int DATA_ACQ_ERROR = -54;
        public const int DEL_DATA_ACQ_ERROR = -55;
        public const int TAG_ACCESS_TWO_ERROR = -56;
        public const int TAG_6B_LOCK_ERROR = -57;
        public const int LOADER_AUTH_ID_ERROR = -58;


        //  串口

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern Byte CharToHex(byte ch);

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComOpenCom(ref IntPtr hCom, string ComPort, int BaudRate);//打开串口

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComCloseCom(ref IntPtr hCom);//关闭串口

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComReadVersion(IntPtr hCom, byte[] RecvBuf);//读版本

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComTagIdentify(IntPtr hCom, int TagType, byte[] Recvbuf);//单标签识别

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComResetReader(IntPtr hCom);//复位读写器

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComStopReadTag(IntPtr hCom);//停止读卡

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComStartReadTag(IntPtr hCom);//启动读卡

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComAfreshTag(IntPtr hCom);//重新识别卡（多卡有效）

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComAfreshGetData(IntPtr hCom);//重新获取数据

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetBaudRate(IntPtr hCom, int BaudRateType);//设置波特率

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComStopWork(IntPtr hCom);//读写器停止工作

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComQueryMultiParameter(IntPtr hCom, int Length, int Lsb, byte[] RecvBuf);//查询多个参数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComQuerySingleParameter(IntPtr hCom, int Lsb, byte[] RecvBuf);//查询单个参数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetMultiParameter(IntPtr hCom, int Length, int Lsb, byte[] WriteData);//设置多个参数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetSingleParameter(IntPtr hCom, int Lsb, byte WriteData);//设置单个参数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComStratReadMultiTag(IntPtr hCom);//启动读多卡

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComGetMultiTagBuf(byte[] dataBuf);//获取读多卡数据

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComGetMultiTagBuf_Ex(byte[] dataBuf);//获取读多卡数据，读取一次清空一次标签数据

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComStopReadMultiTag(IntPtr hCom);//停止读多卡

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComCheckPassWorld(IntPtr hCom, int Type, byte[] RecvBuf);//查询密码

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComOperationPassWorld(IntPtr hCom, int Type, byte[] RecvBuf);//密码操作

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComLoadAuthoId(IntPtr hCom, byte[] SendData);// 导入授权ID

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComDeleAuthoId(IntPtr hCom);// 删除所有授权ID

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetIpPort(IntPtr hCom, byte[] IP, int Port);// 设置IP和端口

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetData(IntPtr hCom, byte[] WriteData);// 设置日期

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetTime(IntPtr hCom, byte[] WriteData);// 设置时间

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComReadTime(IntPtr hCom, byte[] RecvBuf);// 读取日期时间

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetMAC(IntPtr hCom, byte[] RecvBuf);// 设置MAC地址

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetHDCP(IntPtr hCom);// DHCP有效

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComSetRelay(IntPtr hCom, byte ID, byte Relay, byte Mtime);// 设置继电器

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComIOPortState(IntPtr hCom, byte[] RecvBuf);// 设置IO口状态

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComQueryArray(IntPtr hCom, byte[] RecvBuf); //查询存储数据的数组

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComActiveResponse(IntPtr hCom, byte[] RecvBuf); // 应答包

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComDataAcquisition(IntPtr hCom, byte count, byte[] RecvBuf); // 数据采集

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComReadTagContent(IntPtr hCom, int MemBank, int Address, int Length, byte[] RecvBuf); // 读标签内容

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComInitTag(IntPtr hCom);   // 初始化标签

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComTagLock(IntPtr hCom, byte LockType);   // 锁标签

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComTagKill(IntPtr hCom, int[] PassWord);  //销毁标签

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComTagWriteWord(IntPtr hCom, int WriteMode, int MemBank, int Address, int Length, byte[] Data);//写入标签内容

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComTagWriteWord_Ex(IntPtr hCom, byte cmd, byte type, int WriteMode, int MemBank, int Address, int Length, byte[] Data);//写入标签内容

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComReadTag6BContent(IntPtr hCom, byte Address, byte Length, byte[] RecvBuf);//读取6B标签内容

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComTag6BLock(IntPtr hCom, byte Address);   //锁6B标签

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int ComTag6BWriteWord(IntPtr hCom, int Address, int Length, byte[] Data);//写入6B标签


        //网口的
        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern void SocketGetSockByAddr(ref uint GetSock, byte[] Address);//查找ip地址对应的socket值

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern bool SocketGetAddrBySock(uint sock, byte[] GetAddress);//查找socket值对应的ip地址

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern bool SocketGetMulitClientSocket(uint[] sock);//获取存储客户端Socket的数组指针

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern bool SocketMulitSocketClient(String ServerIp, int ServerPort);	 //多客户端函数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketStartClient(ref uint cliSockArray, string ip, int ClientPort);

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketCloseClient(uint[] serSockArray, int serverCount);

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketStartServer(uint[] serSockArray, ref int clientCount, int SerPort);

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketCloseServer(uint[] serSockArray, int clientCount);

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern bool SocketOutTime(uint sock, uint Wtime, uint Etime, bool Openflag);

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketClearBuffer(uint sock);

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketReadVersion(ref uint sock, byte[] recvBuf);//读版本号

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketStopWorkSetting(ref uint sock);//功率停止

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketResetReader(ref uint sock);//复位

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketQuerySingleParameter(ref uint sock, Byte Lsb, byte[] recvBuf);//查询读写器的单个参数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketQueryMultiParameter(ref uint sock, byte Length, byte Lsb, byte[] recvBuf);//查询读写器多个参数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketSetSingleParameter(ref uint sock, byte Lsb, byte Data);//设置读写器单个参数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketSetMultiParameter(ref uint sock, byte Length, byte Lsb, byte[] writeData);//设置读写器多个参数

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketTagIdentify(ref uint sock, byte TagType, byte[] recvBuf);//标签识别

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketAfreshIdentifyTag(ref uint sock);//重新识别

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketStratReadTag(ref uint sock);//启动标签读取

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketStopReadTag(ref uint sock);//停止标签读取

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketStartReadMultiTag(uint[] serSockArray, int clientCount, bool isDivDevice = false);//启动读多卡

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketStopReadMultiTag(uint[] serSockArray, int clientCount);//停止读多卡

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketGetMultiTagBuf(byte[] dataBuf, bool isDivDevice = false);//获取读多卡的数据

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketGetMultiTagBuf_Ex(byte[] dataBuf, bool isDivDevice = false);//获取读多卡的数据

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketCheckPassWorld(ref uint sock, byte Type, byte[] RecvBuf);//查询密码

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketOperationPassWorld(ref uint sock, byte Type, byte[] RecvBuf);//密码操作

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketLoadAuthoId(ref uint sock, byte[] SendData);// 导入授权ID

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketDeleAuthoId(ref uint sock);// 删除所有授权ID

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketSetIpPort(ref uint sock, byte[] IP, int Port); //设置IP和端口

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketSetData(ref uint sock, byte[] WriteData); //设置日期

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketSetTime(ref uint sock, byte[] WriteData); //设置时间

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketReadTime(ref uint sock, byte[] RecvBuf); //读取日期

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketSetMAC(ref uint sock, byte[] RecvBuf); //设置MAC地址

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketSetHDCP(ref uint sock); //DHCP有效

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketSetRelay(ref uint sock, byte ID, byte Replay, byte Mtime); //设置继电器

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketIOPortState(ref uint sock, byte[] RecvBuf); //设置IO口状态

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketQueryArray(ref uint sock, byte[] RecvBuf); //查询存储数据的数组

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketActiveResponse(ref uint sock, byte[] RecvBuf); //应答包

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketDataAcquisition(ref uint sock, byte count, byte[] RecvBuf); //数据采集

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketReadTagContent(ref uint sock, int MemBank, int Address, int Length, byte[] RecvBuf); // 读标签内容

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern bool SocketHeartbeatpacket(ref uint sock);//心跳包

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketInitTag(ref uint sock); //初始化标签

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketTagLock(ref uint sock, byte LockType);   // 锁标签

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketTagKill(ref uint sock, int[] PassWord);  //销毁标签

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketTagWriteWord(ref uint sock, int WriteMode, int MemBank, int Address, int Length, byte[] Data); //写入标签内容

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketTagWriteWord_Ex(ref uint sock, byte cmd, byte type, int WriteMode, int MemBank, int Address, int Length, byte[] Data); //写入标签内容

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketRead6BTagContent(ref uint sock, byte Address, byte Length, byte[] RecvBuf);//读取6B标签内容

        [DllImport("ThridPart\\EpcDll.dll")]
        public static extern int SocketTag6BWriteWord(ref uint sock, int Address, int Length, byte[] Data);//写入6B标签
    }
}
