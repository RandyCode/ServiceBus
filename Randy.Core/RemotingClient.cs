﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

///Randy.Core just for remoting test
namespace Randy.Core
{
    /// <summary>
    /// remoting server 
    /// </summary>
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
            RegisterClient();
        }


        public void SendMessage(Message message)
        {
            var obj = GetRemotingObject();
            obj.SendMessage(message);
            Console.WriteLine(DateTime.Now + "> send signal : " + message.Signal.ToString());
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
            _process.Unregister();
            UnRegisterClient();
        }


        private void RegisterClient()
        {
            var msg = new Message()
            {
                AppId = ClientId,
                Content = DateTime.Now,
                Signal = SignalTypeEnum.REGISTER,
                Target = MessageTargetEnum.SERVER
            };
            SendMessage(msg);
        }

        private void UnRegisterClient()
        {
            var msg = new Message()
            {
                AppId = ClientId,
                Content = DateTime.Now,
                Signal = SignalTypeEnum.UNREGISTER,
                Target = MessageTargetEnum.SERVER
            };
            SendMessage(msg);
        }


        private void BindReceiveEvent()
        {
            //remoting Client & Server wellknow
            var obj = GetRemotingObject();
            _messageWrapper.OnReceiveMessageHandler += Receive_MessageHandler;
            obj.ReceiveMessageHandler += _messageWrapper.ReceiveMessage;
        }

        private void Receive_MessageHandler(Message message)
        {
            //message from server
            if (ReceiveMessageHandler != null)
                ReceiveMessageHandler(message);
        }


    }
}
