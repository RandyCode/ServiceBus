using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class RemotingObjectWrapper : MarshalByRefObject
    {
        public event MessageEventHandler OnReceiveMessageHandler;

        public void ReceiveMessage(Message msg)
        {
            if (OnReceiveMessageHandler != null)
            {
                OnReceiveMessageHandler(msg);
            }
        }

        [OneWay]
        public void ReceiveMessageByOneWay(Message msg)
        {
            ReceiveMessage(msg);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

 

}
