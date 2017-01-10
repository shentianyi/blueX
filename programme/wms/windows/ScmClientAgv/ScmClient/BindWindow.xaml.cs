using MahApps.Metro.Controls;
using ScmWcfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScmClient
{
    /// <summary>
    /// BindWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BindWindow : Window
    {
        public BindWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// window加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetNotBindOff();
            boxIdTB.Focus();
            var cs = this.LightButtonGrid.Children;
            int i = 1;
            foreach (var c in cs)
            {
                if(c is Button)
                {
                    (c as Button).Click += Led_Click;
                    (c as Button).Tag = i;
                    (c as Button).BorderBrush = Brushes.White;
                    i++;
                }
            }
            lightIdTB.Text = "1";
            SetLightWaitBind(lightIdTB.Text);
        }

        /// <summary>
        /// 灯按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Led_Click(object sender, RoutedEventArgs e)
        {
            SetNotBindOff();

            Button b = sender as Button;
            //MessageBox.Show(b.Tag.ToString());
            lightIdTB.Text = b.Tag.ToString();
            SetLightWaitBind(lightIdTB.Text);
            boxIdTB.Focus();
        }

        private Button FindByTag(string tag)
        {
            var cs = this.LightButtonGrid.Children;
            foreach (var c in cs)
            {
                if (c is Button)
                {
                    if((c as Button).Tag.ToString() == tag)
                    {
                        return (c as Button);
                    }
                }
            }
            return null;
        }
         

        private void SetNotBindOff()
        {
            var cs = this.LightButtonGrid.Children;
            foreach (var c in cs)
            {
                if (c is Button)
                {
                    if((c as Button).Background != Brushes.Green)
                    {
                        (c as Button).Background = Brushes.Transparent;
                    }
                }
            }

        }

        private void SetLightWaitBind(string tag)
        {
            Button b = FindByTag(tag);
            if (b != null)
            {
                b.Background = Brushes.Yellow;
            }
        }

        private void SetLightBinded(string tag)
        {
            Button b = FindByTag(tag);
            if (b != null)
            {
                b.Background = Brushes.Green;
            }
        }

        public void SendLightOnCmd(string tag)
        {

        }


        private void boxIdTB_KeyDown(object sender, KeyEventArgs e)
        {
            OrderService os = new OrderService();

            if (e.Key == Key.Enter)
            {
                //bind
                var msg = os.BindBoxAndLed(boxIdTB.Text, lightIdTB.Text);
                if (msg.http_error)
                {
                    MessageBox.Show(msg.Message);
                }
                else if (!msg.Success)
                {
                    MessageBox.Show(msg.meta.message);
                }
                else
                {
                    boxIdTB.Focus();
                    SetLightBinded(lightIdTB.Text);
                    SetNotBindOff();
                    string tag = (int.Parse(lightIdTB.Text) + 1).ToString();
                    if (int.Parse(tag) > this.LightButtonGrid.Children.Count)
                    {
                        MessageBox.Show("已经完成绑定！");
                    }
                    else
                    {
                        lightIdTB.Text = tag;
                        SetLightWaitBind(lightIdTB.Text);
                    }
                }
     
                boxIdTB.Clear();
            }
        }

        private void lightIdTB_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Button b = FindByTag(lightIdTB.Text);
                if(b!= null){
                    SetNotBindOff();
                    SetLightWaitBind(lightIdTB.Text);
                    //if (b.Background == null)
                    //{
                    //    b.Background = Brushes.Yellow;
                    //}
                    boxIdTB.Focus();
                }else
                {
                    MessageBox.Show("小灯ID不存在，请重新输入！");
                }
            }
        }

        private void BindFinishBtn_Click(object sender, RoutedEventArgs e)
        {
             this.Close();
        }
    }
}
