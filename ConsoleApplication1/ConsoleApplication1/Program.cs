using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 最简单的服务器连接代码
            ////IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
            //IPAddress ip = IPAddress.Parse("172.26.1.236");
            ////IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            //TcpListener tlistener = new TcpListener(ip, 10001);
            //tlistener.Start();
            //Console.WriteLine("server监听启动......");
            //while (true)//看上去是死循环，因为堵塞方法，大部分时间都在等待
            //{
            //    TcpClient remoteClient = tlistener.AcceptTcpClient();//接收已连接的client,堵塞方法
            //    Console.WriteLine("client已连接！local:{0}<---Client:{1}", remoteClient.Client.LocalEndPoint, remoteClient.Client.RemoteEndPoint);
            //}
            #endregion
            //ip地址
            string ip = "127.0.0.1";
            //端口号
            int port = 10001;

            Server server = new Server(ip, port);
            server.StartServer();
            Console.ReadKey();
        }
    }
}
