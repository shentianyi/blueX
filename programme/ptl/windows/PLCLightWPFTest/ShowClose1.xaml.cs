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

namespace PLCLightWPFTest
{
    /// <summary>
    /// ShowClose1.xaml 的交互逻辑
    /// </summary>
    public partial class ShowClose1 : Window
    {
        bool goMenu = true;
        public ShowClose1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.goMenu = false;
            this.Close();
            new ShowClose2().Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (goMenu)
            {
                new Menu().Show();
            }
        }
    }
}
