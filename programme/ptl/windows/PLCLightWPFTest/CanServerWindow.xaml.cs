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
using PLCLightWPFTest.Properties;

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
            textBox.Text = Settings.Default.canServerIP;
            textBox1.Text = Settings.Default.canServerPort;
            this.button.Content = "启动";
            this.button1.Content = "暂停";
        }

        private CanLightTcpServer server;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                server = new CanLightTcpServer(textBox.Text, textBox1.Text, CanConfig.CanModels);


                server.AcceptNewClientAction = new Action<string>((string clientIp) =>
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        try
                        {
                            if (CanConfig.CanModels.SingleOrDefault(s => s.UniqKey == clientIp) != null)
                            {
                                canListBox.Items.Add(clientIp);
                            }
                            else
                            {
                                clientListBox.Items.Add(clientIp);
                            }
                            canCountLab.Content = canListBox.Items.Count;
                            clientCountLab.Content = clientListBox.Items.Count;
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Logger.Error(ex.Message, ex);
                            MessageBox.Show(ex.Message);
                        }
                    }));
                });

                server.LostClientAction = new Action<string>((string clientIp) =>
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        try
                        {
                            // clientListBox.Items.Remove(clientIp);
                            if (CanConfig.CanModels.SingleOrDefault(s => s.UniqKey == clientIp) != null)
                            {
                                canListBox.Items.Remove(clientIp);
                            }
                            else
                            {
                                clientListBox.Items.Remove(clientIp);
                            }

                            canCountLab.Content = canListBox.Items.Count;
                            clientCountLab.Content = clientListBox.Items.Count;
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Logger.Error(ex.Message, ex);
                            MessageBox.Show(ex.Message);
                        }
                    }));
                });




                server.ReceiveClientMessageAction = new Action<string>((string msg) =>
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        try
                        {
                            msgListBox.Items.Add(msg);
                            msgCountLab.Content = msgListBox.Items.Count;
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Logger.Error(ex.Message, ex);
                            MessageBox.Show(ex.Message);
                        }
                    }));
                });


                server.Start();
                this.button.Content = "【已启动】";
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
                this.msgListBox.Items.Clear();
                this.clientListBox.Items.Clear();
                this.canListBox.Items.Clear();

                canCountLab.Content = canListBox.Items.Count;
                clientCountLab.Content = clientListBox.Items.Count;
                msgCountLab.Content = msgListBox.Items.Count;

                this.button.Content = "启动";
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.msgListBox.Items.Clear();
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void clientListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
