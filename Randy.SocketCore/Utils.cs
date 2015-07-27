using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Randy.SocketCore
{
    public class Utils
    {
        public static IPAddress GetLocalmachineIPAddress()
        {
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

            foreach (IPAddress ip in ipEntry.AddressList)
            {
                //IPV4
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }

            return ipEntry.AddressList[0];
        }

        public static int GetRandomLocalPort()
        {
            Random r = new Random();
            int port = 0;
            bool isUsed = true;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            while (isUsed)
            {
                port = r.Next(2000, 20000);
                bool foundKey = false;
                foreach (var item in ipEndPoints)
                {
                    if (port == item.Port)
                        foundKey = true;
                }
                if (!foundKey)
                    isUsed = false;
            }
            return port;
        }
    }
}
