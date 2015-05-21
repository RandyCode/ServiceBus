using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class MessageEventInfo 
    {
        public string AppId
        {
            get;
            private set;
        }

        public IDictionary<string, string> Arguments
        {
            get;
            private set;
        }

        public string Channel
        {
            get;
            private set;
        }

        public string Signal
        {
            get;
            private set;
        }

        public object MessageObject
        {
            get;
            private set;
        }

        public Type MessageType
        {
            get;
            private set;
        }


    }


}
