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

        public PickWeightWindow()
        {
            InitializeComponent();
        }


        public PickWeightWindow(PickItem item,PickListWindow parentWindow)
        {
            InitializeComponent();
            this.item = item;
            this.parentWindow = parentWindow;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (BaseConfig.PlayPickPositionVoice)
            {
                new VoiceHelper() { Text = this.item.positions_nr.Split(',').First(), Times = 2 }.Speak();
            }

            new VoiceHelper() { Text = "请去皮!" }.Speak();

            minWeight = item.quantity * (item.part.weight * (1 - item.part.weight_range));// +item.order_box.weight;
            maxWeight = item.quantity * (item.part.weight * (1 + item.part.weight_range));// +item.order_box.weight;
            standWeight = item.part.weight * item.quantity; //+ item.order_box.weight;

            partNrLabel.Content = item.part_nr;
            positionLabel.Content = item.positions_nr;
            unitWeightLabel.Content = item.part.weight;
            qtyLabel.Content = item.quantity;
            
            standWeightLabel.Content =minWeight+"-("+ standWeight+")-"+maxWeight;
            qtyLabel.Content = (int)Math.Round(minWeight / this.item.part.weight) + "-(" + item.quantity + ")-" + (int)Math.Round(maxWeight / this.item.part.weight);
            actualWeightTB.Focus();

            try
            {
                string path = PathHelper.GetImagePath(item.part_nr);
                if (path != null)
                {
                    BitmapImage i = new BitmapImage(new Uri(path, UriKind.Absolute));
                    partImage.Source = i;
                }
            }
            catch { }
        }

       

        private void actualWeightTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key && actualWeightTB.Text.Trim().Length > 0) {
                actualWeightTB.Text = actualWeightTB.Text.Trim();
                actualWeightTB.SelectAll();
                validateWeightAndVoice();
            }
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void validateWeightAndVoice() {
            validateWeight();
            if (valid || (valid == false && BaseConfig.ForceWeightPass == false))
            {
                weightOrderBox();
                this.Close();
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


        private void validateWeight()
        {
            valid = false;
            float weight = 0;
            if (float.TryParse(actualWeightTB.Text.Trim(), out weight))
            {
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

                    this.parentWindow.RefreshData();
                    new VoiceHelper() { Text = "合格" }.Speak();

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

                        new VoiceHelper() { Text = "太轻，还差" + mweight_qty + "个" }.Speak();
                    }
                    else if(weight>maxWeight) {
                        int mweight_qty = (int)Math.Round(( weight-standWeight) / this.item.part.weight);
                        new VoiceHelper() { Text = "太重，多了" + mweight_qty + "个" }.Speak();
                    }


                }


            }
            else
            {
                MessageBox.Show("重量未在范围内");
            }

            actualWeightTB.Focus();
            actualWeightTB.SelectAll();
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


    }
}
