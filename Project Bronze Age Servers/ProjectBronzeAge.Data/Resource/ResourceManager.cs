using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public static class ResourceManager
    {
        private static List<WorldInstanceResource> worldInstances = new List<WorldInstanceResource>();
        private static List<NPCActorResource> npcs = new List<NPCActorResource>();
        private static List<AbilityResource> abilities = new List<AbilityResource>();
        private static List<BuffResource> buffs = new List<BuffResource>();
        private static List<ItemResource> items = new List<ItemResource>();
        private static List<QuestResource> quests = new List<QuestResource>();

        public static WorldInstance GetWorldInstance(int id)
        {
            return worldInstances.Find((w) => { if (w.resourceId == id) return true; else return false; }).Create();
        }
        public static NPCActor GetNPC(int resourceid)
        {
            return npcs.Find((n) => { if (n.resourceId == resourceid) return true; else return false; }).Create();
        }
        public static Ability GetAbility(int id)
        {
            return abilities.Find((a) => { if (a.resourceId == id) return true; else return false; }).Create();
        }
        public static Buff GetBuff(int id)
        {
            return buffs.Find((b) => { if (b.resourceId == id) return true; else return false; }).Create();
        }
        public static Item GetItem(int id)
        {
            return items.Find((i) => { if (i.resourceId == id) return true; else return false; }).Create();
        }
        public static Quest GetQuest(int id)
        {
            return quests.Find((q) => { if (q.resourceId == id) return true; else return false; }).Create();
        }

        public static List<NPCActor> GetNPCs(int[] npcids)
        {
            var newNPCs = new List<NPCActor>();
            foreach (var npcid in npcids)
                newNPCs.Add(GetNPC(npcid));
            return newNPCs;
        }
        public static List<Ability> GetAbilities(int[] abilityids)
        {
            var newAbilities = new List<Ability>();
            foreach (var abilityid in abilityids)
                newAbilities.Add(GetAbility(abilityid));
            return newAbilities;
        }
        public static List<Buff> GetBuffs(int[] buffids)
        {
            var newBuffs = new List<Buff>();
            foreach (var buffid in buffids)
                newBuffs.Add(GetBuff(buffid));
            return newBuffs;
        }
        public static List<Quest> GetQuests(int[] questids)
        {
            var newQuests = new List<Quest>();
            foreach (var questid in questids)
                newQuests.Add(GetQuest(questid));
            return newQuests;
        }
        public static List<Item> GetItems(int[] itemids)
        {
            var newItems = new List<Item>();
            foreach (var itemid in itemids)
                newItems.Add(GetItem(itemid));
            return newItems;
        }
    }
}
