using ProjectBronzeAge.Core.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using System.Runtime.InteropServices;
using ProjectBronzeAge.Interaction;

namespace ProjectBronzeAge.GamePlayServer
{
    class Program
    {
        public const string version = "1.0.0-alpha.1";
        private static GameServer server;

        static void Main(string[] args)
        {
            Console.WriteLine("Project Bronze Age GamePlay-server (version: " + version + ")");

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

            Console.WriteLine("Initializing server...");
            server = new GameServer(12802);

            Console.Read();
        }
    }
}
