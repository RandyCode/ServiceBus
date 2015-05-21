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


        public void Connect(ChannelModeEnum mode, string address)
        {

            _process = new ClientProcess(mode, address);

        }


        public RemotingObject GetRemotingObject()
        {
            var result = _process.GetRemotingObject<RemotingObject>();
            return result;
        }

        public T GetRemotingObject<T>()
        {
            return (T)_process.GetRemotingObject<T>();
        }


        public void DisConnect()
        { }
    }
}
