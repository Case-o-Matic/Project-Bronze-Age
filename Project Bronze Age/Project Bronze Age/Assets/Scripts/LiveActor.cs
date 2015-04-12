using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LiveActor : Actor
{
    public const float baseMaxHealthLevelMultiplier = 4.5f, baseMaxStaminaLevelMultiplier = 2.5f;

    public float baseMaxHealth, baseMaxStamina, baseMovementspeed, baseArmor;
    public Dictionary<AttributeType, float> attributes;
    public List<Buff> buffs;
    public Level level;
    public Inventory inventory;
    public List<Ability> abilities;

    public bool isDead { get { return attributes[AttributeType.CurrentHealth] == 0; } }

    public void UseAbility(Ability ability, AbilityTarget target)
    {
        if (abilities.Contains(ability))
        {
            if (ability.canUseAbility)
            {
                ability.currentAbilityCooldown = ability.abilityCooldown;
            }
        }
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
    }
    public void UnapplyBuff(Buff buff)
    {
        if (buffs.Contains(buff))
        {
            buffs.Remove(buff);
        }
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

    protected override void OnSerializeServerStream(BitStream stream)
    {
        // Should maybe the attributes get serialized instead of level?
        //stream.Serialize(ref level);
        // Send the buffs

        base.OnSerializeServerStream(stream);
    }
    protected override void OnSerializeClientStream(BitStream stream)
    {
        //stream.Serialize(ref level);
        // Receive the buffs

        base.OnSerializeClientStream(stream);
    }

    private void InitializeAttributes()
    {
        attributes = new Dictionary<AttributeType, float>();
        attributes.Add(AttributeType.CurrentHealth, baseMaxHealth);
        attributes.Add(AttributeType.CurrentStamina, baseMaxStamina);

        attributes.Add(AttributeType.BaseMaxHealth, baseMaxHealth);
        attributes.Add(AttributeType.BonusMaxHealth, 0);
        attributes.Add(AttributeType.BaseMaxStamina, baseMaxStamina);
        attributes.Add(AttributeType.BaseMaxStamina, 0);

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
            if(ability.currentAbilityCooldown > 0)
            {
                ability.currentAbilityCooldown -= Time.deltaTime;
                if (ability.currentAbilityCooldown <= 0)
                {
                    ability.currentAbilityCooldown = 0;
                }
            }
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