﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Brilliantech.Framwork.Utils.LogUtil;
using System.Text.RegularExpressions;
using ScmClient.RFID;
using System.Windows.Threading;
using System.Windows.Forms;
using ScmWcfService.Model;
using ScmWcfService;
using ScmWcfService.Model.Message;

namespace ScmClient
{
    /// <summary>
    /// RFIDScanInListPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanInListPage : Page
    {
        RFIDScanInWindow parentWindow;
        bool showMultiCarFlag = false;

        List<RFIDMessage> carMsgList = new List<RFIDMessage>();
        List<RFIDMessage> boxMsgList = new List<RFIDMessage>();

        public OrderCar orderCar { get; set; }
        public List<OrderBox> orderBoxes { get; set; }


        public RFIDScanInListPage()
        {
            InitializeComponent();
        }

        public RFIDScanInListPage(RFIDScanInWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.orderBoxes = new List<OrderBox>();
        }
        public void ReceiveData(List<RFIDMessage> messages)
        {
            List<RFIDMessage> carMsgs = (from msg in messages where msg.Type == MessageType.CAR select msg).ToList();
            List<RFIDMessage> boxMsgs = (from msg in messages where msg.Type == MessageType.BOX select msg).ToList();

            addCarMessages(carMsgs);
            addBoxMessages(boxMsgs);
        }

        //public void ReceiveData(string data)
        //{
        //    List<RFIDMessage> messages = Parser.StringToList(data);
        //    List<RFIDMessage> carMsgs = (from msg in messages where msg.Type == MessageType.CAR select msg).ToList();
        //    List<RFIDMessage> boxMsgs = (from msg in messages where msg.Type == MessageType.BOX select msg).ToList();

        //    addCarMessages(carMsgs);
        //    addBoxMessages(boxMsgs);
        //}

        private void addCarMessages(List<RFIDMessage> msgs)
        {
            foreach (RFIDMessage msg in msgs)
            {
                addCarMessage(msg);
            }
        }

        private void addCarMessage(RFIDMessage msg)
        {
            if ((from c in carMsgList where c.Nr.Equals(msg.Nr) select c).Count() == 0)
            {
                carMsgList.Add(msg);
            }
            handleCarMessages( msg);
        }

        private void addBoxMessages(List<RFIDMessage> msgs)
        {
            foreach (RFIDMessage msg in msgs)
            {
                addBoxMessage(msg);
            }
        }

        private void addBoxMessage(RFIDMessage msg)
        {
            if ((from b in boxMsgList where b.Nr.Equals(msg.Nr) select b).Count() == 0)
            {
                boxMsgList.Add(msg);
                refreshOrderBox(new OrderBox() { nr = msg.Nr });
                handleBoxMessages();
            }
        }

        /// <summary>
        /// 处理扫描的料车号
        /// </summary>
        private void handleCarMessages(RFIDMessage msg)
        {
            if (carMsgList.Count == 1)
            {
                this.orderCar = new OrderCar() { nr = carMsgList.First().Nr };
                OrderCarTB.Text = carMsgList.First().Nr;
            }
            else if (carMsgList.Count > 1)
            {
                this.orderCar = null;
              //  stopScan();

                if (!showMultiCarFlag)
                {
                    showMultiCarFlag = true;
                    System.Windows.Forms.MessageBox.Show("信号干扰！同时扫描到多辆料车:"+msg.Nr+"，请重新扫描！");
                }
            }
        }

        /// <summary>
        /// 处理扫描的料盒号
        /// </summary>
        private void handleBoxMessages()
        {
            QtyLabel.Content = boxMsgList.Count;
        }

 

        private void refreshOrderBox(OrderBox orderBox = null)
        {
            this.orderBoxes.Add(orderBox);
            // 设置datagrid的数据
            PreviewDG.ItemsSource = this.orderBoxes;
            PreviewDG.Items.Refresh();
        }

        private void ScanTB_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ScanTB.Text.Trim().Length > 0)
            {
                RFIDMessage msg = Parser.StringToMessage(ScanTB.Text.Trim());
                if (msg != null)
                {
                    if (msg.Type == MessageType.CAR)
                    {
                        this.carMsgList.Clear();
                        addCarMessage(msg);
                    }
                    else if (msg.Type == MessageType.BOX)
                    {
                        addBoxMessage(msg);
                    }
                    ScanTB.Text = string.Empty;
                }
            }
        }

      


        public void showMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void stopScan()
        {
            //this.Dispatcher.Invoke(DispatcherPriority.Normal, (MethodInvoker)delegate()
            //{
            parentWindow.StopTimer();
            // });
        }

        public bool Validate()
        {
            if (this.orderCar != null && this.orderBoxes.Count > 0)
            {
                return true;
            }
            return false;
        }

    }
}
