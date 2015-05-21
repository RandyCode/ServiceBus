using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{

    public class Person : MarshalByRefObject
    {
        public delegate void CHandle(string n);
        public event CHandle beginEvent;

        private int counter = 0;

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="info"></param>
        public void BroadCastInfo(string info)
        {
            if (beginEvent != null)
            {
                beginEvent(info);
            }
        }



    }


    public class RemotingObject : MarshalByRefObject
    {
        public delegate void MessageEventHandler(string msg);  //Message message
        public event MessageEventHandler MessageHandler;

        public object Request { get; set; }

        public object Response { get; set; }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void BroadCastMessage(string message)
        {
            Console.WriteLine("BroadCastMessage();");
            if (MessageHandler != null)
            {
                //try
                //{
                MessageHandler(message);
                //}
                //catch
                //{

                //}

            }
        }



    }

}
