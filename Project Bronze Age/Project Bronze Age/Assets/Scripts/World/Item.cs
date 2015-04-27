using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int goldWorth;

    public ItemRarity rarity;
}

[Serializable]
public enum ItemRarity
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Legendary = 4,
    Immortal = 4
}