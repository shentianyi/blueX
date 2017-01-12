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
using Brilliantech.Framwork.Utils.LogUtil;
using CanLightServiceLib;
using CanLightServiceLib.Config;

namespace PLCLightWPFTest
{
    /// <summary>
    /// CanServerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CanServerWindow : Window
    {
        public CanServerWindow()
        {
            InitializeComponent();
        }
        private CanLightTcpServer server;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Dictionary<string, string> canClients = new Dictionary<string, string>();
                //canClients.Add("1", "192.168.1.179");
                //server = new CanLightTcpServer("192.168.1.100", "6001", CanConfig.CanModels);
                server = new CanLightTcpServer("169.254.163.50", "6001", CanConfig.CanModels);
                server.Start();
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                server.Stop();
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
