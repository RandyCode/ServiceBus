using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.Concurrent;
using System.Threading;

namespace Randy.Core
{
    public class RemotingServer
    {
        //待处理队列
        private BlockingCollection<Message> _workCollection;
        private Dictionary<string, DateTime> _clientList;
        private ManualResetEvent _manualReset = null;
        private string _serverId = Guid.NewGuid().ToString();

        public RemotingServer()
        {
            _workCollection = new BlockingCollection<Message>();
            _clientList = new Dictionary<string, DateTime>();
            _manualReset = new ManualResetEvent(false);
        }


        public void AddItem(Message msg)
        {
            //lock
            _workCollection.Add(msg);
            Console.WriteLine("add item : id = " + msg.AppId);
        }


        public void Start()
        {

            string fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            RemotingConfiguration.Configure(fileName, false);
            Console.WriteLine("Server Bus Start.");
            RemotingObject ro = new RemotingObject();
            ro.SendMessageHandler += SendMessageHandler;
            //ro.RegisterClientHandler += RegisterClientHandler;
            var obj = RemotingServices.Marshal(ro, "RemotingObject");

            //信号触发
            while (true)
            {
                Console.ReadKey();
                ro.BroadCastMessage(new Message
                {
                    Target = MessageTargetEnum.CLIENT,
                    AppId = _serverId,
                    Content = "SERVER : Broad cast message"
                });
            }

        }


        public void Stop()
        {
            _workCollection.Dispose();
        }

        public void WakeUp()
        {
            _manualReset.Set();
        }

        public void ToDoJobs()
        {

            if (_workCollection.Count <= 0)
            {
                _manualReset.Reset();
                _manualReset.WaitOne();
            }

            var item = _workCollection.Take();
        }



        #region Private mehtod


        private void RegisterServiceType(Type type)
        {
            RemotingConfiguration.RegisterActivatedServiceType(type);
        }

        private void RegisterServiceType(Type type, WellKnownObjectMode wellKnownMode)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(type, type.Name, wellKnownMode);
        }

        private void SendMessageHandler(Message msg)
        {
            if (msg != null && msg.Signal != null)
            {
                switch (msg.Signal)
                {
                    case SignalTypeEnum.JOBITEM:
                        AddItem(msg);
                        break;
                    case SignalTypeEnum.REGISTER:
                        RegisterClient(msg.AppId);
                        break;
                    case SignalTypeEnum.UNREGISTER:
                        UnRegisterClient(msg.AppId);
                        break;
                    case SignalTypeEnum.LOG:
                        //AddItem(msg);
                        break;
                    case SignalTypeEnum.KEEPALIVE:
                        KeepClientAlive(msg.AppId);   //timer run  unregister
                        break;

                    default: break;
                }

            }

            Console.WriteLine(DateTime.Now + "> receive signal : " + msg.Signal.ToString() + " from app : " + msg.AppId);


        }

        private void KeepClientAlive(string appId)
        {
            if (_clientList != null && _clientList.ContainsKey(appId) == true)
            {
                lock (_clientList)
                {
                    _clientList[appId] = DateTime.Now;
                }
            }
            //UnRegisterClient(msg.AppId);
        }

        private void RegisterClient(string appId)
        {
            if (_clientList != null && _clientList.ContainsKey(appId) == false)
            {
                lock (_clientList)
                {
                    _clientList.Add(appId, DateTime.Now);
                }
            }
        }

        private void UnRegisterClient(string appId)
        {
            if (_clientList != null && _clientList.ContainsKey(appId) == true)
            {
                lock (_clientList)
                {
                    _clientList.Remove(appId);
                }
            }
        }
        #endregion

    }
}
