using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class WorldInstanceResource : IResourceID<WorldInstance>
    {
        public string name;
        public int[] npcIds;
        // Add other actor info (statics/...)
        // Add navmesh info

        public int resourceId
        {
            get;
            set;
        }

        public WorldInstance Create()
        {
            return new WorldInstance() { name = name, npcs = ResourceManager.GetNPCs(npcIds) };
        }
    }
}
