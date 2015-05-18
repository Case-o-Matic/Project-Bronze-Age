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
    public class ServerSocket : BaseSocket
    {
        public delegate void OnReceiveClientMessageHandler(int connectionid, byte[] bytes);
        public event OnReceiveClientMessageHandler OnReceiveClientMessage;

        private Thread acceptConnectionsThread;
        private Dictionary<int, SocketThreadInfo> connections;

        public ServerSocket(int port)
            : base(port)
        {
            acceptConnectionsThread = new Thread(AcceptConnections);
            connections = new Dictionary<int, SocketThreadInfo>();
        }

        public override void Start()
        {
            acceptConnectionsThread.Start();
            base.Start();
        }
        public override void Stop()
        {
            foreach (var connection in connections.Values)
                connection.socket.Close();
            connections.Clear();

            base.Stop();
        }

        public void SendMessage(int socketid, byte[] bytes)
        {
            byte[] msgBytes = Crypto.EncryptBytes(bytes);
            /*var sentBytes = */connections[socketid].socket.Send(msgBytes);
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
                if (OnReceiveClientMessage != null)
                    OnReceiveClientMessage((int)connectionid, Crypto.DecryptBytes(buffer));
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
