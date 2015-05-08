using CaseoMaticCore;
using ProjectBronzeAge.Core;
using ProjectBronzeAge.Core.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.GameServer
{
    class Program
    {
        public const string version = "1.0.0-alpha.3";
        private static ServerSocket serverSocket;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Project Bronze Age game server (version: " + version + ")");

                #region Port
                setport:
                Console.Write("Port> ");

                int port;
                if (!int.TryParse(Console.ReadLine(), out port))
                {
                    Console.WriteLine("Thats no integer, retry");
                    goto setport;
                }
                if (port < 1200 || port > 65535)
                {
                    Console.WriteLine("The port number must be between 1200 and 65535, retry");
                    goto setport;
                }
                #endregion

                #region User-database
                setuserdbpath:
                Console.Write("User-database filepath> ");

                string userDbFilepath = Console.ReadLine();
                if (!File.Exists(userDbFilepath) || Path.GetExtension(userDbFilepath) != ".accdb")
                {
                    Console.WriteLine("This not a valid access database.");
                    goto setuserdbpath;
                }
                #endregion

                Console.WriteLine("Initializing server...");
                serverSocket = new ServerSocket(port);
                serverSocket.Start(userDbFilepath);
                serverSocket.OnReceiveMessage += Socket_OnReceiveMessage;
                Console.WriteLine("Initialized the server, use \"?\" for help");

                while (true)
                {
                    Console.Write("Command> ");
                    string fullCommand = Console.ReadLine();

                    string commandName = "";
                    string[] commandArgs = new string[0];

                    if (fullCommand.Contains(":"))
                    {
                        string[] fullCommandSplitted = fullCommand.Split(':');
                        commandName = fullCommandSplitted[0];
                        commandArgs = fullCommandSplitted[1].Split(',');
                    }
                    else
                        commandName = fullCommand;

                    switch (commandName)
                    {
                        case "setaccounttype":
                            var userRow = serverSocket.DbSelectRowFromTableString("Users", "Username", commandArgs[0]);

                            var gameDataUserInfo = GameDataUserInfo.FromBytes(ASCIIEncoding.ASCII.GetBytes(userRow[1]));
                            var newRank = GameDataUserInfo.GameDataUserAccountInfo.GameUserAccountInfoRankType.Normal;
                            if (Enum.TryParse<GameDataUserInfo.GameDataUserAccountInfo.GameUserAccountInfoRankType>(commandArgs[1], out newRank))
                                gameDataUserInfo.account.playerType = newRank;
                            else
                                Console.WriteLine("The second argument \"" + commandArgs[1] + "\" is not valid");
                            break;

                        case "stop":
                            serverSocket.Stop();

                            Console.WriteLine("Stopped the server");
                            Console.Read();
                            return;

                        default:
                            Console.WriteLine("The command \"" + commandName + "\" is not valid");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.Read();
            }
        }

        private static void Socket_OnReceiveMessage(Socket socket, SocketMessage msg)
        {
            switch (msg.type)
            {
                case "register":
                    if (serverSocket.DbInsertRowIntoTable("Users", msg.data[0], Guid.NewGuid().ToString(), new GameDataUserInfo(null, new GameDataUserInfo.GameDataUserAccountInfo(GameDataUserInfo.GameDataUserAccountInfo.GameUserAccountInfoRankType.Normal, 2))))
                    {
                        Console.WriteLine("Created a new account for " + msg.data[0]);
                        serverSocket.Send(socket, new SocketMessage("register"));
                    }
                    else
                    {
                        Console.WriteLine("An account for " + msg.data[0] + " already exists. This should not happen!");
                    }
                    break;

                case "login":
                    string[] loginData = serverSocket.DbSelectRowFromTableString("Users", "Username", msg.data[0]);
                    var loginGameDataUserInfo = loginData != null ? loginData[1] : null;
                    if (loginData != null)
                        serverSocket.DbUpdateValueToColumnFromRowInTable("Users", "LoggedIn", true);

                    serverSocket.Send(socket, new SocketMessage("login", loginGameDataUserInfo != null ? "success" : "fail", loginGameDataUserInfo));
                    break;

                case "logout":
                    serverSocket.DbUpdateValueToColumnFromRowInTable("Users", "LoggedIn", false);
                    serverSocket.Send(socket, new SocketMessage("logout" /* Maybe add a state like "success" or "fail"? */));

                    Console.WriteLine(msg.data[0] + " logged out");
                    break;

                case "dchange":
                    string[] logoutData = serverSocket.DbSelectRowFromTableString("Users", "Username", msg.data[0]);
                    if (logoutData != null)
                    {
                        if (logoutData[2] == "true")
                        {
                            if (msg.data[1] == "char")
                            {
                                var dataChangeGameDataUserInfo = GameDataUserInfo.FromBytes(Encoding.ASCII.GetBytes(logoutData[1]));
                                GameDataUserInfo.GameDataUserCharacterInfo appliedChar = null;
                                foreach (var character in dataChangeGameDataUserInfo.characters)
                                {
                                    if (character.name == msg.data[2])
                                    {
                                        var newChar = GameDataUserInfo.GameDataUserCharacterInfo.FromBytes(Encoding.ASCII.GetBytes(msg.data[3]));
                                        character.posX = newChar.posX;
                                        character.posY = newChar.posY;
                                        character.posZ = newChar.posZ;
                                        character.abilities = newChar.abilities;
                                        character.items = newChar.items;
                                        character.genderStyle = newChar.genderStyle;
                                        character.hairStyleIndex = newChar.hairStyleIndex;
                                        character.bodyStyleIndex = newChar.bodyStyleIndex;
                                        character.level = newChar.level;

                                        appliedChar = character;

                                        Console.WriteLine("Changed char data of the user " + msg.data[0]);
                                        serverSocket.Send(socket, new SocketMessage("dchange", "success"));
                                        break;
                                    }
                                }
                                Console.WriteLine(msg.data[0] + " wanted to change data but no character was found");
                            }
                            else if (msg.data[1] == "account")
                            {
                                // TODO: Apply changed account settings
                            }
                            // else if...
                        }
                        else
                            Console.WriteLine("The user " + msg.data[0] + " wanted to change data but isnt logged in");
                    }
                break;

                default:
                    Console.WriteLine("\"" + msg.type + "\" is an unknown type");
                    break;
            }
        }
    }
}
