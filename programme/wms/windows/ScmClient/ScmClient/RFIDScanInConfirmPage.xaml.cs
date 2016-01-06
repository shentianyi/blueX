using System;
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
    /// RFIDScanInConfirmPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanInConfirmPage : Page
    {
        RFIDScanInWindow parentWindow;
        bool showMultiCarFlag = false;
        public bool valid = true;

        List<RFIDMessage> carMsgList = new List<RFIDMessage>();
        List<RFIDMessage> boxMsgList = new List<RFIDMessage>();

        OrderCar orderCar = null;
        List<OrderBox> orderBoxes = new List<OrderBox>();

        public RFIDScanInConfirmPage()
        {
            InitializeComponent();
        }

        public RFIDScanInConfirmPage(RFIDScanInWindow parentWindow)
        {
            InitializeComponent();
            OrderCarMsgLabel.Visibility = Visibility.Hidden;
            this.parentWindow = parentWindow;
        }


        public void ReceiveData(string data)
        {
            List<RFIDMessage> messages = Parser.StringToList(data);
            List<RFIDMessage> carMsgs = (from msg in messages where msg.Type == MessageType.CAR select msg).ToList();
            List<RFIDMessage> boxMsgs = (from msg in messages where msg.Type == MessageType.BOX select msg).ToList();

            addCarMessages(carMsgs);
            addBoxMessages(boxMsgs);
        }

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
        }

        private void addBoxMessages(List<RFIDMessage> msgs)
        {
            foreach (RFIDMessage msg in msgs)
            {
                addBoxMessage(msg);
            }
            handleCarMessages();
        }

        private void addBoxMessage(RFIDMessage msg)
        {
            if ((from b in boxMsgList where b.Nr.Equals(msg.Nr) select b).Count() == 0)
            {
                boxMsgList.Add(msg);
              //  validateOrderBoxNr(msg.Nr);
            }
            handleBoxMessages();
        }

        /// <summary>
        /// 处理扫描的料车号
        /// </summary>
        private void handleCarMessages()
        {
            if (carMsgList.Count == 1)
            {
                OrderCarTB.Text = carMsgList.First().Nr;
                validateOrderCarNr();
            }
            else if (carMsgList.Count > 1)
            {
                stopScan();

                if (!showMultiCarFlag)
                {
                    showMultiCarFlag = true;
                    valid = false;
                    System.Windows.Forms.MessageBox.Show("信号干扰！同时扫描到多辆料车，请重新扫描！");
                }
            }
        }

        /// <summary>
        /// 检查料车号是否存在
        /// </summary>
        private void validateOrderCarNr()
        {
            OrderService service = new OrderService();
            ResponseMessage<OrderCar> msg = service.GetOrderCarByNr(OrderCarTB.Text);
            if (msg.http_error)
            {
                stopScan();
                showMessageBox(msg.Message);
            }
            else if (!msg.Success)
            {
                OrderCarMsgLabel.Visibility = Visibility.Visible;
            }
            else
            {
                this.orderCar = msg.data;
            }
        }

        /// <summary>
        /// 处理扫描的料盒号
        /// </summary>
        private void handleBoxMessages()
        {
            QtyLabel.Content = boxMsgList.Count;
        }

        /// <summary>
        /// 检查料盒号是否存在
        /// </summary>
        private void validateOrderBoxNr(string boxNr)
        {
            OrderService service = new OrderService();
            ResponseMessage<OrderBox> msg = service.GetOrderBoxByNr(boxNr);
            if (msg.http_error)
            {
                stopScan();
                showMessageBox(msg.Message);
            }
            else if (!msg.Success)
            {
                orderBoxes.Add(new OrderBox() { nr = boxNr });
            }
            else
            {
                orderBoxes.Add(msg.data);
            }
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
                        addCarMessage(msg);
                    }
                    else if (msg.Type == MessageType.BOX)
                    {
                        addBoxMessage(msg);
                    }
                }
            }
        }


        public void showMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void stopScan()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (MethodInvoker)delegate()
            {
                parentWindow.StopTimer();
            });
        }
    }
}
