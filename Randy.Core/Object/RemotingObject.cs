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
        public event MessageEventHandler ReceiveMessageHandler;

        public event MessageEventHandler SendMessageHandler;

        public event MessageEventHandler RegisterClientHandler;

        public void RegisterClient(Message message)
        {

            if (RegisterClientHandler != null)
            {
                RegisterClientHandler(message);
            }
        }

        public void SendMessage(Message message)
        {

            if (SendMessageHandler != null)
            {
                SendMessageHandler(message);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void BroadCastMessage(Message message)
        {

            if (ReceiveMessageHandler != null)
            {
                ReceiveMessageHandler(message);
            }
        }
    }

}
