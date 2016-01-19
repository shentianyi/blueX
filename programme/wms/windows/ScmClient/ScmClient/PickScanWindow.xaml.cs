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
using ScmWcfService;
using ScmWcfService.Model;

namespace ScmClient
{
    /// <summary>
    /// PickWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PickScanWindow : MetroWindow
    {
        public PickScanWindow()
        {
            InitializeComponent();
        }

        List<PickItem> pickItems;
        bool showMenu = true;


        private void orderCarNrTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                getPickItems();
            }
        }

        private void goBtn_Click(object sender, RoutedEventArgs e)
        {
            getPickItems();
        }
        private void getPickItems()
        {
            if (orderCarNrTB.Text.Trim().Length > 0)
            {
                PickService ps = new PickService();
                var msg = ps.GetPickItemByCarNr(orderCarNrTB.Text.Trim());

                if (msg.http_error)
                {
                    showMessageBox(msg.Message);
                    orderCarNrTB.Text = string.Empty;
                }
                else if (!msg.Success)
                {
                    showMessageBox(msg.Message);

                    orderCarNrTB.Text = string.Empty;
                }
                else
                {
                    this.pickItems = msg.data;
                    this.showMenu = false;
                   
                    new PickListWindow(orderCarNrTB.Text.Trim(), this.pickItems).Show();
                    this.Close();
                }
            }
        }


        public void showMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            orderCarNrTB.Focus();
        }


        private void MetroWindow_Closing(object sender, EventArgs e)
        {
            if (this.showMenu)
            {
                new MenuWindow().Show();
            }
        }
    }
}
