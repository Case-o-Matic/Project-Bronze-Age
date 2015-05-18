using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Interaction
{
    public abstract class BaseSocket
    {
        protected readonly IPEndPoint localEndPoint;
        protected Socket socket;
        protected Log log;

        public bool isOnline { get; private set; }

        protected BaseSocket(int port)
        {
            localEndPoint = new IPEndPoint(IPAddress.Any, port); // Is IPAddress.Any ok?

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(localEndPoint);

            log.WriteLine("Initialized the server.");
        }

        public virtual void Start()
        {
            isOnline = true;
            log.WriteLine("Started the server.");
        }
        public virtual void Stop()
        {
            socket.Disconnect(true);
            isOnline = false;

            log.WriteLine("Stopped the server.");
            log.SaveLog(".log", true);
        }
    }
}
