using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// http://yc.xmaylt.cc/app/userpage/listtask
    /// </summary>
    public partial class MainPage : Page
    {
        private MainWindow _window;
        private DateTime startTime;
        private int runCount;

        public List<TaskInfo> addTaskList = new List<TaskInfo>();

        public MainPage(MainWindow window)
        {
            InitializeComponent();
            this._window = window;
            this.Loaded += MainPage_Loaded;
            startTime = DateTime.Now;

            runtime_txt.Text = "0 秒";
            runcount_txt.Text = runCount.ToString();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetUserInfoBox(HttpUtility.CurrentUser);
            
            QueryHasTask();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(3000);
                    QueryWaitHasTask();

                    runCount++;

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        TimeSpan ts = DateTime.Now - startTime;
                        runtime_txt.Text = ((int)ts.TotalSeconds).ToString() + " 秒";
                        runcount_txt.Text = runCount.ToString();
                    }));
                }
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

            this.dayLimit_txt.Text = user.dayLimit.ToString();
            this.monthLimit_txt.Text = user.monthLimit.ToString();
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

            if (missionResult.code == 911)//登陆失效
            {
                HttpUtility.DoLogin();
            }
            else if(missionResult.code == 1)
            {
                this.HasTaskTable.ItemsSource = missionResult.missionList;
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
            else if(taskResult.code == 1)
            {
                if (taskResult.taskList!=null&& taskResult.taskList.Count > 0)
                {
                    _window.NoticeMessage("任务已发布,开始自动抢单");

                    for (int i = 0; i < 2 && i < taskResult.taskList.Count;)
                    {
                        var _task = taskResult.taskList[i];
                        var addResult = AddTask(_task.taskid);
                        if (addResult.code == 1)
                        {
                            _task.price = _task.price / 100;
                            addTaskList.Add(_task);
                            this.NewTaskTable.ItemsSource = addTaskList;
                            i++;
                            _window.NoticeMessage("已获取任务,任务编号" + addResult.missionid);
                        }
                    }
                }
            }
            else
            {
                _window.NoticeMessage(taskResult.message);
            }
        }

        private AddTaskResult AddTask(int taskid)
        {
            var url = "http://yc.xmaylt.cc/app/mission/addpost";
            var postData = "{ \"taskid\":" + taskid + "}";

            var json = HttpUtility.HttpAjaxPost(url, postData);

            AddTaskResult taskResult = JsonConvert.DeserializeObject<AddTaskResult>(json);

            return taskResult;
        }

    }
}
