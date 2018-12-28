using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HT.Spider
{

    /// <summary>
    /// 
    /// </summary>
    public class HTSpider : ISpider
    {
        /// <summary>
        /// cookie的容器
        /// </summary>
        public CookieContainer CookiesContainer { get; set; }

        /// <summary>
        /// 如果需要对request进行设置可使用该事件，否则保持为null
        /// </summary>
        public event EventHandler<HttpWebRequest> OnSetting;

        /// <summary>
        /// 发起请求前执行的事件
        /// </summary>
        public event EventHandler<OnStartEventArgs> OnStart;

        /// <summary>
        /// 获取请求结果后执行的事件
        /// </summary>
        public event EventHandler<OnCompletedEventArgs> OnCompleted;

        /// <summary>
        /// 对异常的处理事件
        /// </summary>
        public event EventHandler<Exception> OnError;

        public async Task<String> GetFunc(Uri uri, WebProxy proxy=null)
        {
            return await Task.Run(() =>
            {
                string pageSource = string.Empty;
                try
                {
                    if (OnStart != null)
                    {
                        OnStart(this, new OnStartEventArgs(uri));
                    }
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Accept = "*/*";
                    request.ServicePoint.Expect100Continue = false;
                    request.ServicePoint.UseNagleAlgorithm = false;
                    request.AllowWriteStreamBuffering = false;
                    request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.AllowAutoRedirect = false;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0: WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.10";
                    request.Timeout = 50000000;
                    request.KeepAlive = true;
                    request.Method = "GET";
                    if (proxy != null)
                    {
                        request.Proxy = proxy;
                    }
                    request.CookieContainer = this.CookiesContainer;
                    request.ServicePoint.ConnectionLimit = int.MaxValue;
                    if (OnSetting != null)
                    {
                        OnSetting(this, request);
                    }
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        foreach (Cookie cookie in response.Cookies)
                        {
                            this.CookiesContainer.Add(cookie);
                        }
                        if(response.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else if(response.ContentType.ToLower().Contains("deflate"))
                        {
                            using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                    watch.Stop();
                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    var milliseconds = watch.ElapsedMilliseconds;
                    request.Abort();
                    if (OnCompleted != null)
                    {
                        OnCompleted(this, new OnCompletedEventArgs(uri, threadId, milliseconds, pageSource));
                    }

                }
                catch (Exception e)
                {
                    if (OnError != null)
                    {
                        OnError(this, e);
                    }
                }
                return pageSource;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task<string> PostFunc(Uri uri, string postData, WebProxy proxy = null)
        {
            return await Task.Run(() =>
            {
                byte[] postByte = Encoding.UTF8.GetBytes(postData);
                string pageSource = string.Empty;
                try
                {
                    if (OnStart != null)
                    {
                        OnStart(this, new OnStartEventArgs(uri));
                    }
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Accept = "*/*";
                    request.ServicePoint.Expect100Continue = false;
                    request.ServicePoint.UseNagleAlgorithm = false;
                    request.AllowWriteStreamBuffering = false;
                    request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.AllowAutoRedirect = false;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0: WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.10";
                    request.Timeout = 50000000;
                    request.KeepAlive = true;
                    request.Method = "POST";
                    request.ContentLength = postByte.Length;
                    if (proxy != null)
                    {
                        request.Proxy = proxy;
                    }
                    request.CookieContainer = this.CookiesContainer;
                    request.ServicePoint.ConnectionLimit = int.MaxValue;
                    if (OnSetting != null)
                    {
                        OnSetting(this, request);
                    }
                    using (Stream postDataStream = request.GetRequestStream())
                    {
                        postDataStream.Write(postByte, 0, postByte.Length);
                    }
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        foreach (Cookie cookie in response.Cookies)
                        {
                            this.CookiesContainer.Add(cookie);
                        }
                        if (response.ContentEncoding.ToLower().Contains("gzip"))
                        {
                            using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else if (response.ContentType.ToLower().Contains("deflate"))
                        {
                            using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                    watch.Stop();
                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    var milliseconds = watch.ElapsedMilliseconds;
                    request.Abort();
                    if (OnCompleted != null)
                    {
                        OnCompleted(this, new OnCompletedEventArgs(uri, threadId, milliseconds, pageSource));
                    }

                }
                catch (Exception e)
                {
                    if (OnError != null)
                    {
                        OnError(this, e);
                    }
                }
                return pageSource;
            });
        }
    }
}
