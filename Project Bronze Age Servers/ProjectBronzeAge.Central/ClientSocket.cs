﻿using Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ProjectBronzeAge.Central
{
    public class ClientSocket
    {
        private const string logFilePath = "client.log";

        public delegate void OnReceiveMessageHandler(SocketMessage msg);
        public event OnReceiveMessageHandler OnReceiveMessage;

        public bool isConnected { get; private set; }
        public IPEndPoint localEndPoint { get; private set; }

        private Socket socket;
        private Thread receiveThread;
        private List<string> logFileLines;

        public ClientSocket(int port)
        {
            try
            {
                localEndPoint = new IPEndPoint(IPAddress.Any, port);

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                receiveThread = new Thread(ReceiveMessages);
            }
            catch (Exception ex)
            {
                Log("Exception while initializing a new client: " + ex.ToString());
            }
        }

        public void Connect(IPEndPoint serverendpoint)
        {
            try
            {
                socket.Bind(localEndPoint);
                socket.Connect(serverendpoint);
                receiveThread.Start();
                logFileLines = new List<string>();

                isConnected = true;
                Log("Connected to the server");
            }
            catch (Exception ex)
            {
                Log("Exception while connecting to the server: " + ex.ToString());
            }
        }
        public void Stop()
        {
            try
            {
                socket.Disconnect(true);
                isConnected = false;

                Log("Stopped the client");
            }
            catch (Exception ex)
            {
                Log("Exception while disconnecting:" + ex.ToString());
            }
            finally
            {
                WriteLogToFile();
            }
        }

        public void Send(SocketMessage socketmessage)
        {
            try
            {
                string messageSerialized = JsonParser.Serialize<SocketMessage>(socketmessage);
                string messageSerializedAndEncrypted = Crypto.EncryptString(messageSerialized, Properties.Resources.CryptoPassword + "-client");

                byte[] messageInBytes = Convert.FromBase64String(messageSerializedAndEncrypted);
                socket.Send(messageInBytes);
            }
            catch (Exception ex)
            {
                Log("Exception while sending a message: " + ex.ToString());
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    byte[] messageInBytes = new byte[256];
                    socket.Receive(messageInBytes);

                    string message = Crypto.DecryptString(Convert.ToBase64String(messageInBytes), Properties.Resources.CryptoPassword + "-server");
                    SocketMessage socketMessage = JsonParser.Deserialize<SocketMessage>(Convert.ToBase64String(messageInBytes));
                    if (OnReceiveMessage != null)
                        OnReceiveMessage(socketMessage);
                }
            }
            catch (Exception ex)
            {
                Log("Exception while receiving messages: " + ex.ToString());
            }
        }
        private void Log(string msg)
        {
            Console.WriteLine("Client, " + DateTime.Now.ToString() + ": " + msg);
        }
        private void WriteLogToFile()
        {
            File.WriteAllLines(logFilePath, logFileLines.ToArray());
        }
    }
}
