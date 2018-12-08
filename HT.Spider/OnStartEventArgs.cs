using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Spider
{
    /// <summary>
    /// OnStart事件的参数
    /// </summary>
    public class OnStartEventArgs
    {
        Uri uri;
        public OnStartEventArgs(Uri uri)
        {
            this.uri = uri;
        }
    }
}
