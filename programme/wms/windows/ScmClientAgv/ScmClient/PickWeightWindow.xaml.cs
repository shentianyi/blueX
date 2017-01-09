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
using System.IO;
using ScmWcfService;
using ScmClient.Helper;
using ScmClient.Enum;
using ScmWcfService.Config;
using SpeechLib;
using ScmClient.Properties;
using Brilliantech.Framwork.Utils.LogUtil;
using System.Net.Sockets;
using ScmWcfService.Model.Message;
using Brilliantech.Framwork.Utils.ConvertUtil;
using PLCLightCL.Light;

namespace ScmClient
{
    /// <summary>
    /// PickWeightWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PickWeightWindow : MetroWindow
    {
        PickItem item;
        float standWeight = 0;
        float minWeight=0;
        float maxWeight = 0;

        bool valid=false;

        PickListWindow parentWindow;
        Socket ptlSocket;
        ProtocolService tcs = new ProtocolService();

        // net weight
        float net_weight = 0;
        bool net_weighted = false;

        ILightController lightController;
        public PickWeightWindow()
        {
            InitializeComponent();
        }


        public PickWeightWindow(Socket socket, PickItem item,PickListWindow parentWindow)
        {
            InitializeComponent();
            this.ptlSocket = socket;
            this.item = item;
            this.parentWindow = parentWindow;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                VoiceHelper vh = new VoiceHelper();
                vh.Text = new List<VoiceText>();
                if (BaseConfig.ForceNetWeight)
                {
                    VoiceText vt1 = new VoiceText() { Text = "请去皮！" };
                    vh.Text.Add(vt1);
                }
                if (BaseConfig.PlayPickPositionVoice && (!string.IsNullOrEmpty(this.item.positions_nr)))
                {
                    VoiceText vt2 = new VoiceText() { Text = "库位：" + this.item.positions_nr };

                    vh.Text.Add(vt2);
                    // new VoiceHelper() { Text = this.item.positions_nr, Times = 2 }.Speak();
                }
                vh.Speak();

                minWeight = item.quantity * (item.part.weight * (1 - item.part.weight_range));// +item.order_box.weight;
                maxWeight = item.quantity * (item.part.weight * (1 + item.part.weight_range));// +item.order_box.weight;
                standWeight = item.part.weight * item.quantity; //+ item.order_box.weight;

                partNrLabel.Content = item.part_nr;
                positionLabel.Content = item.positions_nr;
                unitWeightLabel.Content = item.part.weight;
                qtyLabel.Content = item.quantity;

                standWeightLabel.Content = minWeight + "-(" + standWeight + ")-" + maxWeight;
                qtyLabel.Content = (int)Math.Round(minWeight / this.item.part.weight) + "-(" + item.quantity + ")-" + (int)Math.Round(maxWeight / this.item.part.weight);
                actualWeightTB.Focus();


                string path = PathHelper.GetImagePath(item.part_nr);
                // using local image first
                if (path != null)
                {
                    BitmapImage i = new BitmapImage(new Uri(path, UriKind.Absolute));
                    partImage.Source = i;
                }
                else
                {
                    // using ftp images
                    Uri u = new Uri(Settings.Default.FtpImageServer + "/" + item.part_nr + Settings.Default.ImageExt, UriKind.Absolute);

                    try
                    {
                        partImage.Source = new BitmapImage(u);
                    }
                    catch { }
                }

                lightController = new RamLightController(ServerConfig.ptlComPort);

            }
            catch (Exception ex) {
                LogUtil.Logger.Error(ex.Message);
            }
        }

       

        private void actualWeightTB_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (Key.Enter == e.Key && actualWeightTB.Text.Trim().Length > 0)
                {
                    actualWeightTB.Text = actualWeightTB.Text.Trim();
                    actualWeightTB.SelectAll();
                    validateWeightAndVoice(0);
                }

            }
            catch (Exception ex)
            {

                LogUtil.Logger.Error("[****************************weight exception*************************]");
                LogUtil.Logger.Error(ex.Message);
                LogUtil.Logger.Error(ex.StackTrace);
                throw ex;
            }
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            validateWeightAndVoice(1);
        }

        private void validateWeightAndVoice(int i)
        {
            try
            {
                validateWeight();
                if (valid || (valid == false && BaseConfig.ForceWeightPass == false))
                {
                    weightOrderBox();
                    if (BaseConfig.AutoWeightConfirm)
                    {
                        this.Close();
                    }
                    else if (i == 1)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                LogUtil.Logger.Error("[****************************weight exception*************************]");
                LogUtil.Logger.Error(ex.Message);
                LogUtil.Logger.Error(ex.StackTrace);
                throw ex;
            }
        }


        private void weightOrderBox()
        {
            PickService ps = new PickService();

             float weight = 0;
             float.TryParse(actualWeightTB.Text.Trim(), out weight);

            var msg = ps.WeightOrderBox(item.order_box.id, item.id, item.weight,item.weight_qty,item.weight_valid);

            if (msg.http_error)
            {
                showMessageBox(msg.Message);
            }
            else if (!msg.Success)
            {
                showMessageBox(msg.Message);
            }
        }
        
        public void showMessageBox(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }

        private byte[] ptl_msg = new byte[] {
            0x01, 0x00, 0x00, 0x01, 0x02, 0x01, 0xC0, 0x01, 0x00, 0x02, 0xFF, 0xFF, 0x00
        };

        private byte[] generatePtlColorCmd(byte r, byte g, byte b)
        {
            byte[] ptlMsg = ptl_msg;


            if (item.order_box != null && item.order_box.position_leds.Count > 0
                && item.order_box.position_leds[0] != null
                && item.order_box.position_leds[0].led.id != null
                && item.order_box.position_leds[0].led.modem.id != null)
            {
                ptlMsg[0] = (byte)(int.Parse(item.order_box.position_leds[0].led.modem.id));
                ptlMsg[4] = (byte)(int.Parse(item.order_box.position_leds[0].led.id));
                ptlMsg[9] = (byte)(int.Parse(item.order_box.position_leds[0].led.id));
                ptlMsg[10] = r;
                ptlMsg[11] = g;
                ptlMsg[12] = b;
                //MessageBox.Show(ScaleConvertor.HexBytesToString(ptlMsg));
                sendPtlCmd(ptlMsg);
            }
            else
            {
                MessageBox.Show("未找到择货位置LED灯信息,请联系管理员...");
                return null;
            }

            if (item.order_box != null && item.order_box.position_leds.Count > 0
                    && item.order_box.box_led != null
                    && item.order_box.box_led.id != null
                    && item.order_box.box_led.modem.id != null)
            {
                lightController.Play(PLCLightCL.Enum.LightCmdType.OFF, new List<int>() { int.Parse(item.order_box.box_led.id) });
                //ptlMsg[0] = (byte)(int.Parse(item.order_box.box_led.modem.id));
                //ptlMsg[4] = (byte)(int.Parse(item.order_box.box_led.id));
                //ptlMsg[9] = (byte)(int.Parse(item.order_box.box_led.id));
                //ptlMsg[10] = r;
                //ptlMsg[11] = g;
                //ptlMsg[12] = b;
                ////MessageBox.Show(ScaleConvertor.HexBytesToString(ptlMsg));
                //sendPtlCmd(ptlMsg);
            }
            else
            {
                MessageBox.Show("未找到料盒LED灯信息,请联系管理员...");
                return null;
            }

            return ptlMsg;
        }

        private void sendPtlCmd(byte[] msg)
        {
            if (ptlSocket == null)
            {
                ptlSocket = tcs.ConnectServer(ServerConfig.ptlHost, ServerConfig.ptlPort);

                if (ptlSocket == null)
                {
                    MessageBox.Show("PTL服务器连接失败....");
                    return;
                }
            }

            ProtocolMessage<Socket> rep = tcs.SendMessage(ptlSocket, msg);

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
                //MessageBox.Show("PTL结束通讯...");
            }
            else
            {
                MessageBox.Show("PTL发送失败...");
            }

            return;
        }

        private void validateWeight()
        {
            string alertMessage = "";
            valid = false;
            float weight = 0;

            if (float.TryParse(actualWeightTB.Text.Trim(), out weight))
            {
                if (weight > 0)
                {
                    if (netWeighted())
                    {
                        weight = weight - net_weight;
                        weightMsgBorder.Visibility = Visibility.Visible;
                        int weight_qty = (int)Math.Round(weight / this.item.part.weight);
                        this.item.weight = weight;
                        this.item.weight_qty = weight_qty;
                        this.weightQtyLabel.Value = weight_qty;

                        if (weight >= minWeight && weight <= maxWeight)
                        {
                            playSound(SoundType.SUCCESS);
                            this.item.weight_valid = true;
                            valid = true;
                            weightMsgBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Green"));
                            weightMsgTB.Text = "通过";
                            alertLabel.Content = "通过";
                            this.parentWindow.RefreshData();
                            new VoiceHelper("通过").Speak();
                            
                            //灭灯
                            generatePtlColorCmd(0x00, 0x00, 0x00);
                        }
                        else
                        {
                            playSound(SoundType.FAIL);
                            this.item.weight_valid = false;
                            weightMsgTB.Text = "失败";
                            weightMsgBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red"));

                            if (weight < minWeight)
                            {
                                int mweight_qty = (int)Math.Round((standWeight - weight) / this.item.part.weight);

                                alertMessage = "太轻，还差" + mweight_qty + "个";
                            }
                            else if (weight > maxWeight)
                            {
                                int mweight_qty = (int)Math.Round((weight - standWeight) / this.item.part.weight);
                                alertMessage = "太重，多了" + mweight_qty + "个";
                            }
                            new VoiceHelper(alertMessage).Speak();
                            alertLabel.Content = alertMessage;
                        }
                    }
                    else
                    {
                        if (net_weight > 0)
                        {
                            alertMessage = "未去皮！";
                            new VoiceHelper(alertMessage).Speak();
                            alertLabel.Content = alertMessage;
                        }
                    }
                }

                if ((!net_weighted) && weight > 0)
                {
                    net_weight = weight;
                    alertMessage = "去皮成功！";
                    new VoiceHelper(alertMessage).Speak();
                    alertLabel.Content = alertMessage;

                    partImageBorder.BorderThickness = new Thickness(10);
                    netWeightLabel.Visibility = Visibility.Visible;
                    netWeightLabel.Content = "皮重:" + net_weight;
                    net_weighted = true;
                }



            }
            else
            {
                MessageBox.Show("重量未在范围内");
            }

            actualWeightTB.Focus();
            actualWeightTB.SelectAll();
        }

        private bool netWeighted()
        {
            if (BaseConfig.ForceNetWeight)
            {
                return net_weighted;
            }
            return true;
        }

        private void playSound(SoundType type)
        {
            if (BaseConfig.PlayWeightSound)
            {
                SoundHelper.PlaySound(type);
            }
        }

        private void actualWeightTB_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            weightMsgBorder.Visibility = Visibility.Hidden;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void weightQtyLabel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.item.weight_qty = (float)weightQtyLabel.Value;
            this.parentWindow.RefreshData();


           // MessageBox.Show(weightQtyLabel.Value.ToString());
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parentWindow.Activate();
            parentWindow.ScanTB.Focus();
            parentWindow.ScanTB.SelectAll();
            lightController.Close();
        }

        private void cleanNetWeightBtn_Click(object sender, RoutedEventArgs e)
        {
            net_weight = 0;
            alertLabel.Content = "";

            partImageBorder.BorderThickness = new Thickness(0);
            netWeightLabel.Visibility = Visibility.Hidden;

            net_weighted = false;

            actualWeightTB.Focus();
            actualWeightTB.SelectAll();
        }


    }
}
