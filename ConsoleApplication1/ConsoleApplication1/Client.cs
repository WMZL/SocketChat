using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Client
    {
        private Socket m_ClientSocket;
        private Server m_Server;
        /// <summary>
        /// 处理消息
        /// </summary>
        private Message m_Msg = new Message();

        public Client()
        {

        }

        public Client(Socket clientSocket, Server server)
        {
            this.m_ClientSocket = clientSocket;
            this.m_Server = server;
        }

        /// <summary>
        /// 启动客户端
        /// </summary>
        public void StartClient()
        {
            //判断客户端是否被关闭TODO
            if (m_ClientSocket.Connected == false || m_ClientSocket.Poll(10, SelectMode.SelectRead))
            {
                m_ClientSocket.Close();
                return;
            }

            //等待从客户端的消息的接收
            m_ClientSocket.BeginReceive(m_Msg.data, 0, m_Msg.data.Length, SocketFlags.None, RecvCallBack, null);
        }

        /// <summary>
        /// 接收回调,这个参数什么什么意思TODO
        /// </summary>
        private void RecvCallBack(IAsyncResult result)
        {
            try
            {
                if (m_ClientSocket.Connected == false || m_ClientSocket.Poll(10, SelectMode.SelectRead))
                {
                    m_ClientSocket.Close();
                    return;
                }

                int length = m_ClientSocket.EndReceive(result);
                if (length == 0)
                {
                    m_ClientSocket.Close();
                    return;
                }

                m_Msg.Read(length, OnBoardMsg);
                //继续等待接收消息
                m_ClientSocket.BeginReceive(m_Msg.data, 0, m_Msg.data.Length, SocketFlags.None, RecvCallBack, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("发生错误：" + e);
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        private void OnBoardMsg(string msg)
        {
            m_Server.BroadCast(msg);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMessage(string msg)
        {
            if (m_ClientSocket.Connected == false || m_ClientSocket.Poll(10, SelectMode.SelectRead))
            {
                m_ClientSocket.Close();
                return;
            }
            ///发送消息给客户端
            m_ClientSocket.Send(m_Msg.Pack(msg));
        }
    }
}
