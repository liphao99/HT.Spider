using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Spider
{
    /// <summary>
    /// OnCompleted事件的参数
    /// </summary>
    public class OnCompletedEventArgs
    {
        public Uri Uri { get; set; }
        public int ThreadId { get; private set; }
        public string PageSource { get; private set; }
        public long Milliseconds { get; private set; }
        public OnCompletedEventArgs(Uri uri, int threadId, long milliseconds, string pageSource)
        {
            Uri = uri;
            ThreadId = threadId;
            PageSource = pageSource;
            Milliseconds = milliseconds;
        }
    }
}
