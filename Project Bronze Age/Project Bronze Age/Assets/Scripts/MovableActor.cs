using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovableActor : Actor
{
    public const float baseMaxHealthLevelMultiplier = 4.5f, baseMaxStaminaLevelMultiplier = 2.5f;

    public float baseMaxHealth, baseMaxStamina, baseMovementspeed, baseArmor;
    public Dictionary<AttributeType, float> attributes;
    public List<Buff> buffs;
    public Level level;
    public Inventory inventory;

    public bool isDead { get { return attributes[AttributeType.CurrentHealth] == 0; } }

    protected override void Start()
    {
        InitializeAttributes();
        base.Start();
    }
    protected override void Update()
    {
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

    }

    public struct Level
    {
        public const int neededXpMultiplicator = 450; // Change this?
        public int currentLevel, currentXp, neededXp;

        public bool AddXp(int value)
        {
            // TODO: Add xp and change the level if needed
            return true; // Did the player level up?
        }
    }
}