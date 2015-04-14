using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable()]
public class Buff : ScriptableObject
{
    public string buffName;
    public string description;
    public BuffType type;
    public float liveTime;
    public bool hasLivetime, isRemovable, isDebuff;
    public float currentLiveTime;

    public List<Effect> effects;
}

[Serializable]
public enum BuffType
{
    Heal,
    Damage,
    DoT,
    Poison,
    Movement,
    Stun
}