using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable()]
public class Inventory
{
    [SerializeField]
    private List<Item> items;

    public void AddItem(string item)
    {
        var newItem = UnityEngine.Object.Instantiate<Item>(Resources.Load<Item>("Assets/Resources/Items/" + item));
        items.Add(newItem);
    }
    public void AddItem(Item item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            // Apply the effects of the item to the owning actor
        }
    }

    // Not needed
    //public void RemoveItem(string name)
    //{

    //}

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            // Unapply the effects of the item from the owning actor
            items.Remove(item);
        }
    }

    public Item FindItem(string name)
    {
        return items.Find((i) => { if (i.itemName == name) return true; else return false; });
    }
}