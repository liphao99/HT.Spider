using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Spider
{
    /// <summary>
    /// OnError事件的参数
    /// </summary>
    public class OnErrorEventArgs:Exception
    {
        public Exception e;
        public OnErrorEventArgs(Exception e)
        {
            this.e = e;
        }
    }
}
