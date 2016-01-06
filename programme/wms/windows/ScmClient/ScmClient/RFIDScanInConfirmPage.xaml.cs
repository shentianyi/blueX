using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

        public OrderCar orderCar { get; set; }
        public List<OrderBox> orderBoxes { get; set; }
        public Pick pick { get; set; }
        public bool canNext { get; set; }

        public RFIDScanInConfirmPage()
        {
            InitializeComponent();
        }

        public RFIDScanInConfirmPage(RFIDScanInWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }


        public RFIDScanInConfirmPage(RFIDScanInWindow parentWindow,OrderCar orderCar,List<OrderBox> orderBoxes)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.orderCar = orderCar;
            this.orderBoxes = orderBoxes;
            this.OrderCarLabel.Content = orderCar.nr;
            this.QtyLabel.Content = this.orderBoxes.Count;
            this.PreviewDG.ItemsSource = this.orderBoxes;
            this.canNext = false;
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

        public void GenereatePick() {
            if (this.orderBoxes.Count > 0)
            {
                PickService service = new PickService();
                List<int> ids = new List<int>();
                foreach (OrderBox box in this.orderBoxes)
                {
                    ids.Add(box.id);
                }
                ResponseMessage<Pick> msg = service.CreatePickByOrderCar(this.orderCar.id, ids);
                if (!msg.Success)
                {
                    showMessageBox(msg.Message);
                }
                else
                {
                    this.pick = msg.data;
                    this.canNext = true;
                }
            }
            else {
                showMessageBox("无料盒，不可生成择货单！");
            }
        }
    }
}
