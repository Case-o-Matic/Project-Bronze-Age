using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBronzeAge.Data
{
    public class Buff : Unit, IResourceID<Buff>
    {
        public string name;
        public bool hasLivetime, isUnremovable, isStackable, isDebuff;
        public float livetime;

        public int resourceId
        {
            get;
            set;
        }

        //public virtual void OnApply(LiveActor affected)
        //{
        
        //}
        //public virtual void OnUnapply(LiveActor affected)
        //{
        
        //}

        public Buff Clone()
        {
            return new Buff() { name = name, hasLivetime = hasLivetime, isUnremovable = isUnremovable, isDebuff = isDebuff, livetime = livetime };
        }
    }
}
