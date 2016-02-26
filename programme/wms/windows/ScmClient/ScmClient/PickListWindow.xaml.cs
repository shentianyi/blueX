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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using ScmWcfService.Model;
using ScmClient.Helper;

namespace ScmClient
{
    /// <summary>
    /// PickListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PickListWindow : MetroWindow
    {
        public PickListWindow()
        {
            InitializeComponent();
        }


        List<PickItem> pickItems;
        string carNr;

        public PickListWindow(string carNr,List<PickItem> pickItems)
        {
            InitializeComponent();
            this.carNr = carNr;
            this.pickItems = pickItems;
        }

        private void MetroWindow_Closing(object sender, EventArgs e)
        {
            new MenuWindow().Show();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            new VoiceHelper() { Text="开始配料，请扫描料盒号!"}.Speak();

            OrderCarLabel.Content = this.carNr;
            QtyLabel.Content = this.pickItems.Count;
            QtyValidLabel.Content = getValidCount();
            

            PreviewDG.ItemsSource = this.pickItems;

            ScanTB.Focus();
        }



        private void showWeight(PickItem item) 
        {
            new PickWeightWindow(item,this).ShowDialog();
        }

        private void ScanTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ScanTB.Text.Trim().Length > 0)
            {
                PickItem item = getPickItem(ScanTB.Text.Trim());
                if (item != null)
                {
                    showWeight(item);
                }
                else
                {
                    new VoiceHelper() { Text = "扫描错误！料盒不存在！" }.Speak();
                    MessageBox.Show("扫描错误！料盒不存在！");
                }
                ScanTB.Focus();
                ScanTB.SelectAll();
                // ScanTB.Text = string.Empty;
            }
        }

        private PickItem getPickItem(string box_nr) {
            PickItem item = null;
            item = (from i in this.pickItems where i.order_box_nr.Equals(box_nr) select i).FirstOrDefault();
            return item;
        }

        private void countBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PreviewDG.SelectedIndex > -1) {
                showWeight(PreviewDG.SelectedItem as PickItem);
            }
        }

        public void RefreshData() {
            PreviewDG.ItemsSource = this.pickItems;
            PreviewDG.Items.Refresh();
            QtyValidLabel.Content = getValidCount();    

        }

        public int getValidCount() {
         return   this.pickItems.Where(i => i.weight_valid == true).Count();
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("请确定?", "确定", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private bool valid()
        {
            return getValidCount() == this.pickItems.Count;
        }

        private void PreviewDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PreviewDG.SelectedIndex > -1)
            {
                showWeight(PreviewDG.SelectedItem as PickItem);
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("确定关闭？", "提醒", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                new MenuWindow().Show();
            }
            else
            {
                e.Cancel = true;
            }
        }

    }
}
