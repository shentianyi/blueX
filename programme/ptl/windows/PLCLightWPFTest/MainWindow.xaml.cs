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
using PLCLightCL;
using PLCLightCL.Light;
using PLCLightCL.Enum;
using System.Threading;
 

namespace PLCLightWPFTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
       // PlcLightController pl;

        ILightController lightController;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> coms = new List<string>();
            for (int i = 0; i < 30; i++)
            {
                coms.Add("COM" + (i + 1).ToString());
            }
            comboBox1.ItemsSource = coms;
            comboBox1.SelectedIndex = 0;
            button8.IsEnabled = false;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           // pl.Play(LightCmdType.ON, GetIndexes());
            // new SecondWindow().Show();
            lightController.Play(LightCmdType.ON, GetIndexes());
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //pl.Play(LightCmdType.OFF, GetIndexes());

            lightController.Play(LightCmdType.OFF, GetIndexes());
        }

       

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //pl.Play(LightCmdType.ALL_OFF_BEFORE_ON, GetIndexes());

            lightController.Play(LightCmdType.ALL_OFF_BEFORE_ON, GetIndexes());
        }
        
        private List<int> GetIndexes()
        {
            List<int> indexes = new List<int>();
            if (findLightCB.IsChecked.Value)
            {
               // indexes.Add(int.Parse(indexTB.Text));
                foreach (var i in findListBox.Items) {
                    indexes.Add(int.Parse(i.ToString()));
                }
            }
            else {
                foreach (string s in textBox1.Text.Split(','))
                {
                    indexes.Add(int.Parse(s));
                }
            }
            return indexes;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
           // pl.Play(LightCmdType.ALL_ON);

            lightController.Play(LightCmdType.ALL_ON, GetIndexes());
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {
           // pl.Play(LightCmdType.ALL_OFF);

            lightController.Play(LightCmdType.ALL_OFF, GetIndexes());
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // pl = new PlcLightController(comboBox1.SelectedValue.ToString());

                if (controllerCB.SelectedIndex == 0)
                {
                    lightController = new RamLightController(comboBox1.SelectedValue.ToString());
                }
                else
                {
                    lightController = new PlcLightController(comboBox1.SelectedValue.ToString());
                }
                button8.IsEnabled = true;
                button6.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            new SecondWindow().Show();
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            if (lightController != null) {
                lightController.Close();
            }
            button6.IsEnabled = true;
            button8.IsEnabled = false;
        }

        private void onRB_Checked(object sender, RoutedEventArgs e)
        {
            findListBox.Items.Clear();
            indexTB.Text = "-1";

        }

        private void prevBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                int current = int.Parse(indexTB.Text);
                int next = -1;
                if (button.Name.Equals("prevBtn"))
                {
                    if (current > 0)
                    {
                        next = current - 1;

                        //findListBox.Items.Add(next);
                    }
                }
                else
                {
                    if (current < 111)
                    {
                        next = current + 1;

                    }
                }

                if (next != -1)
                {
                    indexTB.Text = next.ToString();

                    if (!keepCB.IsChecked.Value)
                    {
                        findListBox.Items.Clear();
                    }

                    findListBox.Items.Add(next);
                }

                if (onRB.IsChecked.Value)
                {
                    lightController.Play(LightCmdType.ALL_OFF_BEFORE_ON, GetIndexes());
                }
                else
                {
                    lightController.Play(LightCmdType.OFF, GetIndexes());
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
