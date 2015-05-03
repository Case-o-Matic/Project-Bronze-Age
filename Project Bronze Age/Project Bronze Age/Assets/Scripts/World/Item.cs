using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable()]
public abstract class Item : ScriptableObject, INetworkID
{
    public string itemName;
    public string itemDescription;
    public int goldWorth;
    public ItemRarity rarity;

    private int _networkId;

    public int networkId
    {
        get { return _networkId; }
    }
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