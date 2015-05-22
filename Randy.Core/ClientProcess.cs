using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Random r = new Random();
              //r.Next(2000, 20000);
            props["port"] = 8084;
     
            switch (mode)
            {
                case ChannelModeEnum.HTTP:
                    return new HttpClientChannel();

                case ChannelModeEnum.TCP:
                    return new TcpChannel(props, clientProvider, serverProvider);

                default:
                    return null;
            }
        }


    }
}
