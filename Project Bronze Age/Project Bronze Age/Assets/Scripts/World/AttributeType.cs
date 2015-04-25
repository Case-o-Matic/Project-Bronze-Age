using System;

[Serializable]
public enum AttributeType
{
    CurrentHealth,
    CurrentStamina,

    BaseMaxHealth,
    BonusMaxHealth,

    BaseHealthRegeneration,
    BonusHealthRegeneration,

    BaseMaxStamina,
    BonusMaxStamina,

    BaseStaminaRegeneration,
    BonusStaminaRegeneration,

    BaseMovementspeed,
    BonusMovementspeed,

    BaseArmor,
    BonusArmor
}