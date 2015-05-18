using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Interaction
{
    public class ClientSocket : BaseSocket
    {
        public delegate void OnReceiveServerMessageHandler(byte[] bytes);
        public event OnReceiveServerMessageHandler OnReceiveServerMessage;

        private Thread receiveThread;

        public ClientSocket(int port)
            : base(port)
        { }

        public new void Start(bool manualreceive)
        {
            if (!manualreceive)
            {
                receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();
            }

            base.Start();
        }
        public override void Stop()
        {
            base.Stop();
        }

        public void SendMessage(byte[] bytes)
        {
            var msgEncrypted = Crypto.EncryptBytes(bytes);
            /*var sentBytes = */socket.Receive(msgEncrypted);
        }
        public byte[] ReceiveMessage()
        {
            var buffer = new byte[2048];
            /*var receivedBytes = */socket.Receive(buffer);
            return Crypto.DecryptBytes(buffer);
        }

        private void ReceiveMessages()
        {
            while (isOnline)
            {
                var receivedMsg = ReceiveMessage();
                if (OnReceiveServerMessage != null)
                    OnReceiveServerMessage(receivedMsg);
            }
        }
    }
}
