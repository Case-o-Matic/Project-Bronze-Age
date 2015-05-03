using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ProjectBronzeAge.Core;

public class UserSystem : MonoBehaviour
{
    public const int gameDataServerClientPort = 42002;

    public IPEndPoint gameDataServerEndPoint;
    public GameDataServerConnectionMode gameDataServerConnectionMode = GameDataServerConnectionMode.StartUp;

    public string caseomaticUsername;
    public GameUserInfo userInfo;
    public string projectBronzeAgeUserId;

    private Socket socket;
    private Thread receiveThread;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        ReceiveUserData();
        ConnectToGameServer();
    }

    private void ReceiveUserData()
    {
        try
        {
            gameDataServerConnectionMode = GameDataServerConnectionMode.Initializing;

            string[] args = Environment.GetCommandLineArgs();
            caseomaticUsername = args[1];
            projectBronzeAgeUserId = args[2];
        }
        catch (Exception)
        {
            Debug.LogError("The application got started without commandline arguments. Aborting...");
            gameDataServerConnectionMode = GameDataServerConnectionMode.Aborted;
        }
    }
    private void ConnectToGameServer()
    {
        if (gameDataServerConnectionMode == GameDataServerConnectionMode.Aborted)
            return;

        try
        {
            gameDataServerEndPoint = new IPEndPoint(IPAddress.Loopback, 42001); // Findout the game server endpoint

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, gameDataServerClientPort));
            socket.Connect(gameDataServerEndPoint);

            receiveThread = new Thread(ReceiveGameServerMessages);
            receiveThread.Start();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            gameDataServerConnectionMode = GameDataServerConnectionMode.Aborted;
        }
    }
    private void ReceiveGameServerMessages()
    {
        try
        {
            gameDataServerConnectionMode = GameDataServerConnectionMode.Connected;
            while (gameDataServerConnectionMode != GameDataServerConnectionMode.Aborted)
            {
                byte[] msgBuffer = new byte[0x800];
                socket.Receive(msgBuffer);

                GameUserMessage message = GameUserMessage.ToMessage(msgBuffer);
                switch (message.type)
                {
                    case GameUserMessage.GameUserMessageType.Login:

                        break;
                    case GameUserMessage.GameUserMessageType.Logout:

                        break;
                    case GameUserMessage.GameUserMessageType.GetUserData:

                        break;
                    case GameUserMessage.GameUserMessageType.SetUserData:

                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            gameDataServerConnectionMode = GameDataServerConnectionMode.Aborted;
        }
    }
    private void SendGameServerMessage(GameUserMessage message)
    {
        try
        {
            byte[] msgInBytes = GameUserMessage.ToBytes(message);
            socket.Send(msgInBytes);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            gameDataServerConnectionMode = GameDataServerConnectionMode.Aborted;
        }
    }
    private void CloseGameServerConnection()
    {
        if(socket != null)
        {
            SendGameServerMessage(new GameUserMessage(GameUserMessage.GameUserMessageType.Logout, projectBronzeAgeUserId));

            socket.Close(150);
            socket = null;

            gameDataServerConnectionMode = GameDataServerConnectionMode.Aborted;
        }
    }
}

[Serializable]
public enum GameDataServerConnectionMode
{
    StartUp,
    Initializing,
    Connected,
    Aborted
}