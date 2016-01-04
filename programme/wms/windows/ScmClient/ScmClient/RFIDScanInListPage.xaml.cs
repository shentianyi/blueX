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

namespace ScmClient
{
    /// <summary>
    /// RFIDScanInListPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanInListPage : Page
    {

        RFIDScanInWindow parentWindow;

        List<RFIDMessage> carList = new List<RFIDMessage>();
        List<RFIDMessage> boxList = new List<RFIDMessage>();

        public RFIDScanInListPage()
        {
            InitializeComponent();
        }
        public RFIDScanInListPage(RFIDScanInWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }


        public void ReceiveData(string data)
        {
            List<RFIDMessage> messages = Parser.StringToList(data);
            List<RFIDMessage> cars = (from msg in messages where msg.Type == MessageType.CAR select msg).ToList();
            List<RFIDMessage> boxes = (from msg in messages where msg.Type == MessageType.BOX select msg).ToList();
 
            foreach(RFIDMessage car in cars)
            {
                if ((from c in carList where c.Nr.Equals(car.Nr) select c).Count() == 0)
                {
                    carList.Add(car);
                }
            }

            foreach (RFIDMessage box in boxes)
            {
                if ((from b in boxList where b.Nr.Equals(box.Nr) select b).Count() == 0)
                {
                    boxList.Add(box);
                }
            }

            handleMessage();
        }

        private void handleMessage() {

            if (carList.Count > 1) {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (MethodInvoker)delegate()
                {
                    parentWindow.StopTimer();
                });

                System.Windows.Forms.MessageBox.Show("信号干扰！同时扫描到多辆料车，请重新扫描！");
                return;
            }
            if (carList.Count == 1)
            {
                DeliveryCarTB.Text = carList.First().Nr;
            }

            QtyLabel.Content = boxList.Count();

        }

    }
}
