using Json;
using ProjectBronzeAge.Central;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CaseoMaticServer
{
    class Program
    {
        private const float checkBlockedUsersTimeIntervall = 60000;

        public static string version = "1.00";
        private static ServerSocket serverSocket;

        private static Timer checkBlockedUsersForLoginTime;
        private static Dictionary<string, float> blockedUsersForLogin;

        static void Main(string[] args)
        {
            serverSocket.Log("Project Bronze Age v" + version);

            port:
            Console.Write("Server port> ");
            string portText = Console.ReadLine();

            int port;
            if (!int.TryParse(portText, out port))
            {
                serverSocket.Log(portText + " is not a valid port number, retry");
                goto port;
            }
            if(port < 4500 || port > 65535)
            {
                serverSocket.Log("The port number must be between 4500 and 65535");
                goto port;
            }

            blockedUsersForLogin = new Dictionary<string, float>();
            checkBlockedUsersForLoginTime = new Timer(checkBlockedUsersTimeIntervall);
            checkBlockedUsersForLoginTime.Elapsed += CheckBlockedUsersForLoginTime;

            serverSocket = new ServerSocket(port);
            serverSocket.OnReceiveMessage += ServerSocket_OnReceiveMessage;

            start:

            serverSocket.Log("Press \"Enter\" to start the server");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                accountdbpath:
                Console.Write("Account-DB path> ");
                string accountDbPath = Console.ReadLine();
                if (!File.Exists(accountDbPath) || Path.GetExtension(accountDbPath) != ".accdb")
                    goto accountdbpath;

                serverSocket.Start(accountDbPath);
                checkBlockedUsersForLoginTime.Start();

                bool breakLoop = false;
                while(!breakLoop)
                {
                    string input = "";
                    try
                    {
                        input = Console.ReadLine();
                        string cmd = input.Split(':')[0];
                        string[] cmdArgs = input.Split(':')[1].Split(',');

                        switch (cmd)
                        {
                            case "stop":
                                breakLoop = true;
                                break;

                            case "blockuser":
                                string username = cmdArgs[0];
                                float blockTimeInSeconds = int.Parse(cmdArgs[0]);
                                blockedUsersForLogin.Add(username, blockTimeInSeconds * 1000);
                                break;

                            default:
                                serverSocket.Log(cmd + " is not a valid command");
                                continue;
                        }
                    }
                    catch(Exception ex)
                    {
                        serverSocket.Log("Exception while executing the command \"" + input + "\": " + ex.ToString());
                    }
                }
            }
            else
                goto start;

            if (serverSocket != null)
                serverSocket.Stop();
            if (checkBlockedUsersForLoginTime != null)
                checkBlockedUsersForLoginTime.Dispose();
        }

        private static void CheckBlockedUsersForLoginTime(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < blockedUsersForLogin.Count; i++)
            {
                var blockedUser = blockedUsersForLogin.ElementAt(i);
                blockedUsersForLogin[blockedUser.Key] -= checkBlockedUsersTimeIntervall;
                if (blockedUsersForLogin.ElementAt(i).Value <= 0)
                    blockedUsersForLogin.Remove(blockedUser.Key);
            }
        }

        private static void ServerSocket_OnReceiveMessage(SocketMessage msg)
        {
            switch (msg.type)
            {
                case "login":
                    string loginusername = msg.data[0];
                    string loginpw = msg.data[1];

                    string[] loginRow = serverSocket.DbSelectRowFromTable("AccountsCredentials", loginusername);
                    if (loginRow != null)
                    {
                        if (loginRow[1] == loginpw)
                        {
                            if(blockedUsersForLogin.ContainsKey(loginusername))
                            {
                                serverSocket.Log(loginusername + " tried to log into his account but is blocked for " + (blockedUsersForLogin[loginusername] / 1000) + " ms");
                            }
                            serverSocket.Log("\"" + loginusername + "\" logged into his account");
                            serverSocket.Send(new SocketMessage("loginsuccess", loginpw, loginRow[2]));
                        }
                        else
                        {
                            serverSocket.Log("\"" + loginusername + "\" couldnt log into his account");
                            serverSocket.Send(new SocketMessage("loginfail", "pw"));
                        }
                    }
                    else
                    {
                        serverSocket.Send(new SocketMessage("loginfail", "name"));
                        serverSocket.Log("The username \"" + loginusername + "\" does not exist");
                    }
                    break;

                case "register":
                    string registerusername = msg.data[0];
                    string registerpw = msg.data[1];
                    string registeremail = msg.data[2];

                    string[] registerRow = serverSocket.DbSelectRowFromTable("AccountsCredentials", registerusername);
                    if (registerRow != null)
                    {
                        serverSocket.Send(new SocketMessage("registerfail", "alreadyexists"));
                        serverSocket.Log("Registering the account \"" + registerusername + "\" failed, username already exists");
                    }
                    else
                    {
                        string dataId = Guid.NewGuid().ToString();
                        serverSocket.DbInsertRowIntoTable("AccountsCredentials", registerusername, registerpw, registeremail, dataId);
                        serverSocket.DbInsertRowIntoTable("AccountsData", dataId, JsonParser.Serialize<AccountSettings>(new AccountSettings()));

                        serverSocket.Send(new SocketMessage("registersuccess", dataId));
                        serverSocket.Log("\"" + registerusername + "\" has been registered");
                    }
                    break;

                case "unregister":
                    string unregisterusername = msg.data[0];
                    string unregisterpw = msg.data[1];

                    string[] unregisterRow = serverSocket.DbSelectRowFromTable("AccountsCredentials", unregisterusername);
                    if(unregisterRow != null)
                    {
                        if(unregisterRow[2] == unregisterpw)
                        {
                            if (serverSocket.DbDeleteRowFromTable("AccountsCredentials", unregisterusername))
                            {
                                serverSocket.DbDeleteRowFromTable("AccountsData", unregisterRow[3]);
                                serverSocket.Send(new SocketMessage("unregistersuccess"));
                                serverSocket.Log("Unregistered \"" + unregisterusername + "\" successfully");
                            }
                            else
                            {
                                serverSocket.Send(new SocketMessage("unregisterfail", "internal"));
                                serverSocket.Log("Unregistering \"" + unregisterusername + "\" was unsuccessful");
                            }
                        }
                        else
                        {
                            serverSocket.Send(new SocketMessage("unregisterfail", "pw"));
                            serverSocket.Log("\"" + unregisterusername + "\" cant unregister his account because he gave wrong credentials");
                        }
                    }
                    else
                    {
                        serverSocket.Send(new SocketMessage("unregisterfail", "notfound"));
                        serverSocket.Log("\"" + unregisterusername + "\" does not exist");
                    }
                    break;

                case "getdata":
                    string dataid = msg.data[0];
                    string[] dataRow = serverSocket.DbSelectRowFromTable("AccountsData", dataid);
                    if(dataRow != null)
                    {
                        serverSocket.Send(new SocketMessage("getdatasuccess", dataRow[1] /* TODO: Add new data items if more data gets stored in the database */));
                    }
                    else
                    {
                        serverSocket.Send(new SocketMessage("getdatafail", "notfound"));
                    }
                    break;

                default:
                    serverSocket.Log("The message type \"" + msg.type + "\" is unknown");
                    break;
            }
        }
    }
}
