using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class NpcActor : LiveActor, IResourceID<NpcActor>
    {
        public string dialogText;
        public List<int> abilityIds, questIds;
        public List<Quest> quests;
        // Add Navmesh agent info

        public int resourceId
        {
            get;
            set;
        }

        public override void Start()
        {
            abilityIds.ForEach((aid) => { abilities.Add(ResourceManager.GetAbility(aid)); });
            base.Start();
        }

        public NpcActor Clone()
        {
            var newAbilities = new List<Ability>();
            abilityIds.ForEach((aid) => { newAbilities.Add(ResourceManager.GetAbility(aid)); });

            var newQuests = new List<Quest>();
            questIds.ForEach((qid) => { newQuests.Add(ResourceManager.GetQuest(qid)); });

            var newItems = new List<Item>();
            inventory.itemIds.ForEach((iid) => { newItems.Add(ResourceManager.GetItem(iid)); });

            return new NpcActor() { name = name, dialogText = dialogText, abilities = newAbilities, quests = newQuests, inventory = new Inventory() { gold = inventory.gold, items = newItems } };
        }
    }
}
