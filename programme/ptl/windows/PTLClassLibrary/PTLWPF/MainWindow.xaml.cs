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
                p = new PTL(this.comTB.Text, int.Parse(this.baundTB.Text), int.Parse(this.addressTB.Text));
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Debug(ex.Message);
            }
        }
        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            p.cmdType = CommandType.FIND;
            p.nrs = getNrs();
            p.FindLabels();
        }

        public List<string> getNrs() {
          return  nrTB.Text.Split(',').ToList();
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            p.cmdType = CommandType.CANCEL;
            p.nrs = getNrs();
            p.CancelLabels();
        }

        private void cancelAllBtn_Click(object sender, RoutedEventArgs e)
        {
            p.cmdType = CommandType.ALL_CANCEL;
            p.CancelAllLabels();
        }

        private void findAllBtn_Click(object sender, RoutedEventArgs e)
        {
            p.cmdType = CommandType.ALL_FIND;
            p.FindAllLabels();
        }
    }
}
