using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
  

    [Serializable]
    public delegate void MessageEventHandler(Message msg);

    public class RemotingObject : MarshalByRefObject
    {
        public event MessageEventHandler MessageHandler;

        //public object Request { get; set; }

        //public object Response { get; set; }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void BroadCastMessage(Message message)
        {

            if (MessageHandler != null)
            {
                MessageHandler(message);
            }
        }
    }

}
