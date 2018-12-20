using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HT.Spider
{
    public interface ISpider
    {
        /// <summary>
        /// 发起请求前执行的事件
        /// </summary>
        event EventHandler<OnStartEventArgs> OnStart;

        /// <summary>
        /// 如果需要对request进行设置可使用该事件，否则保持为null
        /// </summary>
        event EventHandler<HttpWebRequest> OnSetting;
        
        /// <summary>
        /// 获取请求结果后执行的事件
        /// </summary>
        event EventHandler<OnCompletedEventArgs> OnCompleted;

        /// <summary>
        /// 对异常的处理事件
        /// </summary>
        event EventHandler<Exception> OnError;

        /// <summary>
        /// cookie的容器
        /// </summary>
        CookieContainer CookiesContainer { get; set; }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="uri">资源的位置标识符</param>
        /// <param name="proxy">代理ip地址</param>
        /// <returns>一个异步任务，该任务对一个String类型的变量赋值</returns>
        Task<String> GetFunc(Uri uri,WebProxy proxy=null);

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="uri">资源的位置标识符</param>
        /// <param name="postData">post请求参数</param>
        /// <param name="proxy">代理ip地址</param>
        /// <returns>一个异步任务，该任务对一个String类型的变量赋值</returns>
        Task<String> PostFunc(Uri uri, String postData, WebProxy proxy = null);
    }
}
