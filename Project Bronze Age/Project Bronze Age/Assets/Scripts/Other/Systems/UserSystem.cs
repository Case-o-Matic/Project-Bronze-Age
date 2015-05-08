using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ProjectBronzeAge.Core;
using CaseoMaticCore;

public class UserSystem : MonoBehaviour, ILoadable
{
    public const int gameDataServerClientPort = 42002;
    public static UserSystem Instance;

    public IPEndPoint gameDataServerEndPoint;
    public GameDataServerConnectionMode gameDataServerConnectionMode = GameDataServerConnectionMode.StartUp;

    public string caseomaticUsername;
    public GameDataUserInfo userInfo;
    public string projectBronzeAgeUserId;

    private ClientSocket socket;

    public float progress
    {
        get;
        private set;
    }
    public bool isLoading
    {
        get;
        private set;
    }
    public bool isDone
    {
        get;
        private set;
    }
    public bool loadErrorOccured
    {
        get;
        private set;
    }

    public void Reconnect()
    {
        // TODO: Check this
        if (gameDataServerConnectionMode == GameDataServerConnectionMode.Aborted)
        {
            CloseGameServerConnection();
            ConnectToGameServer();
        }
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ReceiveUserData();
        ConnectToGameServer();
    }
    void OnApplicationQuit()
    {
        CloseGameServerConnection();
    }

    private void ReceiveUserData()
    {
        try
        {
            gameDataServerConnectionMode = GameDataServerConnectionMode.Initializing;
            caseomaticUsername = Environment.GetCommandLineArgs()[1];

            progress += 25;
        }
        catch (Exception)
        {
            Debug.LogError("The application got started without commandline arguments. Aborting...");
            gameDataServerConnectionMode = GameDataServerConnectionMode.Aborted;

            loadErrorOccured = true;
        }
    }
    private void ConnectToGameServer()
    {
        if (caseomaticUsername == "")
        {
            Debug.Log("You cannot connect to the game server without a specified username.");
            return;
        }
        if (gameDataServerConnectionMode == GameDataServerConnectionMode.Connected)
        {
            Debug.Log("You cannot connect to the game server if you already are connected.");
            return;
        }

        try
        {
            isLoading = true;
            gameDataServerEndPoint = new IPEndPoint(IPAddress.Loopback, 42001); // Find out the game server endpoint

            socket = new ClientSocket(gameDataServerClientPort);
            socket.Connect(gameDataServerEndPoint);
            progress += 25;

            SendGameServerMessage("login", caseomaticUsername, projectBronzeAgeUserId);
            progress += 25;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            gameDataServerConnectionMode = GameDataServerConnectionMode.Aborted;
            loadErrorOccured = true;
        }
    }
    private void ReceiveGameServerMessages()
    {
        try
        {
            gameDataServerConnectionMode = GameDataServerConnectionMode.Connected;
            while (gameDataServerConnectionMode != GameDataServerConnectionMode.Aborted)
            {
                var msg = socket.Receive();

                switch (msg.type)
                {
                    case "register":
                        break;

                    case "login":
                        if (msg.data[0] == "success")
                        {
                            userInfo = GameDataUserInfo.FromBytes(ASCIIEncoding.ASCII.GetBytes(msg.data[1]));
                            projectBronzeAgeUserId = msg.data[2];

                            progress += 25;
                            isDone = true;
                            isLoading = false;
                        }
                        else
                        {
                            Debug.LogError("Your username does not exist, creating account...");
                            SendGameServerMessage("register", caseomaticUsername);
                        }
                        break;

                    case "logout":
                        gameDataServerConnectionMode = GameDataServerConnectionMode.ShutDown;
                        break;

                    case "dchange":
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
    private void SendGameServerMessage(string type, params string[] args)
    {
        try
        {
            socket.Send(new SocketMessage(type, args));
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            gameDataServerConnectionMode = GameDataServerConnectionMode.Aborted;
        }
    }
    private void CloseGameServerConnection()
    {
        try
        {
            if (gameDataServerConnectionMode == GameDataServerConnectionMode.Connected)
            {
                SendGameServerMessage("logout", projectBronzeAgeUserId);

                socket.Stop();
                socket = null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
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
    ShutDown,
    Aborted
}