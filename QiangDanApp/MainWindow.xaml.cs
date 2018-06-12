﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QiangDanApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginPage _LoginPage;
        private MainPage _MainPage;

        public MainWindow()
        {
            InitializeComponent();

            _LoginPage = new LoginPage(this);
            _MainPage = new MainPage(this);

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(HttpUtility.PHPSESSID))
            {
                GoToLoginPage();
            }
            else
            {
                GoToMainPage();
            }
        }

        public void GoToLoginPage()
        {
            this.MainFrame.Content = this._LoginPage;
        }

        public void GoToMainPage()
        {
            this.MainFrame.Content = this._MainPage;
        }
    }
}
