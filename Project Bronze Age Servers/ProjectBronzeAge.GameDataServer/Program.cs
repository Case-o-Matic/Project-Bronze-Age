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
        public const string version = "1.0.0-alpha.1";
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
                Console.WriteLine("User-database filepath> ");
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

                    string commandName;
                    string[] commandArgs;

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
                        case "stop":
                            serverSocket.Stop();

                            Console.WriteLine("Stopped the server");
                            Console.Read();
                            break;

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
                case "auth":
                    var authMsg = PackageConverter.ConvertFromByteArray<ClientDataAuthPackage>(Encoding.ASCII.GetBytes(msg.data[0]));
                    
                    if(authMsg.type == ClientDataAuthPackageType.Login)
                    {
                        string[] data = serverSocket.DbSelectRowFromTableString("Users", "Username", authMsg.username);
                        var gamedatauserinfo = data != null ? data[1] : null;
                        var serverPackage = new ServerDataAuthPackage(data != null ? ServerDataAuthPackageType.LoginValid : ServerDataAuthPackageType.LoginInvalid, gamedatauserinfo);

                        serverSocket.Send(socket, new SocketMessage("auth", Encoding.ASCII.GetString(PackageConverter.ConvertToByteArray<ServerDataAuthPackage>(serverPackage))));
                    }
                    else if(authMsg.type == ClientDataAuthPackageType.Logout)
                    {
                        string[] data = serverSocket.DbSelectRowFromTableString("Users", "Username", authMsg.username);
                        if (data != null)
                        {
                            var gamedatauserinfo = GameDataUserInfo.FromBytes(Encoding.ASCII.GetBytes(data[1]));
                            GameDataUserInfo.GameDataUserCharacterInfo appliedChar = null;
                            foreach (var character in gamedatauserinfo.characters)
                            {
                                if (character.name == authMsg.logoutPlayedCharName)
                                {
                                    var newChar = GameDataUserInfo.GameDataUserCharacterInfo.FromBytes(Encoding.ASCII.GetBytes(authMsg.logoutPlayedCharNewInfo));
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
                                }
                            }
                            if (appliedChar == null)
                                Console.WriteLine("The user \"" + authMsg.username + "\" tried to logout with the character \"" + authMsg.logoutPlayedCharName + "\" but it doesnt exist");
                            else
                                serverSocket.DbApplyValuesToRowFromTable("Users", "Username", authMsg.username, authMsg.username, Encoding.ASCII.GetString(GameDataUserInfo.GameDataUserCharacterInfo.ToBytes(appliedChar)));
                        }
                        else
                            Console.WriteLine("The user \"" + authMsg.username + "\" tried to log out but he doesnt exist");
                    }
                    break;

                case "dchange":
                    break;

                default:
                    Console.WriteLine("\"" + msg.type + "\" is an unknown type");
                    break;
            }
        }
    }
}
