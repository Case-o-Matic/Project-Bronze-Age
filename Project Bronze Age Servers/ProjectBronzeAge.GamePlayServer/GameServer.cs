using ProjectBronzeAge.Core.Communication;
using ProjectBronzeAge.Data;
using ProjectBronzeAge.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectBronzeAge.GamePlayServer
{
    public class GameServer
    {
        public const float networkSendRate = 1000 / 15; // 66.666
        public const int clientPort = 18001;

        private int udpClientPort;
        private ClientSocket resourceServerConnection;
        private Log log;

        private Dictionary<int, SimulationThreadInfo> worldInstanceServers;

        public GameServer(int port)
        {
            udpClientPort = port;
            resourceServerConnection = new ClientSocket(udpClientPort - 1);
            worldInstanceServers = new Dictionary<int,SimulationThreadInfo>();
            log = new Log(true, true);
        }

        public void Start()
        {
            resourceServerConnection.Start(true);
            log.WriteLine("Started the game server");
            var worldInstances = RequestWorldInstancesFromResourceServer();
            log.WriteLine("Received world instances...");

            foreach (var worldinstance in worldInstances)
            {
                var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                var simulationThread = new Thread(SimulateWorldInstance);
                var networkSendTimer = new System.Timers.Timer(networkSendRate);
                var broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, clientPort);
                var id = worldinstance.GetHashCode();

                udpSocket.Bind(new IPEndPoint(IPAddress.Any, udpClientPort));
                udpSocket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.Broadcast, 1);

                var info = new SimulationThreadInfo(udpSocket, simulationThread, worldinstance, broadcastEndPoint, networkSendTimer);

                worldInstanceServers.Add(id, info);
                simulationThread.Start(id);

                udpClientPort += 1;
                log.WriteLine("Initialized simulation thread " + id);
            }
            log.WriteLine("Finished initializing");
        }

        private WorldInstance[] RequestWorldInstancesFromResourceServer()
        {
            resourceServerConnection.SendMessage(PackageConverter.ConvertToByteArray<ClientResourceRequestPackage>(new ClientResourceRequestPackage(ClientServerResourcePackageType.WorldInstances)));
            var serverAnswer = PackageConverter.ConvertFromByteArray<ServerResourceAnswerPackage>(resourceServerConnection.ReceiveMessage());
            return new WorldInstance[0]; // Apply the row data from the server answer
        }
        private void SimulateWorldInstance(object serverid)
        {
            var info = worldInstanceServers[(int)serverid];
            info.worldInstance.Initialize();
            info.worldInstance.OnUpdate += worldInstance_OnUpdate;
            //var playerEndPoints = new List<IPEndPoint>();

            while(true)
            {
                var buffer = new byte[1024];
                //var endPoint = new IPEndPoint(IPAddress.Any, 0);
                /*var receivedBytesCount = */info.udpSocket.Receive(buffer);

                var msg = PackageConverter.ConvertFromByteArray<ClientPlayRequestPackage>(buffer);
                info.worldInstance.ApplyClientRequest(msg);
            }
        }

        void worldInstance_OnUpdate(WorldInstance me, DateTime signaltime)
        {
            var info = worldInstanceServers[me.GetHashCode()];
            foreach (var npc in info.worldInstance.npcs)
            {
                info.udpSocket.SendTo(PackageConverter.ConvertToByteArray<ServerPlayEventPackage>(npc.nextEventPackage), info.broadcastEndPoint);
                info.udpSocket.SendTo(PackageConverter.ConvertToByteArray<ServerPlayStatePackage>(npc.nextStatePackage), info.broadcastEndPoint);
            }
            foreach (var player in info.worldInstance.players.Values)
            {
                info.udpSocket.SendTo(PackageConverter.ConvertToByteArray<ServerPlayEventPackage>(player.nextEventPackage), info.broadcastEndPoint);
                info.udpSocket.SendTo(PackageConverter.ConvertToByteArray<ServerPlayStatePackage>(player.nextStatePackage), info.broadcastEndPoint);
            }
        }

        private class SimulationThreadInfo
        {
            public Socket udpSocket;
            public Thread simulationThread;
            public WorldInstance worldInstance;
            public IPEndPoint broadcastEndPoint;
            public System.Timers.Timer networkSendTimer;

            public SimulationThreadInfo(Socket udpclient, Thread simulationthread, WorldInstance worldinstance, IPEndPoint broadcastendpoint, System.Timers.Timer networksendtimer)
            {
                udpSocket = udpclient;
                simulationThread = simulationthread;
                worldInstance = worldinstance;
                broadcastEndPoint = broadcastendpoint;
                networkSendTimer = networksendtimer;
            }
        }
    }
}
