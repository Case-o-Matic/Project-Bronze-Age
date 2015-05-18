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
        private int baseUdpClientPort;
        private ClientSocket resourceServerConnection;
        private Log log;

        private Dictionary<int, UdpClientThreadWorldInstanceInfo> worldInstanceServers;

        public GameServer(int port)
        {
            baseUdpClientPort = port;
            resourceServerConnection = new ClientSocket(baseUdpClientPort - 1);
            worldInstanceServers = new Dictionary<int,UdpClientThreadWorldInstanceInfo>();
        }

        public void Start()
        {
            resourceServerConnection.Start(true);
            var worldInstances = RequestWorldInstancesFromResourceServer();

            foreach (var worldinstance in worldInstances)
            {
                var udpClient = new UdpClient(baseUdpClientPort);
                var simulationThread = new Thread(SimulateWorldInstance);
                var serverId = udpClient.GetHashCode();

                var info = new UdpClientThreadWorldInstanceInfo(udpClient, simulationThread, worldinstance);

                worldInstanceServers.Add(serverId, info);
                simulationThread.Start(serverId);
            }
        }

        private WorldInstance[] RequestWorldInstancesFromResourceServer()
        {
            resourceServerConnection.SendMessage(PackageConverter.ConvertToByteArray<ClientResourceRequestPackage>(new ClientResourceRequestPackage(ClientServerResourcePackageType.WorldInstances)));
            var serverAnswer = PackageConverter.ConvertFromByteArray<ServerResourceAnswerPackage>(resourceServerConnection.ReceiveMessage());
            return new WorldInstance[0]; // Apply the row data from the server answer
        }
        private void SimulateWorldInstance(object serverid)
        {

        }

        private struct UdpClientThreadWorldInstanceInfo
        {
            public UdpClient udpClient;
            public Thread simulationThread;
            public WorldInstance worldInstance;

            public UdpClientThreadWorldInstanceInfo(UdpClient udpclient, Thread simulationthread, WorldInstance worldinstance)
            {
                udpClient = udpclient;
                simulationThread = simulationthread;
                worldInstance = worldinstance;
            }
        }
    }
}
