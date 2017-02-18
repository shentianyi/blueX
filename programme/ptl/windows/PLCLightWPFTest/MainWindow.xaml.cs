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
using Brilliantech.Framwork.Utils.LogUtil;
using PLCLightWPFTest.Data;
using PLCLightWPFTest.Properties;

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
            IncresCount();
            lightController.Play(LightCmdType.ALL_ON, GetIndexes());
        }
        int count = 0;
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            // pl.Play(LightCmdType.ALL_OFF);
            IncresCount();
            lightController.Play(LightCmdType.ALL_OFF, GetIndexes());
        }

        public void IncresCount()
        {
            count++;
            countLab.Content = count;
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // pl = new PlcLightController(comboBox1.SelectedValue.ToString());

                if (controllerCB.SelectedIndex == 0)
                {
                    lightController = new RamLightController(comboBox1.SelectedValue.ToString());
                }else if (controllerCB.SelectedIndex == 1)
                {
                    lightController = new CanLightController(tcpServer.Text.Split(':')[0], int.Parse(tcpServer.Text.Split(':')[1]), int.Parse(tcpServer.Text.Split(':')[2]));
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
                    if (controllerCB.SelectedIndex == 1)
                    {
                        if (current < 255)
                        {
                            next = current + 1;
                        }
                    }
                    else
                    {
                        if (current < 111)
                        {
                            next = current + 1;

                        }
                    }
                }

                if (next != -1)
                {
                    indexTB.Text = next.ToString();
                    lightIdTB.Text = next.ToString();

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
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            new Menu().Show();
            // this.Close();


        }

        System.Timers.Timer t;
        int c = 0;
        private void autoOFFONBtn_Click(object sender, RoutedEventArgs e)
        {
            if (t != null)
            {
                t.Stop();
                t = null;
            }
            else
            {
                t = new System.Timers.Timer();
                t.Interval = int.Parse(timerInterval.Text);
                t.Elapsed += T_Elapsed;
                t.Enabled = true;
                t.Start();
            }
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            this.Dispatcher.Invoke(new Action(() => {

                if (c % 2 == 0)
                {
                    button4_Click(null, null);
                }
                else
                {
                    button5_Click(null, null);
                }
                c++;

            }));
            t.Start();
        }

        private void binglightBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Bind();
            // new BindLightWindow((this.controllerCB.SelectedItem.ToString()).Split(':')[1].Trim()).Show();
        }















        private void partTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox t = sender as TextBox;
                if (!string.IsNullOrEmpty(t.Text))
                {
                    switch (t.Name)
                    {
                        case "partTB":
                            lightIdTB.Focus();
                            break;
                        case "lightIdTB":
                            this.Bind();
                            partTB.Focus();
                            break;
                        default:
                            partTB.Focus();
                            break;
                    }
                    if (t.Name == "partTB") { }
                }
            }
        }

        private void Bind()
        {
            try
            {
                if (Validate())
                {
                    var c = new AutoWorkDbDataContext(Settings.Default.awDb);
                    var b = c.ELabelOnForPartOnWorkstation.FirstOrDefault(s => s.workstationId == workpalceTB.Text && s.partNr == partTB.Text);
                    if (b == null)
                    {
                        b = new ELabelOnForPartOnWorkstation()
                        {
                            workstationId = workpalceTB.Text,
                            partNr = partTB.Text,
                            controlType = (this.controllerCB.SelectedItem.ToString()).Split(':')[1].Trim(),//controllTypeLab.Content.ToString(),
                            labelAddr = lightIdTB.Text
                        };
                        c.ELabelOnForPartOnWorkstation.InsertOnSubmit(b);
                        c.SubmitChanges();
                        this.ClearInput();
                    }
                    else
                    {
                        b.controlType = (this.controllerCB.SelectedItem.ToString()).Split(':')[1].Trim();//controllTypeLab.Content.ToString();
                        b.labelAddr = lightIdTB.Text;
                        c.SubmitChanges();
                        this.ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(workpalceTB.Text))
            {
                MessageBox.Show("input workplace");
                return false;
            }


            if (string.IsNullOrWhiteSpace(partTB.Text))
            {
                MessageBox.Show("input part");
                return false;
            }

            if (string.IsNullOrWhiteSpace(lightIdTB.Text))
            {
                MessageBox.Show("input lightId");
                return false;
            }
            return true;
        }

        private void ClearInput()
        {
            this.partTB.Text = string.Empty;
            this.lightIdTB.Text = string.Empty;
            this.partTB.Focus();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Bind();
        }

        private void button7_Copy_Click(object sender, RoutedEventArgs e)
        {
            new CanServerWindow().Show();
        }
    }
}
