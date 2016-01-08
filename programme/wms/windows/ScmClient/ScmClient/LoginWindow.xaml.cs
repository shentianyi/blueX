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
using ScmWcfService;
using ScmWcfService.Model;
using ScmWcfService.Model.Message;
using ScmWcfService.Config;

namespace ScmClient
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            initControl();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApiConfig.AutoToken && ApiConfig.Token != null && ApiConfig.Token.Length > 0)
            {
                new MenuWindow().Show();
                this.Close();
            }
        }

        /// <summary>
        /// 点击登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            login();
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        private void login()
        {
            var service = new UserService();
            ResponseMessage<UserSession> msg = service.Login(NrTB.Text, PwdTB.Password);
            if (msg.Success)
            {
                ApiConfig.Token = msg.data.token;
                new MenuWindow().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(msg.Message);
                initControl();
            }
        }

        /// <summary>
        /// 密码框回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PwdTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && PwdTB.Password.Trim().Length > 0)
            {
                login();
            }
        }

        /// <summary>
        /// 初始化控件显示
        /// </summary>
        private void initControl()
        {
            PwdTB.Password = string.Empty;
            NrTB.Focus();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
