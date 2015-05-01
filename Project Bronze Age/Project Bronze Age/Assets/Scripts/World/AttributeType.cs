using System;

[Serializable]
public enum AttributeType
{
    CurrentHealth,
    CurrentMana,

    BaseMaxHealth,
    BonusMaxHealth,

    BaseHealthRegeneration,
    BonusHealthRegeneration,

    BaseMaxMana,
    BonusMaxMana,

    BaseManaRegeneration,
    BonusManaRegeneration,

    BaseMovementspeed,
    BonusMovementspeed,

    BaseArmor,
    BonusArmor
}