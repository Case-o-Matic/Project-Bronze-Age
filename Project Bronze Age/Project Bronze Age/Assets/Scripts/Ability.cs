using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Ability
{
    public string abilityName;
    public string abilityDescription;
    public float abilityCooldown;

    public bool canUseAbility { get { return currentAbilityCooldown == 0; } }

    public float currentAbilityCooldown;

    public void OnUse()
    {
        currentAbilityCooldown = abilityCooldown;
    }
}