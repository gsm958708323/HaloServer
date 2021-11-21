/* ==============================================================================
* 功能描述：AsyncServer  
* 创 建 者：Halo
* 创建日期：2021/11/21 17:33:54
* ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Server
{
    /// <summary>
    /// 异步服务器
    /// </summary>
    public class AsyncServer
    {
        Dictionary<Socket, ClientInfo> dictClient = new Dictionary<Socket, ClientInfo>();

        public AsyncServer(string ip, int port)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Bind
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            server.Bind(iPEndPoint);

            //Listen，参数指定队列中最多可容纳等待接受的连接数，0不限制
            server.Listen(0);

            Console.WriteLine("服务器启动成功");

            //Accept
            server.BeginAccept(OnAccept, server);
        }

        private void OnAccept(IAsyncResult ar)
        {
            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);
            Console.WriteLine("新的客户端连接");

            //Recive
            ClientInfo info = new ClientInfo();
            info.socket = client;
            dictClient.Add(client, info);

            client.BeginReceive(info.buffRead, 0, 1024, 0, OnRecive, info);

            //Accept
            server.BeginAccept(OnAccept, server);
        }

        private void OnRecive(IAsyncResult ar)
        {
            ClientInfo clientInfo = (ClientInfo)ar.AsyncState;
            Socket client = clientInfo.socket;
            int nCount = client.EndReceive(ar);
            if (nCount == 0)
            {
                client.Close();
                dictClient.Remove(client);
                Console.WriteLine("连接断开");
                return;
            }

            string szRecv = Encoding.Default.GetString(clientInfo.buffRead, 0, nCount);
            byte[] buffSend = Encoding.Default.GetBytes($"OnServer：{szRecv}");

            client.Send(buffSend);
            client.BeginReceive(clientInfo.buffRead, 0, 1024, 0, OnRecive, clientInfo);
        }
    }

    internal class ClientInfo
    {
        public Socket socket;
        public byte[] buffRead = new byte[1024];
    }
}