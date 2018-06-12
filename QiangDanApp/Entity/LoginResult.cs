using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QiangDanApp
{
    public class LoginResult
    {
        public int code { get; set; }

        public string PHPSESSID { get; set; }


        public UserInfo user { get; set; }
    }
}
