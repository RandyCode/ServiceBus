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


///Randy.Core just for remoting test
namespace Randy.Core
{
    /// <summary>
    /// remoting server 
    /// </summary>
    public class RemotingServer
    {
        //待处理队列
        private BlockingCollection<Message> _workCollection;
        private ManualResetEvent _manualReset = null;
        private string _serverId = Guid.NewGuid().ToString();
        private JobProcess _jobProcess;

        public RemotingServer()
        {
            _workCollection = new BlockingCollection<Message>();
            _manualReset = new ManualResetEvent(false);
            _jobProcess = new JobProcess();
        }


        public void AddItem(Message msg)
        {
            //lock
            if (msg != null && _workCollection.Any(o => o.Id == msg.Id) == false)
                _workCollection.Add(msg);
        }


        public void Start()
        {

            string fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            RemotingConfiguration.Configure(fileName, false);
            Console.WriteLine("Server Bus Start.");
            RemotingObject ro = new RemotingObject();
            ro.SendMessageHandler += SendMessageHandler;
            var obj = RemotingServices.Marshal(ro, "RemotingObject");

            //接受client 结果
            Thread thread = new Thread(ToDoJobs);
            thread.IsBackground = true;
            thread.Start();

            //生产，分配任务  。1,client 提出 计算请求， 2，服务器收到(生产消费)分发给其他client处理， 返回结构，再返回给目标客户端
            //while (true)
            //{
            //    Console.ReadKey();
            //    ro.BroadCastMessage(new Message
            //    {
            //        Target = MessageTargetEnum.CLIENT,
            //        AppId = _serverId,
            //        Content = " Broad cast message"
            //    });
            //}

        }


        public void Stop()
        { 
            _workCollection.Dispose();
        }

        public void ToDoJobs()
        {
            while (true)
            {

                if (_workCollection.Count <= 0)
                {
                    _manualReset.Reset();
                    _manualReset.WaitOne();
                }

                var item = _workCollection.Take();
                _jobProcess.Process(item);  //分发器

            }
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
            _manualReset.Set();
            Console.WriteLine(DateTime.Now + "> receive signal : " + msg.Signal.ToString() + " from app : " + msg.AppId);
        }


        #endregion

    }
}
