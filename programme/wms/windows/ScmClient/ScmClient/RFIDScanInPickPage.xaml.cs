using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ScmWcfService.Model;

namespace ScmClient
{
    /// <summary>
    /// RFIDScanInPickPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanInPickPage : Page
    {
        RFIDScanInWindow parentWindow;

        public OrderCar orderCar { get; set; }
        public List<OrderBox> orderBoxes { get; set; }
        public Pick pick { get; set; }

        public RFIDScanInPickPage()
        {
            InitializeComponent();
        }

        public RFIDScanInPickPage(RFIDScanInWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }


        public RFIDScanInPickPage(RFIDScanInWindow parentWindow, OrderCar orderCar, List<OrderBox> orderBoxes,Pick pick)
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
