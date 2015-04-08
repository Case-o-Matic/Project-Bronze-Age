using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MovableActor : Actor
{
    public float baseMaxHealth, baseMaxStamina, baseMovementspeed, baseArmor;
    public Dictionary<AttributeType, float> attributes;
    private bool isDead;
    private bool isOnHorse;
    private Inventory inventory;

    protected override void Start()
    {
        Initialize();
        base.Start();
    }

    private void Initialize()
    {
        attributes = new Dictionary<AttributeType, float>();
        attributes.Add(AttributeType.CurrentHealth, baseMaxHealth);
        attributes.Add(AttributeType.CurrentStamina, baseMaxStamina);

        attributes.Add(AttributeType.MaxHealth, baseMaxHealth);
        attributes.Add(AttributeType.MaxStamina, baseMaxStamina);

        attributes.Add(AttributeType.BaseArmor, baseArmor);
        attributes.Add(AttributeType.BonusArmor, baseArmor);

        attributes.Add(AttributeType.BaseMovementspeed, baseMovementspeed);
        attributes.Add(AttributeType.BonusMovementspeed, baseMovementspeed);
    }
}