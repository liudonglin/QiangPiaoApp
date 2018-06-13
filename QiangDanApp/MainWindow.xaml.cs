using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QiangDanApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginPage _LoginPage;
        private MainPage _MainPage;

        WindowState lastWindowState;
        private NotifyIcon notifyIcon;

        private Bitmap GetBitmap(BitmapImage image1)
        {
            BitmapSource m = image1;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            m.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride); bmp.UnlockBits(data);
            return bmp;
        }


        public MainWindow()
        {
            InitializeComponent();

            bitmap1 = new BitmapImage(new Uri("pack://application:,,,/QiangDanApp;component/Resources/App_min_1.ico", UriKind.Absolute));
            App_min_1 = System.Drawing.Icon.FromHandle(GetBitmap(bitmap1).GetHicon());
            bitmap2 = new BitmapImage(new Uri("pack://application:,,,/QiangDanApp;component/Resources/App_min_2.ico", UriKind.Absolute));
            App_min_2 = System.Drawing.Icon.FromHandle(GetBitmap(bitmap2).GetHicon());

            //不在任务栏显示
            this.ShowInTaskbar = false;
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Text = "抢单系统";
            this.notifyIcon.Icon = App_min_1;
            this.notifyIcon.Visible = true;

            this.notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            //打开菜单项
            System.Windows.Forms.MenuItem about = new System.Windows.Forms.MenuItem("About");
            about.Click += new EventHandler(OnAboutClick);
            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Open");
            open.Click += new EventHandler(OpenWindow);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(CloseWindow);

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { about,open, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            icoTimer.Interval = TimeSpan.FromSeconds(0.35);
            icoTimer.Tick += new EventHandler(IcoTimer_Tick);

            _LoginPage = new LoginPage(this);
            _MainPage = new MainPage(this);

            this.Loaded += MainWindow_Loaded;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = WindowState.Normal;
            icoTimer.Stop();
            this.notifyIcon.Text = "抢单系统";
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

        public void NoticeMessage(string msg)
        {
            //闪烁图标
            icoTimer.Start();
            this.notifyIcon.Text = msg;
            ShowBalloonTipText(msg);
        }

        public void GoToLoginPage()
        {
            this.MainFrame.Content = this._LoginPage;
        }

        public void GoToMainPage()
        {
            this.MainFrame.Content = this._MainPage;
        }

        #region NotifyIcon
        private void OpenWindow(object sender, EventArgs e)
        {
            Show();
            this.WindowState = WindowState.Normal;
        }

        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            lastWindowState = WindowState;
            if (lastWindowState == WindowState.Minimized)
            {
                this.Hide();
                ShowBalloonTipText("抢单程序后台运行中......");
            }
        }

        private void OnAboutClick(object sender, EventArgs e)
        {
            System.Windows.MessageBox.Show("作者：刘东林, 创建日期：2018-06-12");
        }

        private void ShowBalloonTipText(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            this.notifyIcon.BalloonTipText = msg;
            this.notifyIcon.ShowBalloonTip(3000);
        }
        #endregion

        #region 闪烁
        DispatcherTimer icoTimer = new DispatcherTimer();
        private BitmapImage bitmap1 = null;
        private BitmapImage bitmap2 = null;
        private Icon App_min_1 = null;
        private Icon App_min_2 = null;
        private bool _status;
        private void IcoTimer_Tick(object sender, EventArgs e)
        {
            if (_status)
                this.notifyIcon.Icon = App_min_1;
            else
                this.notifyIcon.Icon = App_min_2;

            _status = !_status;
        }
        #endregion
    }
}
