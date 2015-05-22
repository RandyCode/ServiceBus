using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class ClientProcess
    {
        private string _address;

        public ClientProcess(ChannelModeEnum mode, string address)
        {
            IChannel channel = GetChannel(mode);
            ChannelServices.RegisterChannel(channel, false);

            string prefix = (mode == ChannelModeEnum.HTTP) ? "http://" : "tcp://";
            _address = prefix + address;
        }


        public void RegisterClientType(Type type)
        {
            RemotingConfiguration.RegisterActivatedClientType(type, _address);
        }


        public T GetRemotingObject<T>()
        {
            Type type = typeof(T);
            string objAddress = _address + "/" + type.Name;
            var result = (T)Activator.GetObject(type, objAddress);
            return result;
        }


        private IChannel GetChannel(ChannelModeEnum mode)
        {

            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            IDictionary props = new Hashtable();

            props["port"] = GetLocalPort();
            IChannel channel = null;

            switch (mode)
            {
                case ChannelModeEnum.HTTP:
                    channel = new HttpClientChannel();
                    break;

                case ChannelModeEnum.TCP:
                    channel = new TcpChannel(props, clientProvider, serverProvider);
                    break;

                default:
                    return null;
            }

            return channel;
        }

        private int GetLocalPort()
        {
            Random r = new Random();
            int port = 0;
            bool isUsed = true;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            while (isUsed)
            {
                port = r.Next(2000, 20000);
                foreach (var item in ipEndPoints)
                {
                    if (port == item.Port)
                        isUsed = false;
                }

            }
            return port;
        }




    }
}
