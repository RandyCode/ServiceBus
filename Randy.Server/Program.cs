using Randy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            RemotingServer rt = new RemotingServer(ChannelModeEnum.HTTP,9090);
            //rt.RegisterServiceType(typeof(TestModel));

            rt.RegisterServiceType(typeof(TestModel), System.Runtime.Remoting.WellKnownObjectMode.SingleCall);

            Console.Read();
        }
    }
}
