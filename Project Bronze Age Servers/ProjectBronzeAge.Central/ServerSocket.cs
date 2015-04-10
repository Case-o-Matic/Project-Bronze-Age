using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ProjectBronzeAge.Central
{
    public class ServerSocket
    {
        public const int serverPort = 30303;

        public bool isOnline { get; private set; }

        private IPEndPoint localEndPoint;

        private Socket socket;
        private Dictionary<Socket, Thread> socketConnections;

        public ServerSocket()
        {
            localEndPoint = new IPEndPoint(IPAddress.Any, serverPort);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4);
            socketConnections = new Dictionary<Socket, Thread>();
        }

        public void Start()
        {
            socket.Bind(localEndPoint);

            while (isOnline)
            {
                socket.Listen(1);

                Socket newSocket = socket.Accept();
                Thread receiveThread = new Thread(ReceiveMessages);

                socketConnections.Add(newSocket, receiveThread);
                receiveThread.Start(newSocket);
            }
        }

        private void ReceiveMessages(object socket)
        {
            Socket sock = socket as Socket;
            while (isOnline)
            {
                byte[] msgInBytes = new byte[256];
                sock.Receive(msgInBytes);

                string message = Crypto.DecryptString(Convert.ToBase64String(msgInBytes), Properties.Resources.CryptoPassword);
                string msgHeader = message.Split(':')[0];
                string[] msgSplitted = message.Split(':')[1].Split(',');

                switch (msgHeader)
                {
                    case "login":
                        string username = msgSplitted[0];
                        string password = msgSplitted[1];
                        break;

                    default:
                        break;
                }
            }
        }

        private void InitializeDB()
        {

        }
        private bool LoginDB(string username, string password)
        {

        }
    }
}
