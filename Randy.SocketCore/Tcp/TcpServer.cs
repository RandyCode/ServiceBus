using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Randy.SocketCore.Tcp
{
    public class TcpServer
    {
        private int _port;
        private Socket _serverSocket = null;

        private Socket _receiveSocket = null;

        public TcpServer(int port=9010)
        {
            _port = port;
        }

        public void Start()
        {
            var address= Utils.GetLocalmachineIPAddress();
            IPEndPoint ipPoint = new IPEndPoint(address, _port);
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _serverSocket.Bind(ipPoint);
            _serverSocket.Listen(1000);

            Thread thrWatch = new Thread(WatchConnecting);
            thrWatch.IsBackground = true;
            thrWatch.Start();

            Console.WriteLine("Server starting....");

        }

        private void WatchConnecting()
        {
            //保持监听 
            while (true)
            {
                _receiveSocket = _serverSocket.Accept();
                IPEndPoint clientIp = _receiveSocket.RemoteEndPoint as IPEndPoint;
                Console.WriteLine(clientIp.ToString());
                Console.WriteLine("connect with client:" + clientIp.Address + " at port:" + clientIp.Port);
                var data = Encoding.UTF8.GetBytes("Server Connected!");
                _receiveSocket.Send(data, data.Length, SocketFlags.None);

                Thread thrMsg = new Thread(RecieveMsg);
                thrMsg.IsBackground = true;
                thrMsg.Start();
            }
        }

        private void RecieveMsg()
        {
            //循环接受消息
            bool hasEx = false;
            while (!hasEx)
            {
                try
                {
                    byte[] arrRequest = new byte[1024 * 1024 * 1];
                    //接收浏览器发来的请求报文，并获取真实 报文长度
                    int realLength = _receiveSocket.Receive(arrRequest);
                    //将数组 转成 请求报文字符串
                    string msg = System.Text.Encoding.UTF8.GetString(arrRequest, 0, realLength);
                    //显示到 窗体
                    IPEndPoint clientIp = _receiveSocket.RemoteEndPoint as IPEndPoint;
                    Console.WriteLine(string.Format("Client[{0}:{1}] say : {2}", clientIp.Address, clientIp.Port, msg));
                    //_receiveSocket.Send(Encoding.UTF8.GetBytes("Server: say again."));
                }
                catch
                {
                    Console.WriteLine("Connecting Closed.");
                    hasEx = true;
                }
            }
        }

    }
}
