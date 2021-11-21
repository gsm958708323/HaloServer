/* ==============================================================================
* 功能描述：SyncClient  
* 创 建 者：Halo
* 创建日期：2021/11/21 16:53:33
* ==============================================================================*/
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    /// <summary>
    /// 异步客户端
    /// </summary>
    public class AsyncClient
    {
        Socket socket;

        byte[] buffRead = new byte[1024];

        public AsyncClient(string ip, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(ip, port, OnConnenct, socket);
        }

        public void Close()
        {
            socket.Close();
            Console.WriteLine("断开连接");
        }

        private void OnConnenct(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Console.WriteLine($"建立连接");

            socket.BeginReceive(buffRead, 0, 1024, 0, OnReceive, socket);
        }

        private void OnReceive(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            int nCount = socket.EndReceive(ar);
            string strRecv = Encoding.Default.GetString(buffRead, 0, nCount);
            Console.WriteLine($"收到数据：{strRecv}");

            socket.BeginReceive(buffRead, 0, 1024, 0, OnReceive, socket);
        }

        public void Send(string str)
        {
            string strBody = str;
            byte[] arrBody = Encoding.Default.GetBytes(strBody);
            byte[] arrLen = BitConverter.GetBytes(arrBody.Length);
            byte[] arrSend = arrLen.Concat(arrBody).ToArray();

            foreach (byte info in arrSend)
            {
                Console.Write($" {info}");
            }
            Console.WriteLine();
            Console.WriteLine($"发送一条数据：{strBody}");
            
            socket.Send(arrSend);
        }


    }
}