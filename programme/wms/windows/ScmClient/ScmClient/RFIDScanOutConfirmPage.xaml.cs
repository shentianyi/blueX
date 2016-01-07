﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ScmWcfService.Model;
using ScmWcfService;
using ScmWcfService.Model.Message;

namespace ScmClient
{
    /// <summary>
    /// RFIDScanOutConfirmPage.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDScanOutConfirmPage : Page
    {
        RFIDScanOutWindow parentWindow;

        public OrderCar orderCar { get; set; }
        public List<OrderBox> orderBoxes { get; set; }
        public bool canNext { get; set; }

        public RFIDScanOutConfirmPage()
        {
            InitializeComponent();
        }

        public RFIDScanOutConfirmPage(RFIDScanOutWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }


        public RFIDScanOutConfirmPage(RFIDScanOutWindow parentWindow, OrderCar orderCar, List<OrderBox> orderBoxes)
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

        public void MoveStroage() {
            if (this.orderBoxes.Count > 0)
            {
                WarehouseService service = new WarehouseService();
                List<int> ids = new List<int>();
                foreach (OrderBox box in this.orderBoxes)
                {
                    ids.Add(box.id);
                }
                ResponseMessage<object> msg = service.MoveStorageByCar(this.orderCar.id, ids);
                if (!msg.Success)
                {
                    showMessageBox(msg.Message);
                }
                else
                {
                    showMessageBox("出库成功！");
                    this.canNext = true;
                }
            }
            else {
                showMessageBox("无料盒，不可出库！");
            }
        }
    }
}