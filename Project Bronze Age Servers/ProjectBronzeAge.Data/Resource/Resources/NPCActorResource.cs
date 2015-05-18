using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class NPCActorResource : IResourceID<NPCActor>
    {
        public string name, dialogText;
        public Vector3 position, rotation;
        public int currentLevel, currentXp;
        public int gold;
        public int[] abilityIds, itemIds, questIds;

        public int resourceId
        {
            get;
            set;
        }

        public NPCActor Create()
        {
            return new NPCActor() { name = name, position = position, rotation = rotation, dialogText = dialogText, abilities = ResourceManager.GetAbilities(abilityIds), quests = ResourceManager.GetQuests(itemIds), inventory = new Inventory() { items = ResourceManager.GetItems(itemIds), gold = gold } };
        }
    }
}
