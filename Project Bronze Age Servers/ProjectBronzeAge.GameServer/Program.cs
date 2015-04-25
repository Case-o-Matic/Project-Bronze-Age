using CaseoMaticCore;
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
        public const string version = "1.0.0";
        private static ServerSocket socket;

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
                socket = new ServerSocket(port);
                socket.Start(userDbFilepath);
                socket.OnReceiveMessage += Socket_OnReceiveMessage;
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
                            socket.Stop();

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

        }
    }
}
