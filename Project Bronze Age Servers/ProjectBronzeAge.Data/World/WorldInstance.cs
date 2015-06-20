using ProjectBronzeAge.Core.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProjectBronzeAge.Data
{
    public class WorldInstance : INetworkID
    {
        public const double frameRate = 1000 / 15;

        public delegate void OnUpdateHandler(WorldInstance me, DateTime signaltime);
        public event OnUpdateHandler OnUpdate;

        public string name;
        public Dictionary<int, PlayerActor> players;
        public List<NPCActor> npcs;
        // Add static actors (chests/doors/...)
        // Add navmesh info

        private Timer updateTimer;
        private DateTime lastFrameTime;

        public int networkId
        {
            get;
            private set;
        }

        public WorldInstance()
        {
            lastFrameTime = DateTime.MinValue;
            updateTimer = new Timer(frameRate);
            updateTimer.Elapsed += updateTimer_Elapsed;
            OnUpdate += WorldInstance_OnUpdate;
        }

        public void Initialize()
        {
            foreach (var npc in npcs)
                npc.Start();

            updateTimer.Enabled = true;
        }

        public void AddPlayer(PlayerActor player)
        {
            if (!players.ContainsKey(player.networkId))
            {
                players.Add(player.networkId, player);
                player.Start();
            }
        }
        public void RemovePlayer(PlayerActor player)
        {
            if(players.ContainsKey(player.networkId))
            {
                players.Remove(player.networkId);
            }
        }

        public void ApplyClientRequest(ClientPlayRequestPackage msg)
        {
            var player = players[msg.actorId];
            player.ApplyClientRequest(msg);
        }

        void WorldInstance_OnUpdate(WorldInstance me, DateTime signaltime)
        {
            float deltaTime = lastFrameTime.TimeOfDay.Milliseconds - signaltime.TimeOfDay.Milliseconds; // Is this right?

            for (int i = 0; i < players.Count; i++)
                players[i].Update(deltaTime);
            for (int i = 0; i < npcs.Count; i++)
                npcs[i].Update(deltaTime);

            lastFrameTime = signaltime;
        }

        void updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (OnUpdate != null)
                OnUpdate(this, e.SignalTime);
        }
    }
}
