using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProjectBronzeAge.Data
{
    public class WorldInstance : INetworkID, IResourceID<WorldInstance>
    {
        public const double frameRate = 1000 / 30;

        public string name;
        public List<PlayerActor> players;
        public List<int> npcIds;
        public List<NpcActor> npcs;
        // Add static actors (chests/doors/...)
        // Add navmesh info

        private Timer updateTimer;
        private DateTime lastFrameTime;

        public int networkId
        {
            get;
            private set;
        }
        public int resourceId
        {
            get;
            set;
        }

        public WorldInstance()
        {
            npcIds.ForEach((nid) => { npcs.Add(ResourceManager.GetNPC(nid)); });

            lastFrameTime = DateTime.MinValue;
            updateTimer = new Timer(frameRate);
            updateTimer.Elapsed += updateTimer_Elapsed;
        }

        public void Initialize(List<NpcActor> allnpcs)
        {
            npcs = allnpcs;
            foreach (var npc in npcs)
                npc.Start();

            updateTimer.Enabled = true;
        }

        public void AddPlayer(PlayerActor player)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
                player.Start();
            }
        }

        public WorldInstance Clone()
        {
            return new WorldInstance() { name = name, npcIds = npcIds };
        }

        void updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            float deltaTime = lastFrameTime.TimeOfDay.Milliseconds - e.SignalTime.TimeOfDay.Milliseconds; // Is this right?

            for (int i = 0; i < players.Count; i++)
                players[i].Update(deltaTime);
            for (int i = 0; i < npcs.Count; i++)
                npcs[i].Update(deltaTime);

            lastFrameTime = e.SignalTime;
        }
    }
}
