using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randy.Core
{
    public class JobProcess
    {

        public JobProcess()
        {
            _clientList = new Dictionary<string, DateTime>();
        }

        private Dictionary<string, DateTime> _clientList;

        public Dictionary<string, DateTime> ClientList
        {
            get { return _clientList; }
            set { _clientList = value; }
        }


        public void Process(Message msg)
        {
            //仅作转发
            if (msg != null && msg.Signal != null)
            {
                switch (msg.Signal)
                {
                    case SignalTypeEnum.JOB:
                        //AddItem(msg); 云计算
                        break;
                    case SignalTypeEnum.REGISTER:
                        RegisterClient(msg.AppId);
                        break;
                    case SignalTypeEnum.UNREGISTER:
                        UnRegisterClient(msg.AppId);
                        break;
                    case SignalTypeEnum.LOG:
                        //AddItem(msg);
                        break;
                    case SignalTypeEnum.KEEPALIVE:
                        KeepClientAlive(msg.AppId);   //timer run  unregister
                        break;

                    default: break;
                }

            }
        }



        private void KeepClientAlive(string appId)
        {
            if (_clientList != null && _clientList.ContainsKey(appId) == true)
            {
                lock (_clientList)
                {
                    _clientList[appId] = DateTime.Now;
                }
            }
            //UnRegisterClient(msg.AppId);
        }

        private void RegisterClient(string appId)
        {
            if (_clientList != null && _clientList.ContainsKey(appId) == false)
            {
                lock (_clientList)
                {
                    _clientList.Add(appId, DateTime.Now);
                }
            }
        }

        private void UnRegisterClient(string appId)
        {
            if (_clientList != null && _clientList.ContainsKey(appId) == true)
            {
                lock (_clientList)
                {
                    _clientList.Remove(appId);
                }
            }
        }
    }
}
