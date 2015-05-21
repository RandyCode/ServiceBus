using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    [Serializable]
    public class MessageEventArgs : EventArgs
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

        public MessageEventArgs(MessageEventInfo info)
        {
            AppId = info.AppId;
            Arguments = info.Arguments;
            Channel = info.Channel;
            Signal = info.Signal;
            MessageObject = info.MessageObject;
            MessageType = info.MessageType;
        }

    }


  


}
