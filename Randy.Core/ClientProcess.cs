﻿using System;
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
        private IChannel _currentChannel;

        public ClientProcess(ChannelModeEnum mode, string address)
        {
            _currentChannel = GetChannel(mode);
            ChannelServices.RegisterChannel(_currentChannel, false);

            string prefix = (mode == ChannelModeEnum.HTTP) ? "http://" : "tcp://";
            _address = prefix + address;
        }


        public void Unregister()
        {
            if (_currentChannel != null)
                ChannelServices.UnregisterChannel(_currentChannel);
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
                    channel = new HttpChannel();
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
