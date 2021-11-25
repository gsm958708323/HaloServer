using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SelectServer server = new SelectServer("127.0.0.1", 8888);
            while(true)
            {
                server.Tick();
                Thread.Sleep(10);
            }
        }
    }
}
