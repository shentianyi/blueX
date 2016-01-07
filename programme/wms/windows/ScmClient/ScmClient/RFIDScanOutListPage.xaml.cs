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
    /// RFIDScanOutListPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanOutListPage : Page
    {
        RFIDScanOutWindow parentWindow;
        bool showMultiCarFlag = false;
        public bool carValid = false;
        public bool carValidated = false;
        public bool boxValid = false;

        List<RFIDMessage> carMsgList = new List<RFIDMessage>();
        List<RFIDMessage> boxMsgList = new List<RFIDMessage>();

        public OrderCar orderCar { get; set; }
        public List<OrderBox> orderBoxes { get; set; }


        public RFIDScanOutListPage()
        {
            InitializeComponent();
        }

        public RFIDScanOutListPage(RFIDScanOutWindow parentWindow)
        {
            InitializeComponent();
            OrderCarMsgLabel.Visibility = Visibility.Hidden;
            this.parentWindow = parentWindow;
            this.orderBoxes = new List<OrderBox>();
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
            handleCarMessages();
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
                validateOrderBoxNr(msg.Nr);
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
                if (!carValidated || !carValid)
                {
                    validateOrderCarNr();
                }
            }
            else if (carMsgList.Count > 1)
            {
                stopScan();

                if (!showMultiCarFlag)
                {
                    showMultiCarFlag = true;
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
                this.carValid = false;
            }
            else
            {
                this.orderCar = msg.data;
                OrderCarMsgLabel.Visibility = Visibility.Visible;
                OrderCarMsgLabel.Content = orderCar.status_display;
                carValidated = true;
                this.carValid = true;
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
                boxValid = false;
                refreshOrderBox(new OrderBox() { nr = boxNr });

                stopScan();
                showMessageBox(msg.Message);
            }
            else if (!msg.Success)
            {
                boxValid = false;
                refreshOrderBox(new OrderBox() { nr = boxNr });
               // PreviewDG.ItemsSource = orderBoxes;
            }
            else
            {
                refreshOrderBox(msg.data);
                boxValid = true;
              //  PreviewDG.ItemsSource = orderBoxes;
            }
        }

        private void refreshOrderBox(OrderBox orderBox)
        {
            OrderBox ob = (from b in this.orderBoxes where b.nr.Equals(orderBox.nr) select b).FirstOrDefault();
            if (ob != null)
            {
                if (orderBox.id == 0)
                {
                    // 如果未查询到数据库数据则不处理
                }
                else
                {
                    if (ob.id == 0) { 
                     // 移除这个空的，这个情况应该不存在，因为已经过滤了
                    } 
                }
            }
            else
            {
                this.orderBoxes.Add(orderBox);
            }

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

        public bool Validate() {
            if (this.carValid == true && this.boxValid == true) {
                return true;
            }
           return false;
        }
    }
}
