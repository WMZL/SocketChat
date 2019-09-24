using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Message
    {
        public byte[] data = new byte[1024];


        /// <summary>
        /// 解析，将byte数组转成string
        /// </summary>
        /// <param name="length"></param>
        /// <param name="anysicsMsg"></param>
        public void Read(int length, Action<string> anysicsMsg)
        {
            string message = Encoding.UTF8.GetString(data, 0, length);
            anysicsMsg(message);
            Array.Clear(data, 0, length);
        }

        /// <summary>
        /// 发送，将string转成byte[]
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public byte[] Pack(string msg)
        {
            byte[] databyte = Encoding.UTF8.GetBytes(msg);
            return databyte;
        }
    }
}
