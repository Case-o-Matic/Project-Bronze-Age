using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    [Serializable]
    public enum ClientServerResourcePackageType : byte
    {
        WorldInstances = 0,
        Buffs = 1,
        Abilities = 2,
        Quests = 3,
        NPCs = 4
    }
}
