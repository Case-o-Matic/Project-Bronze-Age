using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public string abilityDescription;
    public float castTime, cooldown, manaCost;
    public AbilityTarget target;

    
    public float currentCastTime, currentCooldown;
    public bool isInvoking { get; private set; }
    public AbilityInvokation currentInvokationProperties { get; private set; }

    public bool canUseAbility { get { return currentCooldown == 0; } }

    public void Update()
    {
        if(currentCastTime > 0)
        {
            currentCastTime -= Time.deltaTime;
            if (currentCastTime <= 0 && isInvoking)
            {
                currentCastTime = 0;
                Use();
            }
            else
                return; // Is this really needed? One more if-statement needs to get checked otherwise
        }

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                currentCooldown = 0;
            }
        }
    }

    public void Invoke(AbilityInvokation invokation)
    {
        currentCastTime = castTime;
        currentCooldown = cooldown;
        currentInvokationProperties = invokation;
        isInvoking = true;
    }
    public void CancelInvoke()
    {
        if (isInvoking)
        {
            isInvoking = false;
            currentCastTime = 0;
        }
    }

    public void Use()
    {
        if (isInvoking)
        {
            if (currentInvokationProperties.invoker.attributes[AttributeType.CurrentMana] >= manaCost)
            {
                OnUse();
                currentInvokationProperties.invoker.attributes[AttributeType.CurrentMana] -= manaCost;
            }
            else
            {
                Debug.Log("Using the ability " + abilityName + " failed because you have not enough mana");
            }

            isInvoking = false;
        }
        else
            Debug.Log("You cant use an ability thats not invoked");
    }

    protected void OnUse()
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
[Serializable]
public struct AbilityInvokation
{
    public Vector3 targetPosition;
    public LiveActor targetActor;

    public LiveActor invoker;

    public AbilityInvokation(Vector3 targetposition, LiveActor targetactor, LiveActor invoker)
    {
        this.targetPosition = targetposition;
        this.targetActor = targetactor;
        this.invoker = invoker;
    }
}
