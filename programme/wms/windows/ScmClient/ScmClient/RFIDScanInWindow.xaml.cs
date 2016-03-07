using Brilliantech.Framwork.Utils.LogUtil;
using ScmClient.RFID;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Navigation;
using ScmWcfService.Model;
using ScmClient.Enum;
using ScmWcfService.Config;
using System.IO.Ports;
using MahApps.Metro.Controls;
using ScmWcfService.Model.Enum;
using JW.UHF;
using ScmClient.Helper; 

namespace ScmClient
{
    /// <summary>
    /// RFIDScanInWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanInWindow : MetroWindow
    {
        private MenuWindow menuWindow; 
        private Page currentPage;

        //zhaowei
        private System.Timers.Timer dllTimer;
        IntPtr g_selectCom = IntPtr.Zero;       //选择操作的串口句柄

        //weijin
        private JWReader jwReader = null;
        private object lockObj = new object();//线程同步锁
        private DateTime startTime;//启动时间 
        private Queue<Tag> inventoryTagQueue = new Queue<Tag>();//盘点到Tag队列列表
       // Dictionary<string, ListViewItem> tagList = new Dictionary<string, ListViewItem>();//Tag列表
       // UInt64 actual_read_count = 0;//实际读取数量
        private bool stopInventoryFlag = false;//是否停止盘点标志
        private delegate void UHFOperDelegate();//UHF操作跨线程委托类




        public RFIDScanType type { get; set; }
        SerialPort sp;

        public RFIDScanInWindow()
        {
            InitializeComponent();
        }
        public RFIDScanInWindow(MenuWindow menuWindow,RFIDScanType type)
        {
            InitializeComponent();
            this.menuWindow = menuWindow;
            this.type = type;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.currentPage = new RFIDScanInPage(this);
            NaviFrame.NavigationService.Navigate(this.currentPage);
            if (RFIDConfig.ReaderType==RFIDReaderType.ZhaoWei)
            {
                initTimer();
                openDllCom();
            }
            else if (RFIDConfig.ReaderType == RFIDReaderType.WeiJin) {
                #region 连接模块
                Result result = Result.OK;
                jwReader = new JWReader(RFIDConfig.RFIDCOM);

                result = jwReader.RFID_Open();//连接UHF模块

                if (result != Result.OK)
                {
                    #region 第二次尝试打开模块
                    result = jwReader.RFID_Open();
                    if (result != Result.OK)
                    {
                        LogUtil.Logger.Info("串口打开失败！");
                        showMessageBox("RFID启动失败，请重启程序！");
                    }
                    else
                    {
                        LogUtil.Logger.Info("串口打开成功！");
                    }
                    #endregion
                }
                else 
                {
                    LogUtil.Logger.Info("串口打开成功！");
                }
                #endregion

                #region 配置模块
                RfidSetting rs = new RfidSetting();
                rs.AntennaPort_List = new List<AntennaPort>();
                foreach (string index in RFIDConfig.AntennaPort)
                {
                    AntennaPort ap = new AntennaPort();
                    ap.AntennaIndex = int.Parse(index);
                    ap.Power = RFIDConfig.AntennaPower;
                    rs.AntennaPort_List.Add(ap);
                }

                rs.Inventory_Time = RFIDConfig.InventoryTime;

                rs.Region_List = RFIDConfig.RegionListType;

                rs.Speed_Mode = RFIDConfig.SpeedModeType;


                rs.Tag_Group = new TagGroup();
                rs.Tag_Group.SessionTarget = RFIDConfig.SessionTargetType; //SessionTarget.A;//A
                // rs.Tag_Group.SearchMode = SearchMode.DUAL_TARGET;//SINGLE_TARGET
                rs.Tag_Group.SearchMode = RFIDConfig.SearchModeType;//SearchMode.SINGLE_TARGET;
                //  rs.Tag_Group.Session = Session.S0;//S0
                rs.Tag_Group.Session = RFIDConfig.SessionType;//Session.S1;

                result = jwReader.RFID_Set_Config(rs);
                if (result != Result.OK)
                {
                    LogUtil.Logger.Info("RFID配置失败！");
                    showMessageBox("RFID配置失败，请重启程序！");
                }
                else {
                    LogUtil.Logger.Info("RFID配置成功！");
                }

                #endregion

                stopInventoryFlag = false;

                Thread inventoryThread = new Thread(inventory);//盘点线程
                inventoryThread.Start();

                Thread updateThread = new Thread(updateList);//更新列表线程
                updateThread.Start();
            }
            else
            {
                openCustomCom();
            }
        }

        /// <summary>
        /// 更新列表线程
        /// </summary>
        /// <param name="tags"></param>
        private void updateList()
        {
            while (!stopInventoryFlag)//未停止
            {
                updateInventoryGridList();
                Thread.Sleep(100);
            }

            DateTime dt = DateTime.Now;
            while (true)
            {
                updateInventoryGridList();
                //500毫秒内确定没有包了 防止线程提前结束 有些盘点包还没处理完 可保证该线程最后结束。
                //if (inventoryTagQueue.Count == 0 && UtilHelper.DateDiffMillSecond(DateTime.Now, dt) > 5000)
                //    break;
            }

        }

        /// <summary>
        /// 更新列表
        /// </summary>
        private void updateInventoryGridList()
        {
            UHFOperDelegate updateList = delegate()
            {
                while (inventoryTagQueue.Count > 0)
                {
                    Tag packet = inventoryTagQueue.Dequeue();
                    String epc = packet.EPC;
                    handleData(epc);

                }//while循环
            };

           // this.Invoke(updateList);
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (MethodInvoker)delegate()
                   {
                       updateList();
                   });
        }

        private void initTimer()
        {
            dllTimer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.dllTimer)).BeginInit();
            dllTimer.Enabled = false;
            dllTimer.Interval = RFIDConfig.RFIDInterVal;
            dllTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            ((System.ComponentModel.ISupportInitialize)(this.dllTimer)).EndInit();
        }


        /// <summary>
        /// 数据上报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TagsReport(object sender, TagsEventArgs args)
        {
            Tag tag = args.tag;
            inventoryTagQueue.Enqueue(tag);//回调函数事情越少越好。
        }


        /// <summary>
        /// 盘点线程
        /// </summary>
        void inventory()
        {
            //盘点开始初始化动作
            UHFOperDelegate modifyInventoryStart = delegate()
            {
                startTime = DateTime.Now;
            };

            jwReader.TagsReported += TagsReport;
            //盘点
            jwReader.RFID_Start_Inventory();

            //盘点完毕处理动作
            DateTime endTime = DateTime.Now;
            UHFOperDelegate modifyInventoryStop = delegate()
            {
                this.stopInventoryFlag = true;
            };

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            closeWindow();
            //if (this.currentPage.Name == "RFIDScanInPickPageName")
            //{
            //    this.NextBtn.Visibility = Visibility.Visible;
            //    this.NextBtn.Content = "下一步";
            //    this.BackBtn.Content = "取消";
            //    if (RFIDConfig.USE_DLL)
            //    {
            //        this.dllTimer.Enabled = true;
            //        this.dllTimer.Start();
            //    }
            //    this.currentPage = new RFIDScanInPage(this);
            //    NaviFrame.NavigationService.Navigate(this.currentPage);
            //}
            //else
            //{
            //    closeWindow();
            //}
        }

        private void closeWindow() {
            this.Close(); 
        }

        private void closeJwReader()
        {
            Result result = Result.OK;

            result = jwReader.RFID_Stop_Inventory();//停止当前UHF操作

            result = jwReader.RFID_Close();//关闭模块连接
            if (result == Result.OK)
            {
                LogUtil.Logger.Info("串口关闭成功！");
            }
            else {

                LogUtil.Logger.Error("串口关闭失败！");
            }
        }

        private void closeCustomCOM()
        {
            try
            {
                if (sp != null)
                {
                    sp.Close();
                    LogUtil.Logger.Info("串口关闭成功！");
                }
            }
            catch (Exception e)
            {
                LogUtil.Logger.Error("串口关闭失败！");
                LogUtil.Logger.Error(e.Message);
            }
        }

        private void closeDllCOM()
        {
            int stopReadFlag = RFIDDll.ComStopReadMultiTag(g_selectCom);
            if (stopReadFlag == RFIDDll.STOP_READ_MULITTAG_SUCCESS)
            {
                dllTimer.Stop();
            }

            int restFlag = RFIDDll.ComResetReader(g_selectCom);

            if (restFlag != RFIDDll.COM_COMMAND_SUCCESS) { }

            Thread.Sleep(500);

            int closeFlag = RFIDDll.ComCloseCom(ref g_selectCom);
            if (closeFlag == RFIDDll.CLOSE_COM_SUCCESS)
            {
                g_selectCom = IntPtr.Zero;
                LogUtil.Logger.Info("串口关闭成功！");
            }
        
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            this.GoToNextPage();
        }

        public void GoToNextPage()
        {

            BackBtn.Content = "取消";
            NextBtn.Visibility = Visibility.Visible;

            if (this.currentPage.Name == "RFIDScanInPageName")
            {
                NextBtn.Content = "确定";
                this.currentPage = new RFIDScanInListPage(this);
                NaviFrame.NavigationService.Navigate(this.currentPage);
            }
            else if (this.currentPage.Name == "RFIDScanInListPageName")
            {
                RFIDScanInListPage listPage = (RFIDScanInListPage)currentPage;
                if (listPage.Validate())
                {
                    this.StopTimer();

                    if (this.type == RFIDScanType.IN)
                    {
                        NextBtn.Content = "生成择货";

                    }
                    else if (this.type == RFIDScanType.OUT)
                    {
                        NextBtn.Content = "确定";
                    }

                    this.currentPage = new RFIDScanInConfirmPage(this, listPage.orderCar, listPage.orderBoxes);
                    NaviFrame.NavigationService.Navigate(this.currentPage);

                }
                else
                {
                    showMessageBox("料车或料盒未扫描完整，请继续或重新扫描！");
                }
            }
            else if (this.currentPage.Name == "RFIDScanInConfirmPageName")
            {
                RFIDScanInConfirmPage confirmPage = (RFIDScanInConfirmPage)currentPage;
                if (this.type == RFIDScanType.IN)
                {
                    new RFIDDoor().OpenDoor();
                    confirmPage.GenereatePick();
                    if (confirmPage.pick != null && confirmPage.canNext)
                    {
                       // this.Close();
                        BackBtn.Content = "返回";
                        NextBtn.Visibility = Visibility.Hidden;
                        this.currentPage = new RFIDScanInPickPage(this, confirmPage.orderCar, confirmPage.orderBoxes, confirmPage.pick);
                        NaviFrame.NavigationService.Navigate(this.currentPage);
                    }
                }
                else if (this.type == RFIDScanType.OUT)
                {
                    confirmPage.MoveStroage();
                    if (confirmPage.canNext)
                    {
                        new RFIDDoor().OpenDoor();

                        this.Close();
                        //BackBtn.Content = "取消";
                        //this.NextBtn.Visibility = Visibility.Visible;
                        //this.NextBtn.Content = "下一步";
                        //this.currentPage = new RFIDScanInPage(this);
                        //NaviFrame.NavigationService.Navigate(this.currentPage);
                        //this.dllTimer.Enabled = true;
                        //this.dllTimer.Start();
                    }
                }
            }
        }

        public void showMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void openCustomCom() 
        {
            try
            {
                sp = new SerialPort(RFIDConfig.RFIDCOM, RFIDConfig.RFIDBaudRate);
                sp.Open();
                sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);

                LogUtil.Logger.Info("串口打开成功！");
            }
            catch (Exception e) {
                if (sp.IsOpen) {
                    sp.Close();
                }

                LogUtil.Logger.Error("串口打开失败！");
                LogUtil.Logger.Error(e.Message);
            }
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] recvbuf = new Byte[17];
            sp.Read(recvbuf, 0, recvbuf.Length);
            string data = string.Empty;
            foreach (byte b in recvbuf)
            {
                data += b.ToString("X0").PadLeft(2, '0');
            }

            LogUtil.Logger.Info(recvbuf);

            LogUtil.Logger.Info(data); 
            if (recvbuf.Length != 0 && recvbuf.Length < 40960)
            {
                handleData(data);
            }
        }

        private void handleData(string data) 
        {
            LogUtil.Logger.Info("[接收到]" + data);
            List<RFIDMessage> messages = new List<RFIDMessage>();
            messages = Parser.StringToList(data);
            if (messages.Count > 0)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (MethodInvoker)delegate()
                {
                    LogUtil.Logger.Info(this.currentPage.Name);
                    if (this.currentPage.Name == "RFIDScanInPageName")
                    {
                        this.currentPage = new RFIDScanInListPage(this);
                        NaviFrame.NavigationService.Navigate(this.currentPage);
                    }
                    if (this.currentPage.Name == "RFIDScanInListPageName")
                    {
                        ((RFIDScanInListPage)this.currentPage).ReceiveData(messages);
                    }
                });
            }
        }

        private void openDllCom()
        {
            IntPtr t_hCom = IntPtr.Zero;
            int openFlag = RFIDDll.ComOpenCom(ref t_hCom,RFIDConfig.RFIDCOM, RFIDConfig.RFIDBaudRate);

            string m_MsgInfo = string.Empty;

            if (openFlag == RFIDDll.COM_OPEN_SUCCESS)
            {
                Thread.Sleep(500);
                LogUtil.Logger.Info("串口打开成功！");

                Byte[] recvbuf = new Byte[40];
                int flag = RFIDDll.ComReadVersion(t_hCom, recvbuf);
                if (flag == RFIDDll.READ_VERSION_SUCCESS)
                {
                    g_selectCom = t_hCom;

                    m_MsgInfo = "打开串口" + "COM1" + "成功 ，软件版本为:V" + recvbuf[0].ToString() + "." + recvbuf[1].ToString();

                    int readFlag = RFIDDll.ComStratReadMultiTag(g_selectCom);
                    if (readFlag == RFIDDll.READ_MULIT_TAG_SUCCESS)
                    {
                        LogUtil.Logger.Info("Read Multi COM Success");
                        LogUtil.Logger.Info(m_MsgInfo);
                        dllTimer.Enabled = true;
                        dllTimer.Start();
                    }
                    else {
                        LogUtil.Logger.Error("Read COM Error");
                        System.Windows.MessageBox.Show("RFID启动失败，请重启程序！");
                    }

                }
                else
                {
                    m_MsgInfo = "打开串口" + "COM1" + "失败：读版本失败";
                    LogUtil.Logger.Error(m_MsgInfo);
                    System.Windows.MessageBox.Show("RFID启动失败，请重启程序！");
                }
            }
            else {
                LogUtil.Logger.Error("串口打开失败！");
                System.Windows.MessageBox.Show("RFID启动失败，请点击返回，重启程序！");
            }
        }

        public void StopTimer()
        {
            if (RFIDConfig.ReaderType==RFIDReaderType.ZhaoWei)
            {
                LogUtil.Logger.Info("Stop Timer Scan");
                dllTimer.Stop();
                dllTimer.Enabled = false;
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //按显示的每条数据长度为50个字节计算的话，该数组最多可存储约40960/50 = 819张不同标签
            Byte[] recvbuf = new Byte[40960];
             
            int flag = 0;
              
            flag = RFIDDll.ComGetMultiTagBuf_Ex(recvbuf);

            if (flag == RFIDDll.GET_TAG_DATA_SUCCESS)
            {
                string data = System.Text.Encoding.Default.GetString(recvbuf);
                handleData(data);
                //LogUtil.Logger.Info("[接收到]" + data);

                //List<RFIDMessage> messages = new List<RFIDMessage>();
                //messages = Parser.StringToList(data);
                //if (messages.Count > 0)
                //{
                //    this.Dispatcher.Invoke(DispatcherPriority.Normal, (MethodInvoker)delegate()
                //    {
                //        LogUtil.Logger.Info(this.currentPage.Name);
                //        if (this.currentPage.Name == "RFIDScanInPageName")
                //        {
                //            this.currentPage = new RFIDScanInListPage(this);
                //            NaviFrame.NavigationService.Navigate(this.currentPage);
                //        } 
                //       if (this.currentPage.Name == "RFIDScanInListPageName")
                //       {
                //           ((RFIDScanInListPage)this.currentPage).ReceiveData(messages);
                //       }
                //    });
                //}
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (RFIDConfig.ReaderType == RFIDReaderType.ZhaoWei)
            {
                closeDllCOM();
                Thread.Sleep(500);
            }
            else if (RFIDConfig.ReaderType == RFIDReaderType.WeiJin) {
                closeJwReader();
            }
            else
            {
                closeCustomCOM();
            }
            new MenuWindow().Show();
        }

    }
}
