/* ==============================================================================
* 功能描述：SyncServer  
* 创 建 者：Halo
* 创建日期：2021/11/21 17:34:00
* ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Server
{
    /// <summary>
    /// 同步服务器
    /// </summary>
    public class SyncServer
    {
        public SyncServer()
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Bind
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 8888);
            server.Bind(iPEndPoint);

            //Listen，参数指定队列中最多可容纳等待接受的连接数，0不限制
            server.Listen(0);

            Console.WriteLine("服务器启动成功");

            while (true)
            {
                //Accept
                Socket client = server.Accept();
                Console.WriteLine($"接受客户端连接：{client}");

                //Receive
                byte[] arrRead = new byte[1024];
                int nCount = client.Receive(arrRead);
                string strRead = Encoding.Default.GetString(arrRead, 0, nCount);
                Console.WriteLine($"接收数据: {strRead}");

                //Send
                byte[] arrSend = Encoding.Default.GetBytes($"客户端你好，我是服务器：{strRead}");
                client.Send(arrSend);
            }
        }
    }
}