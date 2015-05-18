using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class AbilityResource : IResourceID<Ability>
    {
        public string name, description;
        public float cooldown, castTime, mana;

        public int resourceId
        {
            get;
            set;
        }

        public Ability Create()
        {
            return new Ability() { name = name, description = description, cooldown = cooldown, castTime = castTime, mana = mana };
        }
    }
}
