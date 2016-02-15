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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using Brilliantech.Framwork.Utils.LogUtil;

namespace PTLWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort sp;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openComBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sp = new SerialPort(comTB.Text);
                sp.BaudRate = int.Parse(baundTB.Text);
                sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                LogUtil.Logger.Info("COM Opend!");
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Debug(ex.Message);
            }
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
          
        }
    }
}
