using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable()]
public abstract class Quest : ScriptableObject, INetworkID, ICloneable
{
    public int questName, questDescription, acceptedQuestOwnerDialogText, neededLevel;

    public int rewardGold, rewardXp;
    public string[] rewardItems;

    public QuestVariation variation;
    public QuestType type;

    private int _networkId;

    public int networkId
    {
        get
        {
            return _networkId;
        }
    }

    public virtual bool CheckQuestCompleted(PlayerActor actor)
    {
        return false;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

[Serializable]
public enum QuestVariation
{
    CollectItem,
    Kill,
    Deliver
    /*
    Escort,
    Profession,
    ...
    */
}
[Serializable]
public enum QuestType
{
    Normal,
    Time,
    Daily,
    Seasonal
}