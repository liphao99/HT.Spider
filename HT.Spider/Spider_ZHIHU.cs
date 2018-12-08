using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HT.Spider
{
    public class Spider_ZHIHU
    {
        
    }

    /// <summary>
    /// 爬取知乎热点的例子
    /// </summary>
    public class Spider_ZHIHU_HOT : HTSpider
    {
        public List<HotPoint> list;
        public Spider_ZHIHU_HOT()
        {
            //初始化热点列表
            list = new List<HotPoint>();

            //添加cookie实现模拟登录
            Cookie cookie = new Cookie("z_c0", "2|1:0|10:1544258229|4:z_c0|92:Mi4xdk04Z0F3QUFBQUFBRUdGU0x0bU1EU1lBQUFCZ0FsVk50ZEQ0WEFDR2RYNDd3UU9rUGZXNGY2bXl6bW82QWV2NUJB|eec6d9ebaa4334e00fa08bfa930f3d4ab9e565e6a0c55ee15edefd44b4791d46", "/", ".zhihu.com");
            this.CookiesContainer = new System.Net.CookieContainer();
            this.CookiesContainer.Add(cookie);

            //添加OnCompleted事件
            OnCompleted += Parse;

            //添加异常处理
            OnError += (s, e) =>
            {
                System.Console.WriteLine(e.Message);
            };
        }

        /// <summary>
        /// 对页面内容进行解析抓取
        /// </summary>
        /// <param name="sendor"></param>
        /// <param name="args"></param>
        private void Parse(object sendor, OnCompletedEventArgs args)
        {
            string strRef = @"(<div class=""HotItem-content"">)[\s|\S]+?<\/div><\/div><\/span><\/div><\/div>";//@"(href|HREF)[ ]*=[ ]*[""'][^""'#(img)]+[""']"
            MatchCollection matches = new Regex(strRef).Matches(args.PageSource);
            foreach (Match match in matches)
            {
                String url = new Regex(@"href="".+?""").Match(match.Value).Value.Substring(5).Trim('"');
                String title = new Regex(@"h2 class=""HotItem-title"".+?<").Match(match.Value).Value.Substring(25).Trim('<');
                String multiLine = String.Empty;
                try
                {
                    multiLine = new Regex(@"HotItem-excerpt"">.+?<").Match(match.Value).Value.Substring(17).Trim('<');
                }
                catch (Exception excep)
                {
                    //System.Console.WriteLine(excep.Message);
                }

                String degree = new Regex(@"<\/svg>.+?万").Match(match.Value).Value.Substring(6).Trim('万');
                list.Add(new HotPoint(url, multiLine, title, Convert.ToInt32(degree)));
            }
        }
    }


    public class HotPoint
    {
        public String Url { get; set; }

        public String MultiLine { get; set; }

        public String Title { get; set; }

        /// <summary>
        /// 热度（以万为单位）
        /// </summary>
        public int HotDegree { get; set; }

        public HotPoint(String url, String multiLine, String title, int hotDegree)
        {
            this.Url = url;
            this.MultiLine = multiLine;
            this.Title = title;
            this.HotDegree = hotDegree;
        }

        
        public override String ToString()
        {
            return Title + "     " + HotDegree + "万热度\n" + MultiLine + "\n超链接:" + Url+"\n------------------------\n";
        }
    }
}
