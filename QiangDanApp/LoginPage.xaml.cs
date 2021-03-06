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

namespace QiangDanApp
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        private MainWindow _window;

        public LoginPage(MainWindow window)
        {
            InitializeComponent();
            this._window = window;
            this.Loaded += LoginPage_Loaded;
        }

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.loginName_Txt.Text = Properties.Settings.Default.LoginName;
            this.passwordTxt.Password = Properties.Settings.Default.Password;
            HttpUtility.GetSessid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginnane = this.loginName_Txt.Text.Trim();
            var passwor = this.passwordTxt.Password.Trim();

            if (string.IsNullOrWhiteSpace(loginnane))
            {
                MessageBox.Show("请输入账号");
                return;
            }
            if (string.IsNullOrWhiteSpace(loginnane))
            {
                MessageBox.Show("请输入密码");
                return;
            }

            HttpUtility.LoginName = loginnane;
            HttpUtility.Password = passwor;

            if (HttpUtility.DoLogin())
            {
                this._window.GoToMainPage();
            }
            else
            {
                MessageBox.Show("登陆失败，请检查账号和密码");
            }
        }
    }
}
