using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncClient client = new AsyncClient("127.0.0.1", 8888);

            while (true)
            {
                string strInput = Console.ReadLine();
                if (strInput == "q")
                {
                    client.Close();
                    break;
                }
                else
                {
                    client.Send(strInput);
                }
            }
        }
    }
}
