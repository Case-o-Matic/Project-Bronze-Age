using ProjectBronzeAge.Core;
using ProjectBronzeAge.Core.Communication;
using ProjectBronzeAge.Interaction;
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
                Console.WriteLine("Project Bronze Age GameData-server (version: " + version + ")");

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
                serverSocket.Start();
                serverSocket.OnReceiveClientMessage += Socket_OnReceiveMessage;
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
                            break;

                        case "stop":
                            serverSocket.Stop();
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

        private static void Socket_OnReceiveMessage(int connectionid, byte[] bytes)
        {
            var package = PackageConverter.ConvertFromByteArray<IPackage>(bytes);
            if (package is ClientDataAuthPackage)
            {
                var authPackage = (ClientDataAuthPackage)package;
                switch (authPackage.type)
                {
                    case ClientDataAuthPackageType.Login:
                        break;
                    case ClientDataAuthPackageType.Logout:
                        break;
                    case ClientDataAuthPackageType.Register:
                        break;
                    case ClientDataAuthPackageType.Unregister:
                        break;
                }
            }
            else if(package is ClientDataChangePackage)
            {
                var changePackage = (ClientDataChangePackage)package;
                switch (changePackage.type)
                {
                    case ClientDataChangePackageType.GetData:
                        break;
                    case ClientDataChangePackageType.CreateChar:
                        break;
                    case ClientDataChangePackageType.RemoveChar:
                        break;
                    case ClientDataChangePackageType.SetRank:
                        break;
                }
            }
        }
    }
}
