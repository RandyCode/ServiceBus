using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class RemotingClient
    {

        private ClientProcess _process;
        private RemotingObjectWrapper _messageWrapper;
        private bool isConnected = false;
        private readonly string _clientId = Guid.NewGuid().ToString();

        public event MessageEventHandler ReceiveMessageHandler;

        public string ClientId { get { return _clientId; } }


        public void Connect(ChannelModeEnum mode, string address)
        {
            isConnected = true;
            _process = new ClientProcess(mode, address);
            _messageWrapper = new RemotingObjectWrapper();
            BindReceiveEvent();

        }

     


        public void Receive_MessageHandler(Message message)
        {
            //message from server
            if (ReceiveMessageHandler != null)
                ReceiveMessageHandler(message);
        }

        public void Send(Message message)
        {
            
        }


        public RemotingObject GetRemotingObject()
        {
            if (isConnected == false)
                throw new Exception("DisConnect server");

            var result = _process.GetRemotingObject<RemotingObject>();
            return result;
        }

        public T GetRemotingObject<T>()
        {
            if (isConnected == false)
                throw new Exception("DisConnect server");

            return (T)_process.GetRemotingObject<T>();
        }


        public void DisConnect()
        {
            isConnected = false;

        }


        private void BindReceiveEvent()
        {
            //remoting Client & Server wellknow
            var obj = GetRemotingObject();
            _messageWrapper.OnReceiveMessageHandler += Receive_MessageHandler;
            obj.MessageHandler += _messageWrapper.ReceiveMessage;
        }


   
    }
}
