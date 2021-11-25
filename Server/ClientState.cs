/* ==============================================================================
* 功能描述：ClientState  
* 创 建 者：Halo
* 创建日期：2021/11/25 22:00:50
* ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ClientState
    {
        public Socket socket;
        public byte[] buffRead = new byte[1024];
    }
}