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
    public float receiveDamage;
    public DamageType receiveDamageType;

    // ReceiveHeal
    public float receiveHeal;

    // ApplyBuffs/UnapplyBuffs
    public Buff[] applyBuffs;
    public BuffType unapplyBuffsType;

    // Stun
    public bool stun;
    [HideInInspector]
    public bool stunBefore;
}

[Serializable]
public enum EffectAffection
{
    Attribute,
    ReceiveDamage,
    ReceiveHeal,
    ApplyBuffs,
    UnapplyBuffs,
    Stun
}