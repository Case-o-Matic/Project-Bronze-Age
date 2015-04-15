using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LiveActor : Actor
{
    public const float baseMaxHealthLevelMultiplier = 4.5f, baseMaxStaminaLevelMultiplier = 2.5f;

    public float baseMaxHealth, baseMaxStamina, baseHealthRegeneration, baseStaminaRegeneration, baseMovementspeed, baseArmor;
    public Dictionary<AttributeType, float> attributes;
    public List<Buff> buffs;
    public Level level;
    public Inventory inventory;
    public List<Ability> abilities;
    public bool isPoisonImmune, isImmortal, isStunned; // TODO: Buff-stacking of the stuns wont work properly, one isStunned change will block out the others, change this!

    private List<Effect> currentEffects;
    private Ability currentAbility;

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

    public void UseAbility(Ability ability, AbilityTarget target)
    {
        if (isStunned || isDead)
        {
            Debug.Log("You cannot use abilities if you are stunned or dead");
            return;
        }

        if (abilities.Contains(ability))
        {
            if (ability.canUseAbility)
            {
                currentAbility = ability;
                StartCoroutine(InvokeAbility(target)); // Play cast time animation while the cast time fades out
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
        base.Awake();
    }
    protected override void Update()
    {
        UpdateAttributes();
        UpdateAbilities();
        UpdateBuffs();

        base.Update();
    }

    private void InitializeAttributes()
    {
        attributes = new Dictionary<AttributeType, float>();
        attributes.Add(AttributeType.CurrentHealth, baseMaxHealth);
        attributes.Add(AttributeType.CurrentStamina, baseMaxStamina);

        attributes.Add(AttributeType.BaseMaxHealth, baseMaxHealth);
        attributes.Add(AttributeType.BonusMaxHealth, 0);

        attributes.Add(AttributeType.BaseHealthRegeneration, baseHealthRegeneration);
        attributes.Add(AttributeType.BonusHealthRegeneration, 0);

        attributes.Add(AttributeType.BaseMaxStamina, baseMaxStamina);
        attributes.Add(AttributeType.BaseMaxStamina, 0);

        attributes.Add(AttributeType.BaseStaminaRegeneration, baseMaxStaminaLevelMultiplier);
        attributes.Add(AttributeType.BonusStaminaRegeneration, 0);

        attributes.Add(AttributeType.BaseArmor, baseArmor);
        attributes.Add(AttributeType.BonusArmor, baseArmor);

        attributes.Add(AttributeType.BaseMovementspeed, baseMovementspeed);
        attributes.Add(AttributeType.BonusMovementspeed, baseMovementspeed);
    }

    private void UpdateAttributes()
    {
        attributes[AttributeType.BaseMaxHealth] = baseMaxHealth + (level.currentLevel * baseMaxHealthLevelMultiplier);
        attributes[AttributeType.BaseMaxStamina] = baseMaxStamina + (level.currentLevel * baseMaxStaminaLevelMultiplier);
    }
    private void UpdateBuffs()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            Buff buff = buffs[i];
            if (buff.hasLivetime && buff.currentLiveTime > 0)
            {
                buff.currentLiveTime -= Time.deltaTime;
                if (buff.currentLiveTime <= 0)
                {
                    buff.currentLiveTime = 0;
                    UnapplyBuff(buff);
                }
            }
        }
    }
    private void UpdateAbilities()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            Ability ability = abilities[i];
            if(ability.currentCooldown > 0)
            {
                ability.currentCooldown -= Time.deltaTime;
                if (ability.currentCooldown <= 0)
                {
                    ability.currentCooldown = 0;
                }
            }
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
            switch (effect.affects)
            {
                case EffectAffection.Attribute:
                    attributes[effect.attribute] += effect.attributeAdded;
                    break;
                case EffectAffection.ReceiveDamage:
                    
                    break;
                case EffectAffection.ApplyBuffs:
                    foreach (var buff in effect.applyBuffs)
                    {
                        ApplyBuff(buff);
                    }
                    break;
                case EffectAffection.UnapplyBuffs:
                    for (int i = 0; i < buffs.Count; i++)
                    {
                        if (buffs[i].type == effect.unapplyBuffsType)
                            UnapplyBuff(buffs[i]);
                    }
                    break;
                case EffectAffection.Stun:
                    Stun(effect.stun);
                    break;
            }
        }
    }
    private void RemoveEffect(Effect effect)
    {
        if(currentEffects.Contains(effect))
        {
            switch (effect.affects)
            {
                case EffectAffection.Attribute:
                    attributes[effect.attribute] -= effect.attributeAdded;
                    break;
            }
        }
    }

    private IEnumerator InvokeAbility(AbilityTarget target)
    {
        yield return new WaitForSeconds(currentAbility.castTime);

        // TODO: Invoke the ability
        Ability a = currentAbility;
        currentAbility.currentCooldown = currentAbility.cooldown;

        // Apply the effects
        switch (a.target)
        {
            case Ability.AbilityTarget.Self:
                
                break;
            case Ability.AbilityTarget.Actor:
                break;
            case Ability.AbilityTarget.Point:
                break;
            default:
                break;
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
    public struct AbilityTarget
    {
        public Vector3 position;
        public LiveActor actor;
    }
}

public enum DamageType
{
    Physical,
    Pure,
    Poison
}