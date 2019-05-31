using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace YummyGame.Framework
{
    public class TcpClient : Client
    {
        byte[] netBuffer = new byte[4096];
        public TcpClient(string host, int port) : base(host, port)
        {
        }

        protected override void StartReceive(Socket s)
        {
            s.BeginReceive(netBuffer, 0, 4096, SocketFlags.None, (ar) =>
            {
                Socket _s = (Socket)ar.AsyncState;
                _s.EndReceive(ar);
            }, s);
        }

        protected override Socket OnSocketInit()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
