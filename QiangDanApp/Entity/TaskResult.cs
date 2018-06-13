using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QiangDanApp
{
    public class TaskResult
    {
        public int code { get; set; }

        public string message { get; set; }

        public List<TaskInfo> taskList { get; set; }
    }

    public class TaskInfo
    {
        public int taskid { get; set; }

        public int shopid { get; set; }

        public string shopName { get; set; }

        public string shopCategory { get; set; }

        public int price { get; set; }

        public int amount { get; set; }

        public int interval { get; set; }

        public string platformName { get; set; }
    }
}
