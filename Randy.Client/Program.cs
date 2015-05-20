using Randy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            RemotingClient rt = new RemotingClient(ChannelModeEnum.HTTP, "localhost:9090");

            //rt.RegisterClientType(typeof(TestModel));


            TestModel te = rt.GetRemotingObject<TestModel>();
            var aa= te.Hello();



            Console.Read();

        }
    }
}
