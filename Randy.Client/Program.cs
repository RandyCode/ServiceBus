using Randy.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Client
{

    class Program
    {
        static void Main(string[] args)
        {



            string ip = ConfigurationManager.AppSettings["RemotingServer"];

            RemotingClient rt = new RemotingClient();
            rt.Connect(ChannelModeEnum.TCP, ip);

            var obj = rt.GetRemotingObject();
            var wapper = new MessageEventHandlerWapper();

            wapper.MessageHandler += ro_MessageHandler;
            obj.MessageHandler += wapper.Push;



            Console.Read();

        }



        static void ro_MessageHandler(string message)
        {

            Console.WriteLine("ro_MessageHandler : " + message);
        }
    }
}
