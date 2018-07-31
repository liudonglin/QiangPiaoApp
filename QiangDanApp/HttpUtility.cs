using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace QiangDanApp
{
    public class HttpUtility
    {
        static CookieContainer loginCookie = new CookieContainer();

        public static string LoginName { get; set; }

        public static string Password { get; set; }

        public static string PHPSESSID { get; set; }

        public static UserInfo CurrentUser { get; set; }

        public static string HttpAjaxPost(string Url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            string postDataStr = postData;
            request.ContentLength = Encoding.Default.GetByteCount(postDataStr);
            request.CookieContainer = loginCookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.Default);
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //var cookieStr = response.Headers["Set-Cookie"];
            //if (!string.IsNullOrWhiteSpace(cookieStr))
            //{
            //    CookiesParse(cookieStr);
            //}

            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.Default);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static void GetSessid()
        {
            string loginPageUrl = "http://yc.xmaylt.cc/app/userlogin/login";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginPageUrl);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            var cookieStr = response.Headers["Set-Cookie"];
            if (!string.IsNullOrWhiteSpace(cookieStr))
            {
                var cookieValue = cookieStr.Substring(cookieStr.IndexOf("=") + 1, 32);
                PHPSESSID = cookieValue;
                loginCookie.Add(new Cookie("PHPSESSID", cookieValue, "/", "yc.xmaylt.cc"));
            }
        }

        //public static void CookiesParse(string cookieStr)
        //{
        //    var cookieValue = GetCookieValue(cookieStr);
        //    var cookieName = "PHPSESSID";

        //    loginCookie = new CookieContainer();
        //    loginCookie.Add(new Cookie(cookieName, cookieValue, "/", "yc.xmaylt.cc"));
        //}

        //public static string GetCookieValue(string cookie)
        //{
        //    Regex regex = new Regex("=.*?;");
        //    Match value = regex.Match(cookie);
        //    string cookieValue = value.Groups[0].Value;
        //    return cookieValue.Substring(1, cookieValue.Length - 2);
        //}

        public static bool DoLogin()
        {
            string loginUrl = "http://yc.xmaylt.cc/app/userlogin/loginpost";
            var postData = "{\"mobile\":\"" + LoginName + "\",\"password\":\"" + Password + "\",\"openid\":\"\"}";
            var json = HttpAjaxPost(loginUrl, postData);

            LoginResult missionResult = JsonConvert.DeserializeObject<LoginResult>(json);

            if(missionResult!=null&& missionResult.code == 1)
            {
                CurrentUser = missionResult.user;

                Properties.Settings.Default.LoginName = LoginName;
                Properties.Settings.Default.Password = Password;
                Properties.Settings.Default.Save();
                return true;
            }
            else
            {
                CurrentUser = null;
                return false;
            }
        }
    }
}
