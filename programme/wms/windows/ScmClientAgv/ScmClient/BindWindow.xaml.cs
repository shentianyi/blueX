using MahApps.Metro.Controls;
using PLCLightCL.Light;
using ScmWcfService;
using ScmWcfService.Config;
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
        PickListWindow parentWindow;
        ILightController lc = null;

        public BindWindow()
        {
            InitializeComponent();
        }

        public BindWindow(PickListWindow parentWindow, ILightController lc)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.lc = lc;
        }

        private Dictionary<string, string> bindList = new Dictionary<string, string>();
        /// <summary>
        /// window加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             //lc = new RamLightController(ServerConfig.ptlComPort);
            SetNotBindOff();
            boxIdTB.Focus();
            var cs = this.LightButtonGrid.Children;
            int i = 0;
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
            lightIdTB.Text = "0";
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
        /// <summary>
        /// 通过当前的小灯tag查找下一个待绑定的灯
        /// </summary>
        /// <param name="curTag"></param>
        /// <returns></returns>
        private Button FindNextNotBindByTag(string curTag)
        {
            var cs = this.LightButtonGrid.Children;
            foreach (var c in cs)
            {
                if (c is Button)
                {
                    var btn = c as Button;
                    if (int.Parse((btn.Tag).ToString()) > int.Parse(curTag) && btn.Background == Brushes.Transparent)
                    {
                        return btn;
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
                // 按钮显示黄色，灯亮起。
                b.Background = Brushes.Yellow;
                //SendLightOnCmd(tag);
            }
        }

        private void SetLightBinded(string tag)
        {
            Button b = FindByTag(tag);
            if (b != null)
            {
                // 按钮显示绿色，灯关闭。
                b.Background = Brushes.Green;
                SendLightOnCmd(tag);                                                               
                //SendLightOffCmd(tag);
            }
        }
        /// <summary>
        /// 亮起小灯
        /// </summary>
        /// <param name="tag"></param>
        public void SendLightOnCmd(string tag)
        {
            lc.Play(PLCLightCL.Enum.LightCmdType.ON,new List<int> { int.Parse(tag)});
        }

        /// <summary>
        /// 关闭小灯
        /// </summary>
        /// <param name="tag"></param>
        public void SendLightOffCmd(string tag)
        {
            ILightController lc = new LightController();
            lc.Play(PLCLightCL.Enum.LightCmdType.OFF, new List<int> { int.Parse(tag) });
        }

        private void boxIdTB_KeyDown(object sender, KeyEventArgs e)
        {
            // 验证是否已经绑定过
            if (bindList.Values.Contains(boxIdTB.Text))
            {
                foreach(var binded in bindList)
                {
                    if (binded.Value.Equals(boxIdTB.Text))
                    {
                        MessageBox.Show("料盒" + boxIdTB.Text + "已经绑定到小灯" + binded.Key+",请重新选择！");
                    }
                }
            }else
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
                        // 绑定后将lightIdTB、boxIdTB存入字典中，以便验证
                        bindList.Add(lightIdTB.Text, boxIdTB.Text);
                        try
                        {
                            var btn = FindNextNotBindByTag(lightIdTB.Text);
                            if (btn != null)
                            {
                                string tag = btn.Tag.ToString();
                                lightIdTB.Text = tag;
                                SetLightWaitBind(lightIdTB.Text);
                            }
                            else
                            {
                                MessageBox.Show("已经完成绑定！");
                            }
                            boxIdTB.Clear();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
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
                    boxIdTB.Focus();
                }else
                {
                    MessageBox.Show("小灯ID不存在，请重新输入！");
                }
            }
        }

        private void BindFinishBtn_Click(object sender, RoutedEventArgs e)
        {
            lc.Play(PLCLightCL.Enum.LightCmdType.ALL_OFF);
             this.Close();
                
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            lc.Close();
        }
    }
}
