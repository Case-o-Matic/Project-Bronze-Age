using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public static class ResourceManager
    {
        private static List<WorldInstance> worldInstances = new List<WorldInstance>();
        private static List<NpcActor> npcs = new List<NpcActor>();
        private static List<Ability> abilities = new List<Ability>();
        private static List<Buff> buffs = new List<Buff>();
        private static List<Item> items = new List<Item>();
        private static List<Quest> quests = new List<Quest>();

        public static WorldInstance GetWorldInstance(int id)
        {
            return worldInstances.Find((w) => { if (w.resourceId == id) return true; else return false; }).Clone();
        }
        public static NpcActor GetNPC(int resourceid)
        {
            return npcs.Find((n) => { if (n.resourceId == resourceid) return true; else return false; }).Clone();
        }
        public static Ability GetAbility(int id)
        {
            return abilities.Find((a) => { if (a.networkId == id) return true; else return false; }).Clone();
        }
        public static Buff GetBuff(int id)
        {
            return buffs.Find((b) => { if (b.resourceId == id) return true; else return false; }).Clone();
        }
        public static Item GetItem(int id)
        {
            return items.Find((i) => { if (i.resourceId == id) return true; else return false; }).Clone();
        }
        public static Quest GetQuest(int id)
        {
            return quests.Find((q) => { if (q.resourceId == id) return true; else return false; }).Clone();
        }
    }
}
