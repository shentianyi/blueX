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
using PLCLightWPFTest.Data;
using PLCLightWPFTest.Properties;

namespace PLCLightWPFTest
{
    /// <summary>
    /// BindLightWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BindLightWindow : Window
    {
        public BindLightWindow()
        {
            InitializeComponent();
        }
        public BindLightWindow(string type)
        {
            InitializeComponent();
            this.controllTypeLab.Content = type;
        }
        private void partTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox t=sender as TextBox;
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
                    if(t.Name== "partTB") { }
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
                            controlType = controllTypeLab.Content.ToString(),
                            labelAddr = lightIdTB.Text
                        };
                        c.ELabelOnForPartOnWorkstation.InsertOnSubmit(b);
                        c.SubmitChanges();
                        this.ClearInput();
                    }
                    else
                    {
                        b.controlType = controllTypeLab.Content.ToString();
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
    }
}
