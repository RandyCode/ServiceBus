using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    [Serializable]
    public class MessageObject
    {
        public string AppId { get; set; }

        public string Signal { get; set; }

        public object Message { get; set; }
    }

}
