using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class RemotingClient
    {
        private string _address;

        public RemotingClient(ChannelModeEnum mode, string address)
        {
            IChannel channel = GetChannel(mode);
            ChannelServices.RegisterChannel(channel, true);

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
            switch (mode)
            {
                case ChannelModeEnum.HTTP:
                    return new HttpClientChannel();

                case ChannelModeEnum.TCP:
                    return new TcpClientChannel();

                default:
                    return null;
            }
        }


    }
}
