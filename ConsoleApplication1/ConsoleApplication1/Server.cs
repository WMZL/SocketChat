using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    public class Server
    {
        /// <summary>
        /// 客户端Socket
        /// </summary>
        private Socket m_ClientSocket = null;
        /// <summary>
        /// 多个客户端用list存储，方便转发
        /// </summary>
        private List<Client> m_ClientList = new List<Client>();
        /// <summary>
        /// 服务器Socket
        /// </summary>
        private Socket m_ServerSocket = null;

        public Server()
        {

        }

        public Server(string ip, int port)
        {
            m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            //将Socket绑定到对应的ip地址和端口
            m_ServerSocket.Bind(point);
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void StartServer()
        {
            //开始监听，0表示等待连接队列的最大数 TODO这里有疑问
            m_ServerSocket.Listen(0);

            Console.WriteLine("服务器启动成功，开始监听...");
            Console.WriteLine("等待客户端连接");
            m_ServerSocket.BeginAccept(ConnectSucCallBack, null);
        }

        /// <summary>
        /// 连接成功的回调方法
        /// 此方法必须带IAsyncResult类型参数
        /// </summary>
        private void ConnectSucCallBack(IAsyncResult result)
        {
            m_ClientSocket = m_ServerSocket.EndAccept(result);
            Console.WriteLine("一个客户端连接成功...");
            Client client = new Client(m_ClientSocket, this);
            ///将连接的客户端加入到列表中
            m_ClientList.Add(client);

            client.StartClient();

            m_ServerSocket.BeginAccept(ConnectSucCallBack, null);
        }

        /// <summary>
        /// 服务器广播消息
        /// </summary>
        public void BroadCast(string msg)
        {
            foreach (Client item in m_ClientList)
            {
                item.SendMessage(msg);
            }
        }
    }
}
