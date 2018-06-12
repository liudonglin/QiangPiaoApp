using Newtonsoft.Json;
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

namespace QiangDanApp
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            QueryHasTask();
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

            if (missionResult.code != 1)
            {
                //doLogin();
            }
        }


    }
}
