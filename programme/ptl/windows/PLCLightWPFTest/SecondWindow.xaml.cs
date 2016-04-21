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
using PLCLightCL;
using PLCLightCL.Model;
using PLCLightWPFTest.Properties;

namespace PLCLightWPFTest
{
    /// <summary>
    /// SecondWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SecondWindow : Window
    {
      static  string conn = "Data Source=svp37001.p37.leoni.local;Initial Catalog=LEPS;Persist Security Info=True;User ID=ASS_user;Password=ass_user";
        //GetHeadRecord();
      LEPSController lc;
        List<string> modules = null;

        public SecondWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lc = new LEPSController(conn);

            boardTB.Text = Settings.Default.Board;
            lineTB.Text = Settings.Default.Line;
            workplaceTB.Text = Settings.Default.Workplace;

        }

        private void startBT_Click(object sender, RoutedEventArgs e)
        {
            HeadMessage msg = lc.StartAndGetHarnessByBoard(boardTB.Text, workplaceTB.Text);
            if (msg != null) {
                kskTB.Text = msg.KSK;
            }
        }

        private void getModuleBtn_Click(object sender, RoutedEventArgs e)
        {
            modules = new List<string>();
            modules = lc.GetBasicModule(workplaceTB.Text, kskTB.Text);
            string s = string.Empty;
            foreach (string ss in modules) {
                s +=ss+ ";";
            }
            moduleLab.Content = s;
        }

        private void akBtn_Click(object sender, RoutedEventArgs e)
        {
           
            foreach (string ss in modules)
            {
                lc.AKBasicModule(lineTB.Text, workplaceTB.Text, kskTB.Text,ss);
            }
        }

        private void completeBtn_Click(object sender, RoutedEventArgs e)
        {
            lc.CompleteHarness(boardTB.Text, workplaceTB.Text, kskTB.Text);
        }


    }
}
