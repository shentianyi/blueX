﻿using System;
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
using ScmWcfService.Config;
using System.Net.Sockets;
using ScmClient.service;
using Brilliantech.Framwork.Utils.ConvertUtil;
using ScmWcfService;
using ScmWcfService.Model.Message;

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
        int currentAgvPoint = 0;

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
            new VoiceHelper("开始配料，请扫描料盒号！").Speak();

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
                    new VoiceHelper("扫描错误！料盒不存在！").Speak();
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

        Socket socket = null;
        ProtocolService tcs = new ProtocolService();
        private byte[] station_msg = new byte[] {
                                                          //方向
                0xFD, 0x13, 0x00, 0x06, 0x0F, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

        public int getAgvPoint()
        {
            string ip = ServerConfig.agvHost;
            int port = ServerConfig.agvPort;

            if (socket == null)
            {
                socket = tcs.ConnectServer(ip, port);

                if (socket == null)
                {
                    MessageBox.Show("服务器连接失败....");
                    return 0;
                }
            }

            ProtocolMessage<int> rep = tcs.GetAgvInfo(socket);

            if (rep.result)
            {
                return rep.data;
            }
            else
            {
                MessageBox.Show("发送失败...");
            }


            return 0;
        }

        private void sendDesStation(byte[] msg)
        {
            string ip = ServerConfig.agvHost;
            int port = ServerConfig.agvPort;

            if (socket == null)
            {
                socket = tcs.ConnectServer(ip, port);

                if (socket == null)
                {
                    MessageBox.Show("服务器连接失败....");
                    return;
                }
            }

            ProtocolMessage<Socket> rep = tcs.SendMessage(socket, msg);

            if (rep.result)
            {
                //MessageBox.Show("开始接收数据...");
                //byte[] recvBytes = new byte[1024];
                //int bytes = 0;
                ////bytes = rep.data.Receive(recvBytes, recvBytes.Length, 0);
                ////MessageBox.Show(ScaleConvertor.HexBytesToString(recvBytes));
                ////MessageBox.Show(Encoding.Default.GetString(recvBytes));
                ////rep.data.Shutdown(SocketShutdown.Both);
                ////rep.data.Close();
                MessageBox.Show("结束通讯...");
            }
            else
            {
                MessageBox.Show("发送失败...");
            }

            return;
        }

        private void sendAgvAndPtlCmd(PickItem item)
        {
            //send agv cmd
            if (currentAgvPoint == 0)
            {
                currentAgvPoint = getAgvPoint();
            }

            //TODO generate direction cmd 
            byte[] msg = station_msg;
            msg[8] = 0x01;

            if (item.order_box!=null && item.order_box.position_leds[0]!=null && item.order_box.position_leds[0].dock_point != null)
            {
                msg[7] = (byte)(int.Parse(item.order_box.position_leds[0].dock_point.id));
                sendDesStation(msg);
            }
            else
            {
                MessageBox.Show("未找到停靠点信息,请联系管理员...");
            }

            
            //TODO send ptl cmd

            return;
        }


        private void showAgvAction()
        {
            new AgvActionWindow(this, currentAgvPoint).ShowDialog();
        }

        private void startAgvBtn_Click(object sender, RoutedEventArgs e)
        {
            showAgvAction();
            //Socket socket = null;
            //TcpClientService tcs = new TcpClientService();
            //byte[] msg = new byte[] {
            //    //0xFD, 0x13, 0x00, 0x07, 0x0F, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    0xFD, 0x13, 0x00, 0x07, 0x0F, 0x01, 0x00, 0x50, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

            //    //0x01, 0x00, 0x00, 0x01, 0x02, 0x01, 0xC0, 0x01, 0x00, 0x1B, 0xFF, 0xFF, 0x00
            //};


            //MessageBox.Show(ScaleConvertor.HexBytesToString(msg));
            //string ip = "192.168.1.254";
            //int port = 9000;
            //string ip1 = ServerConfig.agvHost;
            //int port1 = int.Parse(ServerConfig.agvPort);

            //socket = tcs.ConnectServer(ip, port);

            //if (socket == null)
            //{
            //    MessageBox.Show("服务器连接已断开....");
            //    return;
            //}

            //TcpMessage<Socket> rep = tcs.SendMessage(socket, msg);

            //if (rep.result)
            //{
            //    //MessageBox.Show("开始接收数据...");
            //    //byte[] recvBytes = new byte[1024];
            //    //int bytes = 0;
            //    //bytes = rep.data.Receive(recvBytes, recvBytes.Length, 0);
            //    //MessageBox.Show(ScaleConvertor.HexBytesToString(recvBytes));
            //    //MessageBox.Show(Encoding.Default.GetString(recvBytes));
            //    //rep.data.Shutdown(SocketShutdown.Both);
            //    //rep.data.Close();
            //    MessageBox.Show("结束通讯...");
            //}
            //else
            //{
            //    MessageBox.Show("发送失败...");
            //}

            //return;
        }
    }
}
