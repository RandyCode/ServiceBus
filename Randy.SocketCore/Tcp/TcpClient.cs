using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Randy.SocketCore.Tcp
{
    public class TcpClient
    {
        public void Connect(string ip,int port)
        {
            IPAddress ServerIp = ip == null ? IPAddress.Any : IPAddress.Parse(ip);   
            byte[] data = new byte[1024];
            Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint IP = new IPEndPoint(ServerIp, port);
            try
            {
                Client.Connect(IP);
            }
            catch (Exception e)
            {
                Console.WriteLine("unable to connect to server");
                Console.WriteLine(e.ToString());
                Console.ReadLine();
                return;
            }

            int recv = Client.Receive(data);
            string strdata = Encoding.UTF8.GetString(data, 0, recv);
            Console.WriteLine(strdata);
            bool hasEx = false;
            while (!hasEx)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                    break;
                try
                { 
                    Client.Send(Encoding.UTF8.GetBytes(input));
                }
                catch
                {
                    Console.WriteLine("disconnect from sercer");
                    hasEx = true;
                }
                //data = new byte[1024];
                //recv = Client.Receive(data);
                //strdata = Encoding.UTF8.GetString(data, 0, recv);
                //Console.WriteLine(strdata);
            }
            Client.Shutdown(SocketShutdown.Both);
            Client.Close();
            Console.ReadLine();
        }
    }
}
