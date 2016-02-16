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
using System.Runtime.InteropServices;

namespace RFIDdemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
 
    public partial class MainWindow : Window
    {
        static List<string> dataList;
        static SerialPort serialPort;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataList = new List<string>();

            try
            {
             serialPort = new SerialPort();
                serialPort.PortName = "COM1";
                serialPort.BaudRate = 9600;
                serialPort.DataBits = 8;
                serialPort.Open();
                serialPort.DataReceived+=new SerialDataReceivedEventHandler(serialPort_DataReceived);
                DG.ItemsSource = dataList;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
           [DllImport(@"Thrid\EpcDll.dll")]
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
          //  string data = ((SerialPort)sender).ReadExisting();
          //  dataList.Add(data);

           // int tmp = serialPort.ReadByte();
            byte[] readBuffer = new byte[serialPort.ReadBufferSize];
            serialPort.Read(readBuffer, 0, readBuffer.Length);
            string data = Encoding.Unicode.GetString(readBuffer);
            string s = "";
  //this.Invoke(updateText, new string[] { Encoding.Unicode.GetString(readBuffer) });
        }
    }
}
