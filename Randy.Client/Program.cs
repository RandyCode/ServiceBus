using Randy.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Client
{

    class Program
    {
        static void Main(string[] args)
        {



            string ip = ConfigurationManager.AppSettings["RemotingServer"];

            //RemotingClient rt = new RemotingClient();
            //rt.Connect(ChannelModeEnum.TCP, ip);



            //RemotingObject ro = rt.GetRemotingObject();
            //var result = rt.GetRemotingObject<Person>();
            //result.beginEvent += result_beginEvent;


            Console.Read();

        }

        static void result_beginEvent(string n)
        {
            Console.WriteLine("客户端执行时间：" + n);
        }


        static void ro_MessageHandler(string message)
        {

            Console.WriteLine("ro_MessageHandler : " +message);
        }
    }
}
