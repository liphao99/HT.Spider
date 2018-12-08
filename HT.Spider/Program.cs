using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            Spider_ZHIHU_HOT hOT = new Spider_ZHIHU_HOT();
            hOT.Download(new Uri("https://www.zhihu.com/hot")).Wait();
            foreach(var i in hOT.list)
            {
                System.Console.WriteLine(i.ToString());
            }
        }
    }
}
