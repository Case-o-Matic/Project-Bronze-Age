using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Interaction
{
    public class ServerSocket
    {
        public delegate void OnReceiveMessageHandler(int connectionid, byte[] bytes);
        public event OnReceiveMessageHandler OnReceiveMessage;

        public readonly IPEndPoint localEndPoint;

        private Socket socket;
        private Thread acceptConnectionsThread;
        private Dictionary<int, SocketThreadInfo> connections;
        private Log log;

        public bool isOnline { get; private set; }

        public ServerSocket(int port)
        {
            localEndPoint = new IPEndPoint(IPAddress.Any, port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            acceptConnectionsThread = new Thread(AcceptConnections);
            connections = new Dictionary<int, SocketThreadInfo>();
            log = new Log(true);
        }

        public void Start()
        {
            isOnline = true;

            socket.Bind(localEndPoint);
            acceptConnectionsThread.Start();

            log.WriteLine("Started the server on " + localEndPoint.ToString() + ".");
        }
        public void Stop()
        {
            socket.Disconnect(true);
            isOnline = false;

            log.WriteLine("Stopped the server.");
            log.SaveLog("server.log", true);
        }

        public void SendMessage(int socketid, byte[] bytes)
        {
            byte[] msgBytes = Crypto.EncryptBytes(bytes);
            /*var receivedBytes = */connections[socketid].socket.Send(msgBytes);
        }

        private void AcceptConnections()
        {
            while(isOnline)
            {
                socket.Listen(1);
                var newSocket = socket.Accept();
                var newReceiveThread = new Thread(ReceiveMessages);
                var connectionId = newSocket.GetHashCode() + newReceiveThread.GetHashCode();

                connections.Add(connectionId, new SocketThreadInfo(newSocket, newReceiveThread));
                newReceiveThread.Start(connectionId);

                log.WriteLine("Accepted the client " + newSocket.RemoteEndPoint.ToString() + " (connection-ID: " + connectionId + ").");
            }
        }

        private void ReceiveMessages(object connectionid)
        {
            var sock = connections[(int)connectionid].socket;
            while(isOnline)
            {
                var buffer = new byte[2048];
                /*var receivedBytes = */sock.Receive(buffer);
                if (OnReceiveMessage != null)
                    OnReceiveMessage((int)connectionid, Crypto.DecryptBytes(buffer));
                log.WriteLine("Received a message from " + sock.RemoteEndPoint.ToString() + " (connection-ID: " + connectionid + ")");
            }
        }

        private struct SocketThreadInfo
        {
            public Socket socket;
            public Thread receiveThread;

            public SocketThreadInfo(Socket socket, Thread receivethread)
            {
                this.socket = socket;
                this.receiveThread = receivethread;
            }
        }
    }
}
