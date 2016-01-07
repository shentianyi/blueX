using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ScmWcfService.Model;

namespace ScmClient
{
    /// <summary>
    /// RFIDScanOutPickPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanOutPickPage : Page
    {
        RFIDScanOutWindow parentWindow;

        public OrderCar orderCar { get; set; }
        public List<OrderBox> orderBoxes { get; set; }
        public Pick pick { get; set; }

        public RFIDScanOutPickPage()
        {
            InitializeComponent();
        }

        public RFIDScanOutPickPage(RFIDScanOutWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }


        public RFIDScanOutPickPage(RFIDScanOutWindow parentWindow, OrderCar orderCar, List<OrderBox> orderBoxes, Pick pick)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.orderCar = orderCar;
            this.orderBoxes = orderBoxes;
            this.pick = pick;
            this.PickNrLabel.Content = this.pick.nr;
            this.PickStatusLabel.Content = this.pick.status_display;
            this.OrderCarLabel.Content = orderCar.nr;
            this.QtyLabel.Content = this.orderBoxes.Count;

            this.PreviewDG.ItemsSource = this.orderBoxes;
        }

      
        public void showMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }


       
    }
}
