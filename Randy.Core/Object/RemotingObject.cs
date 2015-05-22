using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class MessageEventHandlerWapper : MarshalByRefObject
    {
        public event MessageEventHandler MessageHandler;
        public void Push(string msg)
        {
            if (MessageHandler != null)
            {
                MessageHandler(msg);
            }
        }
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    [Serializable]
    public delegate void MessageEventHandler(string msg);  //Message message

    public class RemotingObject : MarshalByRefObject
    {
        public event MessageEventHandler MessageHandler;

        public object Request { get; set; }

        public object Response { get; set; }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void BroadCastMessage(string message)
        {

            if (MessageHandler != null)
            {
                MessageHandler(message);

            }
        }
    }

}
