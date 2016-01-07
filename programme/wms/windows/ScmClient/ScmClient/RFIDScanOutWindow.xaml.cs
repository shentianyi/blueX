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

namespace ScmClient
{
    /// <summary>
    /// RFIDScanOutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanOutWindow : Window
    {
        private MenuWindow menuWindow; 
        private Page currentPage;

        private System.Timers.Timer timer;
        IntPtr g_selectCom = IntPtr.Zero;       //选择操作的串口句柄

        public RFIDScanOutWindow()
        {
            InitializeComponent();
        }
        public RFIDScanOutWindow(MenuWindow menuWindow)
        {
            InitializeComponent();
            this.menuWindow = menuWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.currentPage = new RFIDScanOutPage();
            NaviFrame.NavigationService.Navigate(this.currentPage);
            initTimer();
            openCom();
        }

        private void initTimer()
        {
            timer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            timer.Enabled = false;
            timer.Interval = 500;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
        }


        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentPage.Name == "RFIDScanOutPickPageName")
            {
                this.NextBtn.Visibility = Visibility.Visible;
                this.NextBtn.Content = "下一步";
                this.BackBtn.Content = "放弃";
                this.timer.Enabled = true;
                this.timer.Start();
                this.currentPage = new RFIDScanOutPage();
                NaviFrame.NavigationService.Navigate(this.currentPage);
            }
            else {
                closeWindow();
            }
        }

        private void closeWindow() {
            closeCOM();

            this.Close();
            if (this.menuWindow != null && !this.menuWindow.IsActive)
            {
                this.menuWindow.Activate();
                this.menuWindow.Show();
            }
        }
        private void closeCOM() {
            int stopReadFlag = RFIDDll.ComStopReadMultiTag(g_selectCom);
            if (stopReadFlag == RFIDDll.STOP_READ_MULITTAG_SUCCESS)
            {
                timer.Stop();
            }

            int restFlag = RFIDDll.ComResetReader(g_selectCom);

            if (restFlag != RFIDDll.COM_COMMAND_SUCCESS) { }

            Thread.Sleep(500);

            int closeFlag = RFIDDll.ComCloseCom(ref g_selectCom);
            if (closeFlag == RFIDDll.CLOSE_COM_SUCCESS)
            {
                g_selectCom = IntPtr.Zero;
                LogUtil.Logger.Info("Close COM Success");
            }
        
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            this.GoToNextPage();
        }

        public void GoToNextPage() {

            BackBtn.Content = "放弃";
            NextBtn.Visibility = Visibility.Visible;

            if (this.currentPage.Name == "RFIDScanOutPageName")
            {
                NextBtn.Content = "完成扫描";
                this.currentPage = new RFIDScanOutListPage(this);
                NaviFrame.NavigationService.Navigate(this.currentPage);
            }
            else if (this.currentPage.Name == "RFIDScanOutListPageName")
            {
                RFIDScanOutListPage listPage = (RFIDScanOutListPage)currentPage;
                if (listPage.Validate())
                {
                    NextBtn.Content = "完成出库";
                    this.currentPage = new RFIDScanOutConfirmPage(this, listPage.orderCar, listPage.orderBoxes);
                    this.StopTimer();
                    NaviFrame.NavigationService.Navigate(this.currentPage);
                }
                else
                {
                    showMessageBox("存在未通过验证料车或料盒，请初始化数据并重新扫描！");
                }
            }
            else if (this.currentPage.Name == "RFIDScanOutConfirmPageName")
            {
                RFIDScanOutConfirmPage confirmPage = (RFIDScanOutConfirmPage)currentPage;
                 confirmPage.GenereatePick();
                if (confirmPage.pick != null)
                {
                    BackBtn.Content = "返回";
                    NextBtn.Visibility = Visibility.Hidden;
                    this.currentPage = new RFIDScanOutPickPage(this,confirmPage.orderCar,confirmPage.orderBoxes,confirmPage.pick);
                    NaviFrame.NavigationService.Navigate(this.currentPage);
                }
            }
        }

        public void showMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void openCom()
        {
            IntPtr t_hCom = IntPtr.Zero;
            int openFlag = RFIDDll.ComOpenCom(ref t_hCom, "COM1", 9600);

            string m_MsgInfo = string.Empty;

            if (openFlag == RFIDDll.COM_OPEN_SUCCESS)
            {
                Thread.Sleep(500);
                LogUtil.Logger.Info("Open COM Success");

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
                        timer.Enabled = true;
                        timer.Start();
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
                LogUtil.Logger.Error("Open Com Error");
                System.Windows.MessageBox.Show("RFID启动失败，请点击返回，重启程序！");
            }
        }

        public void StopTimer()
        {
            LogUtil.Logger.Info("Stop Timer Scan");
            timer.Stop();
            timer.Enabled = false;
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

                LogUtil.Logger.Info("[接收到]"+data);

                this.Dispatcher.Invoke(DispatcherPriority.Normal, (MethodInvoker)delegate()
                {
                    LogUtil.Logger.Info(this.currentPage.Name);
                    if (this.currentPage.Name == "RFIDScanOutPageName")
                    {
                        this.currentPage = new RFIDScanOutListPage(this);
                        NaviFrame.NavigationService.Navigate(this.currentPage);
                    }
                });
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (MethodInvoker)delegate()
               {
                   if (this.currentPage.Name == "RFIDScanOutListPageName")
                   {
                       ((RFIDScanOutListPage)this.currentPage).ReceiveData(data);
                   }
               });
            }
        }

    }
}
