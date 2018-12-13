using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Spider
{
    interface IController
    {
        /// <summary>
        /// 控制整个抓取流程
        /// </summary>
        void StartCrawling();
    }
}
