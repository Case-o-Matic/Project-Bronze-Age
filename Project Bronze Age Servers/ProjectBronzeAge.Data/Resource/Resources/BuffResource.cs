using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class BuffResource : IResourceID<Buff>
    {
        public string name, description;
        public bool isStackable, isUnremovable, isDebuff, hasLivetime;
        public float livetime;

        public int resourceId
        {
            get;
            set;
        }

        public Buff Create()
        {
            return new Buff() { name = name, description = description, livetime = livetime, isDebuff = isDebuff, isStackable = isStackable, isUnremovable = isUnremovable, hasLivetime = hasLivetime };
        }
    }
}
