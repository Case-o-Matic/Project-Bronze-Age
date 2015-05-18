using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class Item : Unit
    {
        public string name, description;
        public ItemRarity rarity;
        public int goldWorth;
        // Add specific info
    }
    public enum ItemRarity
	{
	    Common = 0,
        Uncommon = 1,
        Rare = 2,
        Precious = 3,
        Immortal = 4,
        Legendary = 5,
        Sacred = 6
	}
}
