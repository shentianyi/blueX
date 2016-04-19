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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PLCLightCL;

namespace PLCLightWPFTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        PlcLight pl;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pl = new PlcLight("COM13");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            pl.Play(CommandType.ON, GetIndexes());
            // new SecondWindow().Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            pl.Play(CommandType.OFF, GetIndexes());
        }

       

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            pl.Play(CommandType.ALL_OFF_BEFORE_ON, GetIndexes());
        }
        
        private List<int> GetIndexes()
        {
            List<int> indexes = new List<int>();
            foreach (string s in textBox1.Text.Split(','))
            {
                indexes.Add(int.Parse(s));
            }
            return indexes;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            pl.Play(CommandType.ALL_ON);
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            pl.Play(CommandType.ALL_OFF);
        }

    }
}
