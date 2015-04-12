using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    public string abilityDescription;
    public float abilityCooldown;
    public AbilityTarget target;
    public List<Effect> targetEffects;
    public float currentAbilityCooldown;

    public bool canUseAbility { get { return currentAbilityCooldown == 0; } }

    [Serializable]
    public enum AbilityTarget
    {
        Self,
        Actor,
        Point
    }
}
