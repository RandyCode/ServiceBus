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

namespace Randy.Core
{
    public class RemotingServer
    {


        public void Start()
        {
            string fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            RemotingConfiguration.Configure(fileName, true);
            RegisterServiceType(typeof(RemotingObject),WellKnownObjectMode.Singleton);
            RegisterServiceType(typeof(Person), WellKnownObjectMode.Singleton);
            Console.WriteLine("Server Bus Start.");

            RemotingObject ro = new RemotingObject();
            var person = new Person();
            var obj = RemotingServices.Marshal(person, "SayHello");
            while (true)
            {
                string yes = System.Console.ReadLine();
                if (yes == "yes")
                {
                    //执行服务端事件
                    person.BroadCastInfo("hello");
                }
            }

        }

        /// <summary>
        /// 客户端激活 对象由自己管理，可调用自定义构造
        /// </summary>
        /// <param name="type"></param>
        public void RegisterServiceType(Type type)
        {
            RemotingConfiguration.RegisterActivatedServiceType(type);
        }

        /// <summary>
        /// 服务端激活 对象由GC管理 ，调用默认构造
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        public void RegisterServiceType(Type type, WellKnownObjectMode wellKnownMode)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(type, type.Name, wellKnownMode);
        }


    }
}
