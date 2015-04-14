using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Effect
{
    public EffectAffection affects;
    public GameObject gameObject;

    // Attribute
    public AttributeType attribute;
    public float attributeAdded;
    
    // ReceiveDamage
    public float receiveDamage, receiveDamageIntervall, receiveDamageIntervallTime;
    public DamageType receiveDamageType;

    // ApplyBuffs/UnapplyBuffs
    public string[] applyBuffs;
    public BuffType unapplyBuffsType;

    // Stun
    public bool stun;
}

[Serializable]
public enum EffectAffection
{
    Attribute,
    ReceiveDamage,
    ApplyBuffs,
    UnapplyBuffs,
    Stun
}