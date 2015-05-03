using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System;
using System.IO;

public class ResourceSystem : MonoBehaviour
{
    public static ResourceSystem Instance;
    private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(ResourceData));

    void Awake()
    {
        Instance = this;
    }

    public void ApplyResourceData(LiveActor actor)
    {
        try
        {
            var resourceData = GetResourceData(actor);
            foreach (var resourceDataItem in resourceData.items)
            {
                switch (resourceDataItem.type)
                {
                    case ResouceDataItem.ResourceDataType.Attributes:
                        #region Attributes
                        switch (resourceDataItem.name)
                        {
                            case "BaseMaxHealth":
                                actor.baseMaxHealth = float.Parse(resourceDataItem.data.ToString());
                                break;
                            case "BaseMaxStamina":
                                actor.baseMaxMana = float.Parse(resourceDataItem.data.ToString());
                                break;
                            case "BaseMovementspeed":
                                actor.baseMovementspeed = float.Parse(resourceDataItem.data.ToString());
                                break;
                            case "BaseArmor":
                                actor.baseArmor = float.Parse(resourceDataItem.data.ToString());
                                break;

                            default:
                                Debug.LogWarning("The resource data of the npc \"" + actor.actorName + "\" has an invalid item: " + resourceDataItem.name + ", ignoring...");
                                break;
                        }
                        #endregion
                        break;
                    case ResouceDataItem.ResourceDataType.Items:
                        actor.startItems.Add(resourceDataItem.data.ToString());
                        break;
                    case ResouceDataItem.ResourceDataType.Buffs:
                        actor.startItems.Add(resourceDataItem.data.ToString());
                        break;
                    case ResouceDataItem.ResourceDataType.Abilities:
                        actor.startItems.Add(resourceDataItem.data.ToString());
                        break;
                    default:
                        break;
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("An exception occured while applying the resource data of npc \"" + actor.actorName + "\": " + ex.ToString());
        }
    }

    private ResourceData GetResourceData(LiveActor actor)
    {
        var resfilepath = @"resources\" + ".xml"; // TODO: Use the ActorResourceInfo thats ready to implement in the actor-class
        var xmlReader = XmlReader.Create(resfilepath);

        return (ResourceData)xmlSerializer.Deserialize(xmlReader);
    }
}

public class ResourceData
{
    public List<ResouceDataItem> items;
}
public class ResouceDataItem
{
    public ResourceDataType type;
    public string name;
    public object data;

    public enum ResourceDataType
    {
        Attributes,
        Items,
        Buffs,
        Abilities
    }
}
