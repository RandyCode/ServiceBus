using Randy.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Server
{
    class Program
    {
        static void Main(string[] args)
        {

            RemotingServer rt = new RemotingServer();
            rt.Start();

     
            Console.Read();


        }
    }
}
