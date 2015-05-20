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
    public class RemotingServer
    {
        public RemotingServer(ChannelModeEnum mode, int port)
        {
            IChannel channel = GetChannel(mode,port);
            ChannelServices.RegisterChannel(channel, true);
             
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


        private IChannel GetChannel(ChannelModeEnum mode, int port)
        {
            switch (mode)
            {
                case ChannelModeEnum.HTTP:
                    return new HttpChannel(port);

                case ChannelModeEnum.TCP:
                    return new TcpChannel(port);

                default:
                    return null;
            }
        }

    }
}
