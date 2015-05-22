using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class RemotingClient
    {

        public ClientProcess _process;
        private bool isConnected = false;

        public void Connect(ChannelModeEnum mode, string address)
        {
            _process = new ClientProcess(mode, address);
            _process.RegisterClientType(typeof(RemotingObject));
            isConnected = true;
        }



        public RemotingObject GetRemotingObject()
        {
            if (isConnected == false)
                throw new Exception("DisConnect server");

            var result = _process.GetRemotingObject<RemotingObject>();
            return result;
        }

        public T GetRemotingObject<T>()
        {
            if (isConnected == false)
                throw new Exception("DisConnect server");

            return (T)_process.GetRemotingObject<T>();
        }


        public void DisConnect()
        {
            isConnected = false;
        }
    }
}
