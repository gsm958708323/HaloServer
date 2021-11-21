/* ==============================================================================
* 功能描述：SyncClient  
* 创 建 者：Halo
* 创建日期：2021/11/21 17:12:36
* ==============================================================================*/
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    /// <summary>
    /// 同步客户端
    /// </summary>
    public class SyncClient
    {
        public SyncClient()
        {
            Console.WriteLine("Hello World!");

            //Connect
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect("127.0.0.1", 8888);
            Console.WriteLine("客户端连接服务器成功");

            //Send
            string strBody = Console.ReadLine();
            byte[] arrBody = Encoding.Default.GetBytes(strBody);
            byte[] arrLen = BitConverter.GetBytes(arrBody.Length);
            byte[] arrSend = arrLen.Concat(arrBody).ToArray();
            client.Send(arrSend);
            Console.WriteLine($"发送一条数据：{strBody}");

            //Recv
            byte[] arrRead = new byte[1024];
            int nCount = client.Receive(arrRead);
            string strRecv = Encoding.Default.GetString(arrRead);
            Console.WriteLine($"接收一条数据：{strRecv}");


            //Close
            client.Close();

            Console.ReadKey();
        }
    }
}