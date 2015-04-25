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
    public float liveTime, effectInvokationIntervall;
    public bool hasLivetime, isRemovable, isDebuff, hasInvokationIntervall;
    public float currentLiveTime, currentEffectInvokationIntervall;

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