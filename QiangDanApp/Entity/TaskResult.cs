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
        public string taskid { get; set; }

        public string shopid { get; set; }

        public string shopName { get; set; }

        public string shopCategory { get; set; }

        public string price { get; set; }

        public string amount { get; set; }

        public string interval { get; set; }

        public string platformName { get; set; }
    }
}
