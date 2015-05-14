using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LiveActor : Actor
{
    public const float baseMaxHealthLevelMultiplier = 4.5f, baseMaxManaLevelMultiplier = 2.5f, baseHealthRegenerationLevelMultiplier = 2, baseManaRegenerationLevelMultiplier = 2;

    public List<string> startBuffs, startAbilities, startItems;
    public float baseMaxHealth, baseMaxMana, baseHealthRegeneration, baseManaRegeneration, baseMovementspeed, baseArmor;
    public Transform bodyHeadTransform, bodyCenterTransform, bodyLeftHandTransform, bodyRightHandTransform, bodyLeftFootTransform, bodyRightFootTransform;
    public Dictionary<AttributeType, float> attributes;
    public List<Buff> buffs;
    public Level level;
    public Inventory inventory;
    public List<Ability> abilities;

    // Controlled by buffs
    public bool isPoisonImmune, isImmortal, isStunned;

    // public ActorResourceInfo resourceInfo;

    private List<Effect> currentEffects;
    private Ability currentAbility;

    public float totalMaxHealth { get { return attributes[AttributeType.BaseMaxHealth] + attributes[AttributeType.BonusMaxHealth]; } }
    public float totalMovementspeed { get { return attributes[AttributeType.BaseMovementspeed] + attributes[AttributeType.BonusMovementspeed]; } }

    public bool isDead { get { return attributes[AttributeType.CurrentHealth] <= 0; } }

    public void ReceiveDamage(float damage, DamageType type, LiveActor dd)
    {
        if(isImmortal)
        {
            Debug.Log(actorName + " currently is immortal and cant receive damage");
            return;
        }
        if (damage > 0)
        {
            Debug.Log(dd.actorName + " tried to deal " + damage + " damage to " + actorName + ", positive damage is invalid!");
            return;
        }

        float totalDamage = damage; // Change damage according to type
        switch (type)
        {
            case DamageType.Physical:
                // Special formula
                break;
            case DamageType.Pure:
                break;
            case DamageType.Poison:
                if (isPoisonImmune)
                    totalDamage = 0;
                break;
            default:
                break;
        }

        attributes[AttributeType.CurrentHealth] -= totalDamage;
        if(isDead)
        {
            attributes[AttributeType.CurrentHealth] = 0;
            OnDeath();
        }
    }
    public void ReceiveHeal(float amount, LiveActor healer)
    {
        if(amount < 0)
        {
            Debug.Log(healer.actorName + " tried to heal " + actorName + " by " + amount + " points, negative heals are invalid!");
            return;
        }

        attributes[AttributeType.CurrentHealth] += amount;
    }

    public void UseAbility(int abilityid, Vector3 targetpos, LiveActor targetactor)
    {
        if (isStunned || isDead)
        {
            Debug.Log("You cannot use abilities if you are stunned or dead");
            return;
        }
        if(currentAbility != null)
        {
            Debug.Log("Already casting a different ability: " + currentAbility.abilityName);
        }

        var ability = abilities[abilityid]; // TODO: Use a real ID-system for abilities
        if (abilities.Contains(ability))
        {
            if (ability.canUseAbility && attributes[AttributeType.CurrentMana] >= ability.manaCost)
            {
                currentAbility = ability;
                currentAbility.Invoke(new AbilityInvokation(targetpos, targetactor, this));
            }
            else
            {
                Debug.Log("You cannot use this ability (reason: cooldown/mana)");
            }
        }
    }
    public void AbortAbility()
    {
        if (currentAbility == null)
            return;
        if (IsInvoking("InvokeAbility"))
            CancelInvoke("InvokeAbility");
    }

    public void AddAbility(string ability)
    {
        var newAbility = Instantiate<Ability>(Resources.Load<Ability>("Assets/Resources/Abilities/" + ability));
        abilities.Add(newAbility);
    }
    public void RemoveAbility(Ability ability)
    {
        if (abilities.Contains(ability))
            abilities.Remove(ability);
    }

    public void ApplyBuff(string buff)
    {
        var newBuff = Instantiate<Buff>(Resources.Load<Buff>("Assets/Resources/Buffs/" + buff));
        buffs.Add(newBuff);

        foreach (var effect in newBuff.effects)
        {
            AddEffect(effect);
        }
    }
    public void ApplyBuff(Buff buff)
    {
        var newBuff = Instantiate<Buff>(buff);
        buffs.Add(newBuff);

        foreach (var effect in newBuff.effects)
        {
            AddEffect(effect);
        }
    }
    public void UnapplyBuff(Buff buff)
    {
        if (buffs.Contains(buff) && buff.isRemovable)
        {
            foreach (var effect in buff.effects)
            {
                RemoveEffect(effect);
            }
            buffs.Remove(buff);
        }
    }

    public virtual void Stun(bool value)
    {
        isStunned = value;
        AbortAbility(); // Automatically abort current ability
    }

    protected override void Awake()
    {
        InitializeAttributes();
        //ResourceSystem.Instance.ApplyResourceData(this);
        currentEffects = new List<Effect>();

        base.Awake();
    }
    protected override void Update()
    {
        // Send a position/rotation serverstate somewhere (using the network sendrate)

        UpdateAttributes();
        UpdateAbilities();
        UpdateBuffs();

        base.Update();
    }

    protected override void OnReceiveClientRequest(ClientRequest rq)
    {
        if(rq.invokeAbilityId != 0)
        {
            // Check and use the ability
            SendServerEvent(new ServerEvent(invokeabilityid: rq.invokeAbilityId, invokeabilitytargetactorid: rq.invokeAbilityTargetActorId, invokeabilitytargetpos: rq.invokeAbilityTargetPos));
        }
        if(rq.pickupItemToInvId != 0)
        {
            // Check and pickup item
            SendServerEvent(new ServerEvent(pickupitemtoinvid: rq.pickupItemToInvId));
        }
        if(rq.removeItemFromInvId != 0)
        {
            // Check and remove item
            SendServerEvent(new ServerEvent(removeitemfrominvid: rq.removeItemFromInvId));
        }
        if(rq.interactWithActorId != 0)
        {
            // Check and interact with actor
            SendServerEvent(new ServerEvent(interactwithactorid: rq.interactWithActorId));
        }

        base.OnReceiveClientRequest(rq);
    }
    protected override void OnApplyServerEvent(ServerEvent ev)
    {
        if(ev.receiveDamageAmount != 0)
        {
            // Receive damage
        }
        if(ev.invokeAbilityId != 0)
        {
            // Find ability and invoke it with the given event parameters
        }
        if(ev.interactWithActorId != 0)
        {
            // Interact with the actor
        }
        if(ev.pickupItemToInvId != 0)
        {
            // Pickup the item from the ground
        }
        if (ev.removeItemFromInvId != 0)
        {
            // Remove the item from the inventory
        }

        base.OnApplyServerEvent(ev);
    }
    protected override void OnApplyServerState(ServerState state, float timestamp)
    {
        if (state.attributes != null)
        {
            for (int i = 0; i < attributes.Count; i++)
            {

            }
        }
        if(state.currentGold != 0)
            inventory.gold = state.currentGold;
        if (state.currentLevel != 0)
            level.currentLevel = state.currentLevel;
        if (state.currentXp != 0)
            level.currentXp = state.currentXp;

        base.OnApplyServerState(state, timestamp);
    }

    private void InitializeStartValues()
    {
        foreach (var startBuff in startBuffs)
        {
            ApplyBuff(startBuff);
        }
        foreach (var startAbility in startAbilities)
        {
            AddAbility(startAbility);
        }
        foreach (var startItem in startItems)
        {
            inventory.AddItem(startItem);
        }
    }
    private void InitializeAttributes()
    {
        attributes = new Dictionary<AttributeType, float>();
        attributes.Add(AttributeType.CurrentHealth, baseMaxHealth);
        attributes.Add(AttributeType.CurrentMana, baseMaxMana);

        attributes.Add(AttributeType.BaseMaxHealth, baseMaxHealth);
        attributes.Add(AttributeType.BonusMaxHealth, 0);

        attributes.Add(AttributeType.BaseHealthRegeneration, baseHealthRegeneration);
        attributes.Add(AttributeType.BonusHealthRegeneration, 0);

        attributes.Add(AttributeType.BaseMaxMana, baseMaxMana);
        attributes.Add(AttributeType.BonusMaxMana, 0);

        attributes.Add(AttributeType.BaseManaRegeneration, baseMaxManaLevelMultiplier);
        attributes.Add(AttributeType.BonusManaRegeneration, 0);

        attributes.Add(AttributeType.BaseArmor, baseArmor);
        attributes.Add(AttributeType.BonusArmor, baseArmor);

        attributes.Add(AttributeType.BaseMovementspeed, baseMovementspeed);
        attributes.Add(AttributeType.BonusMovementspeed, baseMovementspeed);
    }
    private void UpdateAttributes()
    {
        attributes[AttributeType.BaseMaxHealth] = baseMaxHealth + (level.currentLevel * baseMaxHealthLevelMultiplier);
        attributes[AttributeType.BaseMaxMana] = baseMaxMana + (level.currentLevel * baseMaxManaLevelMultiplier);
    }
    private void UpdateBuffs()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].Update();
            if (buffs[i].currentLiveTime <= 0)
                UnapplyBuff(buffs[i]);
        }
    }
    private void UpdateAbilities()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].Update();
        }
    }

    private void OnDeath()
    {
        if(isDead)
        {
            // What happens here?
        }
    }

    private void AddEffect(Effect effect)
    {
        if(!currentEffects.Contains(effect))
        {
            currentEffects.Add(effect);

            GameObject effectObj1 = Instantiate<GameObject>(effect.gameObject);
            GameObject effectObj2 = null;
            if (effect.position != EffectPosition.Center || effect.position != EffectPosition.Head)
                effectObj2 = Instantiate<GameObject>(effect.gameObject);

            Transform effectObj1Parent = null, effectObj2Parent = null;
            switch (effect.position)
            {
                case EffectPosition.Head:
                    effectObj1Parent = bodyHeadTransform;
                    break;
                case EffectPosition.Center:
                    effectObj1Parent = bodyCenterTransform;
                    break;
                case EffectPosition.Hands:
                    effectObj1Parent = bodyLeftFootTransform;
                    effectObj2Parent = bodyRightFootTransform;
                    break;
                case EffectPosition.Foots:
                    effectObj1Parent = bodyLeftHandTransform;
                    effectObj2Parent = bodyRightHandTransform;
                    break;
            }

            effectObj1.transform.parent = effectObj1Parent;
            effectObj1.transform.localPosition = Vector3.zero;
            effectObj1.transform.localRotation = Quaternion.identity;

            if (effectObj2 != null)
            {
                effectObj2.transform.parent = effectObj2Parent;
                effectObj1.transform.localPosition = Vector3.zero;
                effectObj1.transform.localRotation = Quaternion.identity;
            }

            effect.instantiatedEffectObject1 = effectObj1;
            effect.instantiatedEffectObject2 = effectObj2;
        }
    }
    private void RemoveEffect(Effect effect)
    {
        if(currentEffects.Contains(effect))
        {
            Destroy(effect.instantiatedEffectObject1);
            if (effect.instantiatedEffectObject2 != null)
                Destroy(effect.instantiatedEffectObject2);

            currentEffects.Remove(effect);
        }
    }

    [Serializable]
    public class Level
    {
        public const int neededXpMultiplicator = 450; // Change this?
        public int currentLevel, currentXp, neededXp;

        public bool AddXp(int value)
        {
            if(currentXp + value >= neededXp)
            {
                currentLevel += 1;
                currentXp = 0;
                neededXp = currentLevel * neededXpMultiplicator;

                return true;
            }
            else
            {
                currentXp += value;
                return false;
            }
        }
    }
    [Serializable]
    public struct ActorResourceInfo
    {
        public string resourceName;
        public bool applyResource;
    }
}

[Serializable]
public enum DamageType : byte
{
    Physical = 0x01,
    Pure = 0x02,
    Poison = 0x03
}