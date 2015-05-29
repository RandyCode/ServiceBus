using Randy.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
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
            rt.ReceiveMessageHandler += ro_MessageHandler;
            rt.Connect(ChannelModeEnum.TCP, ip);


            var cc = ChannelServices.RegisteredChannels;

            int count = 1;
            while (true)
            {
                Console.ReadKey();
                rt.SendMessage(new Message { AppId = rt.ClientId, Signal = SignalTypeEnum.JOB, Content = "client send msg ," + count++ });

            }

            //rt.DisConnect();
            //Console.Read();

        }


        static void ro_MessageHandler(Message message)
        {

            Console.WriteLine("server msg : " + message.Content);
        }

    }
}
