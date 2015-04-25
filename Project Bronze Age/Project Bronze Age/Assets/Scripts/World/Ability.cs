using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    public string abilityDescription;
    public float castTime, cooldown;
    public AbilityTarget target;
    public string appliedBuffOnInvoke;
    public float targetPointRange;
    public float currentCooldown;

    public bool canUseAbility { get { return currentCooldown == 0; } }

    [Serializable]
    public enum AbilityTarget
    {
        Self,
        Actor,
        Point
    }
}
