using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QiangDanApp
{
    public class MissionResult
    {
        public MissionResult()
        {
            missionList = new List<Missionentity>();
        }

        public int code;

        public string message;

        public List<Missionentity> missionList;
    }

    public class Missionentity
    {
        public string taskid { get; set; }

        public string userid { get; set; }

        public string taobao { get; set; }

        public string userName { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        public string missionCode { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public int price { get; set; }

        /// <summary>
        /// 服务佣金
        /// </summary>
        public int userPay { get; set; }
    }
}
