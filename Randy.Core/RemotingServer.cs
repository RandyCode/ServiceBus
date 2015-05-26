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
        private Dictionary<string, string> _clientList;
        private ManualResetEvent _manualReset = null;


        public RemotingServer()
        {
            _workCollection = new BlockingCollection<Message>();
            _clientList = new Dictionary<string, string>();
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
            ro.RegisterClientHandler += RegisterClientHandler;
            var obj = RemotingServices.Marshal(ro, "RemotingObject");

            //信号触发
            while (true)
            {
                Console.ReadKey();
                ro.BroadCastMessage(new Message { Content = "Broad cast message" });
            }

        }

        private void RegisterClientHandler(Message msg)
        {
            if (_clientList != null && _clientList.ContainsKey(msg.AppId) == false)
            {
                _clientList.Add(msg.AppId, msg.Content.ToString());
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
            AddItem(msg);
        }
        #endregion

    }
}
