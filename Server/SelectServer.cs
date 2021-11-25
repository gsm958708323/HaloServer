/* ==============================================================================
* 功能描述：PollServer  
* 创 建 者：Halo
* 创建日期：2021/11/25 21:57:31
* ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class SelectServer
    {
        Dictionary<Socket, ClientState> dictClient = new Dictionary<Socket, ClientState>();

        Socket server;

        public SelectServer(string ip, int port)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Bind
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            server.Bind(iPEndPoint);

            //Listen，参数指定队列中最多可容纳等待接受的连接数，0不限制
            server.Listen(0);

            Console.WriteLine("服务器启动成功");
        }

        List<Socket> listDel = new List<Socket>();
        public void Tick()
        {
            //Accpet
            if (server.Poll(0, SelectMode.SelectRead))
            {
                Accept();
            }

            //Receive
            listDel.Clear();
            foreach (ClientState state in dictClient.Values)
            {
                Socket client = state.socket;
                if (client.Poll(0, SelectMode.SelectRead))
                {
                    if (!Receive(state))
                    {
                        listDel.Add(client);
                    }
                }
            }
            if (listDel.Count != 0)
            {
                foreach (var client in listDel)
                {
                    dictClient.Remove(client);
                    client.Close();
                    Console.WriteLine("客户端断开连接");
                }
            }
        }

        void Check()
        {
            List<Socket> del = new List<Socket>();

            foreach (ClientState state in dictClient.Values)
            {
                Socket client = state.socket;
                byte[] readBuffer = state.buffRead;
                int count = client.Receive(readBuffer);
                //客户端关闭
                if (count == 0)
                {

                    client.Close();
                    dictClient.Remove(client);
                    Console.WriteLine("Socket Close");
                }
            }
        }

        private bool Receive(ClientState state)
        {
            byte[] readBuffer = state.buffRead;
            Socket client = state.socket;
            int count = client.Receive(readBuffer);
            if (count == 0)
            {
                return false;
            }

            string recvStr = System.Text.Encoding.Default.GetString(readBuffer, 0, count);
            Console.WriteLine("Receive " + recvStr);
            string sendStr = client.RemoteEndPoint.ToString() + ":" + recvStr;
            byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
            foreach (ClientState cs in dictClient.Values)
            {
                cs.socket.Send(sendBytes);
            }
            return true;
        }

        private void Accept()
        {
            Console.WriteLine("新的客户端连接");
            Socket client = server.Accept();
            ClientState state = new ClientState();
            state.socket = client;
            dictClient.Add(client, state);
        }
    }

}