using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBronzeAge.Data
{
    public class Buff : Unit
    {
        public string name, description;
        public bool hasLivetime, isUnremovable, isStackable, isDebuff;

        public float livetime, currentLivetime;

        public virtual void OnApply(LiveActor affected)
        {

        }
        public virtual void OnUnapply(LiveActor affected)
        {

        }
    }
}
