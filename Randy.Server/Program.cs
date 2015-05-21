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

            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, true);

            var person = new Person();
            var obj = RemotingServices.Marshal(person, "SayHello");

            System.Console.WriteLine("服务器正在等待信息,按任意键退出！");

            while (true)
            {
                string yes = System.Console.ReadLine();
                if (yes == "yes")
                {
                    //执行服务端事件
                    person.BroadCastInfo("hello");
                }
            }


            //RemotingServer rt = new RemotingServer();
            //rt.Start();
           

            Console.Read();


        }
    }
}
