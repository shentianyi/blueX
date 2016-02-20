using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScmWcfService.Model;
using ScmWcfService;
using ScmWcfService.Model.Message;
using System;
using Brilliantech.Framwork.Utils.LogUtil;
using ScmWcfService.Config;

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
            this.canNext = false;
        }

        private void RFIDScanInConfirmPageName_Loaded(object sender, RoutedEventArgs e)
        {
            this.OrderCarLabel.Content = orderCar.nr;
            this.PreviewDG.ItemsSource = this.orderBoxes;
            setQtyLabel();
            validate();
        }

        public void validate() {
            validateOrderCarNr();
            validateOrderBoxNr();
        }

        /// <summary>
        /// 检查料车号是否存在
        /// </summary>
        private void validateOrderCarNr()
        {
            OrderService service = new OrderService();
            ResponseMessage<OrderCar> msg = service.GetOrderCarByNr(this.orderCar.nr);
            if (msg.http_error)
            {
                showMessageBox(msg.Message);
            }
            else if (!msg.Success)
            {
                OrderCarMsgLabel.Visibility = Visibility.Visible;
            }
            else
            {
                this.orderCar = msg.data;
                //OrderCarMsgLabel.Visibility = Visibility.Visible;
                //OrderCarMsgLabel.Content = orderCar.status_display;
            }
        }


        /// <summary>
        /// 检查料盒号是否存在
        /// </summary>
        private void validateOrderBoxNr()
        { 
            OrderService service = new OrderService();
            ResponseMessage<List<OrderBox>> msg = service.GetOrderBoxByNrs(OrderBox.GetAllNrs(this.orderBoxes));
            if (msg.http_error)
            {
                showMessageBox(msg.Message);
            }
            else if (!msg.Success)
            {
                 
            }
            else
            {
                List<OrderBox> ob = msg.data;
                if (ob.Count > 0)
                {
                    OrderBox.Updates(this.orderBoxes, ob);
                }
                setQtyLabel();
                this.PreviewDG.ItemsSource = null;
                this.PreviewDG.ItemsSource = this.orderBoxes;
            }
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
                    setQtyLabel();
                }
            }
        }

        /// <summary>
        /// 生成择货单
        /// </summary>
        public void GenereatePick()
        {
            if (this.orderCar != null && this.orderCar.id>0 && OrderBox.GetNotNullCount(this.orderBoxes) == this.orderBoxes.Count)
            {
                PickService service = new PickService();
                
                ResponseMessage<Pick> msg = service.CreatePickByOrderCar(this.orderCar.id, OrderBox.GetAllIds(this.orderBoxes));
                if (!msg.Success)
                {
                    this.canNext = false;
                    showMessageBox(msg.Message);
                }
                else
                {
                    this.pick = msg.data;
                    this.canNext = true;
                }
            }
            else
            {
                this.canNext = false;
                showMessageBox("料车或料盒不存在，不可生成择货单！");
            }
        }

        /// <summary>
        /// 根据料车移库
        /// </summary>
        public void MoveStroage()
        {
            try
            {
                if (this.orderCar != null && this.orderCar.id > 0)
                {
                    List<int> boxIds = null;
                    if (RFIDConfig.OutAutoLoadPick == false)
                    {
                        int qty = OrderBox.GetNotNullCount(this.orderBoxes);
                        if (qty == this.orderBoxes.Count && qty > 0) { 
                        boxIds= OrderBox.GetAllIds(this.orderBoxes);
                        }
                    }


                    WarehouseService service = new WarehouseService();
                    ResponseMessage<object> msg = service.MoveStorageByCarAndBoxes(this.orderCar.id, boxIds);
                    if (!msg.Success)
                    {
                        this.canNext = false;
                    }
                    else
                    {
                        this.canNext = true;
                    }
                }
                else
                {
                    this.canNext = false;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message);
            }
            this.canNext = true;
                
            showMessageBox("出库成功！");
        }

        private void setQtyLabel()
        {
            this.QtyLabel.Content = this.orderBoxes.Count;
            this.QtyValidLabel.Content = OrderBox.GetNotNullCount(this.orderBoxes);
        }

        private void ValidateBtn_Click(object sender, RoutedEventArgs e)
        {
            validate();
        }

    }
}
