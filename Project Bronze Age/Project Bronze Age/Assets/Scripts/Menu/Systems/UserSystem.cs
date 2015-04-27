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
    public const int gameServerClientPort = 42002;

    public IPEndPoint gameServerEndPoint;
    public GameServerConnectionMode gameServerConnectionMode = GameServerConnectionMode.StartUp;

    public string caseomaticUsername;
    public GameUserInfo userInfo;
    public string projectBronzeAgeUserId;

    private Socket socket;
    private Thread receiveThread;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ReceiveUserData();
        ConnectToGameServer();
    }

    private void ReceiveUserData()
    {
        try
        {
            gameServerConnectionMode = GameServerConnectionMode.Initializing;

            string[] args = Environment.GetCommandLineArgs();
            caseomaticUsername = args[0];
            projectBronzeAgeUserId = args[1];

            //using (var pipe = new NamedPipeClientStream("CaseoMaticClient Game-Pipeserver", "Game-Pipeclient"))
            //{
            //    pipe.Connect();
            //    using (var reader = new StreamReader(pipe))
            //    {
            //        username = reader.ReadLine();
            //        projectBronzeAgeGameId = int.Parse(reader.ReadLine());
            //    }
            //}
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            gameServerConnectionMode = GameServerConnectionMode.Aborted;
        }
    }
    private void ConnectToGameServer()
    {
        try
        {
            gameServerEndPoint = new IPEndPoint(IPAddress.Loopback, 42001); // Findout the game server endpoint

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, gameServerClientPort));
            socket.Connect(gameServerEndPoint);

            receiveThread = new Thread(ReceiveGameServerMessages);
            receiveThread.Start();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            gameServerConnectionMode = GameServerConnectionMode.Aborted;
        }
    }
    private void ReceiveGameServerMessages()
    {
        try
        {
            gameServerConnectionMode = GameServerConnectionMode.Running;
            while (socket != null)
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
            gameServerConnectionMode = GameServerConnectionMode.Aborted;
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
            gameServerConnectionMode = GameServerConnectionMode.Aborted;
        }
    }
    private void CloseGameServerConnection()
    {
        if(socket != null)
        {
            SendGameServerMessage(new GameUserMessage(GameUserMessage.GameUserMessageType.Logout, projectBronzeAgeUserId));

            socket.Close(150);
            socket = null;
        }
    }
}

[Serializable]
public enum GameServerConnectionMode
{
    StartUp,
    Initializing,
    Running,
    Aborted
}