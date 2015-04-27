using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public string abilityDescription;
    public float castTime, cooldown;
    public AbilityTarget target;
    public float currentCooldown;
    public bool isChanneling;

    public bool canUseAbility { get { return currentCooldown == 0; } }

    public virtual void OnUse(LiveActor user)
    {

    }

    [Serializable]
    public enum AbilityTarget
    {
        Self,
        Actor,
        Point
    }
}
