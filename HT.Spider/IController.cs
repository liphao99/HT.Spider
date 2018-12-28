using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Spider
{
    /// <summary>
    /// 建议将IController作为整个抓取过程控制器
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 控制整个抓取流程
        /// </summary>
        Task<bool> StartCrawling();
    }
}
