using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class ItemResource : IResourceID<Item>
    {
        public string name, description;
        public ItemRarity rarity;
        public int goldWorth;

        public int resourceId
        {
            get;
            set;
        }

        public Item Create()
        {
            return new Item() { name = name, description = description, rarity = rarity, goldWorth = goldWorth };
        }
    }
}
