using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Randy.SocketCore
{

    [Serializable]
    public sealed class MessageInfo
    {
        public string Id { get; set; }
        public string AppId { get; set; }

        public string Signal { get; set; }

        public object Content { get; set; }

        public string Target { get; set; }

    }
}
