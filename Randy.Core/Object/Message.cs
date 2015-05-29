using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{

    [Serializable]
    public sealed class Message
    {
        public string Id { get; set; }
        public string AppId { get; set; }

        public SignalTypeEnum? Signal { get; set; }
    
        public object Content { get; set; }

        public MessageTargetEnum? Target { get; set; }

    }

}
