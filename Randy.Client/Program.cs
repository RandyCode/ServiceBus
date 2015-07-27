using Randy.Core;
using Randy.SocketCore;
using Randy.SocketCore.Tcp;
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

            //RemotingClient rt = new RemotingClient();
            //rt.ReceiveMessageHandler += ro_MessageHandler;
            //rt.Connect(ChannelModeEnum.TCP, ip);

            for (int i = 0; i < 10000; i++)
            {
                TcpClient client = new TcpClient();
                var address = Utils.GetLocalmachineIPAddress();
                client.Connect(address.ToString(), 9010);

            }
            //int count = 1;
            //while (true)
            //{
            //    Console.ReadKey();
            //    rt.SendMessage(new Message { AppId = rt.ClientId, Signal = SignalTypeEnum.JOB, Content = "client send msg ," + count++ });

            //}

            //rt.DisConnect();
            Console.Read();

        }


        static void ro_MessageHandler(Message message)
        {

            Console.WriteLine("server msg : " + message.Content);
        }

    }
}
