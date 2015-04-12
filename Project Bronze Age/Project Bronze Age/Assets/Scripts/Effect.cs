using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Effect : ScriptableObject
{
    private EffectAffection affects;
    public float damage;
    public string[] buffs;
}