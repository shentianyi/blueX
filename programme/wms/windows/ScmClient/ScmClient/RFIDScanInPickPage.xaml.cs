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

        public RFIDScanInPickPage()
        {
            InitializeComponent();
        }

        public RFIDScanInPickPage(RFIDScanInWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }


        public RFIDScanInPickPage(RFIDScanInWindow parentWindow, OrderCar orderCar, List<OrderBox> orderBoxes)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.orderCar = orderCar;
            this.orderBoxes = orderBoxes;
            this.OrderCarLabel.Content = orderCar.nr;
            this.QtyLabel.Content = this.orderBoxes.Count;
            this.PreviewDG.ItemsSource = this.orderBoxes;
        }

      
        public void showMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewDG.SelectedIndex > -1)
            {
                if (MessageBox.Show("删除后将无法恢复，请确认是否删除！", "确认提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    OrderBox box = PreviewDG.SelectedItem as OrderBox;
                    this.orderBoxes.Remove(box);
                    PreviewDG.ItemsSource = null;
                    PreviewDG.ItemsSource = this.orderBoxes;

                    this.QtyLabel.Content = this.orderBoxes.Count;
                }
            }
        }

        public Pick GenereatePick() {
            Pick pick = null;
            return pick;
        }
    }
}
