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

namespace Randy.Core
{
    public class RemotingServer
    {

        private BlockingCollection<Message> _blockingCollection;
        private Dictionary<string, string> _clientList;


        public RemotingServer()
        {
            _blockingCollection = new BlockingCollection<Message>();
            _clientList = new Dictionary<string, string>();
        }

        public void Start()
        {

            string fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            RemotingConfiguration.Configure(fileName, false);
            RegisterServiceType(typeof(RemotingObject),WellKnownObjectMode.Singleton);
      
            Console.WriteLine("Server Bus Start.");
            RemotingObject ro = new RemotingObject();
            var obj = RemotingServices.Marshal(ro, "RemotingObject");

            while (true)
            {
                Console.ReadKey();
                ro.BroadCastMessage("server broadcast");
            }
            
        }

        public void Stop()
        {
 
        }


        #region Private mehtod

        /// <summary>
        /// 客户端激活 对象由自己管理，可调用自定义构造
        /// </summary>
        /// <param name="type"></param>
        private void RegisterServiceType(Type type)
        {
            RemotingConfiguration.RegisterActivatedServiceType(type);
        }

        /// <summary>
        /// 服务端激活 对象由GC管理 ，调用默认构造
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        private void RegisterServiceType(Type type, WellKnownObjectMode wellKnownMode)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(type, type.Name, wellKnownMode);
        } 
        #endregion

    }
}
