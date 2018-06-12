using Newtonsoft.Json;
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

namespace QiangDanApp
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        private MainWindow _window;

        public MainPage(MainWindow window)
        {
            InitializeComponent();
            this._window = window;
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetUserInfoBox(HttpUtility.CurrentUser);

            QueryWaitHasTask();
            QueryHasTask();

            Task.Factory.StartNew(() =>
            {

            });
        }

        private void SetUserInfoBox(UserInfo user)
        {
            if (user == null) return;
            this.taobao_txt.Text = user.taobao;
            this.mobile_txt.Text = user.mobile;
            this.name_txt.Text = user.name;
            this.weChat_txt.Text = user.weChat;

            this.userid_txt.Text = user.userid;
            this.freeMoney_txt.Text = (user.freeMoney/100).ToString();
            this.stockMoney_txt.Text = (user.stockMoney/100).ToString();
            this.lastReceive_txt.Text = user.lastReceive;
        }

        private void QueryHasTask()
        {
            //查询已结任务
            var queryUrl = "http://yc.xmaylt.cc/app/mission/listpost";
            var postData = "{\"pageNo\":0,\"pageSize\":0,\"order\":{\"createtime\":-1},\"type\":1}";

            var json = HttpUtility.HttpAjaxPost(queryUrl, postData);

            MissionResult missionResult = JsonConvert.DeserializeObject<MissionResult>(json);
            if (missionResult.missionList != null)
            {
                foreach (var item in missionResult.missionList)
                {
                    item.price = item.price / 100;
                    item.userPay = item.userPay / 100;
                }
            }

            this.HasTaskTable.ItemsSource = missionResult.missionList;

            if (missionResult.code == 911)//登陆失效
            {
                HttpUtility.DoLogin();
            }
        }

        private void QueryWaitHasTask()
        {
            //查询已结任务
            var queryUrl = "http://yc.xmaylt.cc/app/task/listpost";
            var postData = "{\"pageNo\":0,\"pageSize\":0,\"order\":{\"taskid\":1},\"appoint\":0}";

            var json = HttpUtility.HttpAjaxPost(queryUrl, postData);

            TaskResult taskResult = JsonConvert.DeserializeObject<TaskResult>(json);

            if (taskResult.code == 911)//登陆失效
            {
                HttpUtility.DoLogin();
            }
        }

    }
}
