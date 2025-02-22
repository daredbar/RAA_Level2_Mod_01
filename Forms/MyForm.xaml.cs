﻿using Microsoft.Win32;
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


namespace RAA_Level2_Mod_01
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MyForm : Window
    {
        public MyForm()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = @"C:\";
            openFile.Filter = "csv files (*.csv)|*.csv";

            if (openFile.ShowDialog() == true)
            {
                tbxFile.Text = openFile.FileName;
            }
            else
            {
                tbxFile.Text = "";
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public string GetTextBoxValue()
        {
            return tbxFile.Text;
        }

        public string GetGroup1()
        {
            if (rb1.IsChecked == true)
                return rb1.Content.ToString();
            else 
                return rb2.Content.ToString();
        }
        public bool GetCheckbox1()
        {
            if (chbCheck1.IsChecked == true)
                return true;
            else
                return false;
        }

        public bool GetCheckbox2()
        {
            if (chbCheck2.IsChecked == true)
                return true;
            else
                return false;
        }
        public bool GetCheckbox3()
        {
            if (chbCheck3.IsChecked == true)
                return true;
            else
                return false;
        }
    }
}
