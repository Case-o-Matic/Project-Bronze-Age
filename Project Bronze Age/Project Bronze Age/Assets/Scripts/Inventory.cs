using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable()]
public class Inventory
{
    private List<Item> items;

    public void AddItem(string name)
    {
    }
    public void AddItem(Item item)
    {
    }

    public void RemoveItem(string name)
    {
    }

    public void RemoveItem(Item item)
    {
    }

    public Item FindItem(string name)
    {
        return items.Find((i) => { if (i.itemName == name) return true; else return false; });
    }
}