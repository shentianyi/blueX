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
        PTL p;

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
                sp.Open();
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
            Byte[] recvbuf = new Byte[26];
            sp.Read(recvbuf, 0, recvbuf.Length);
            LogUtil.Logger.Info(recvbuf);
            p.HandleMsg(recvbuf);
        }

        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
           // PTL p = new PTL(sp, int.Parse(addressTB.Text));
            p = new PTL(sp, int.Parse(addressTB.Text));
           //  p.nrs = getNrs();
           //  p.FindLabels();
           // p.FindLabels();
            p.FindLabels(getNrs());
        }

        public List<string> getNrs() {
          return  nrTB.Text.Split(',').ToList();
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            PTL p = new PTL(sp, int.Parse(addressTB.Text));
            p.CancelLabels(getNrs());
        }
    }
}
