using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class LiveActor : Actor
    {
        public string name;
        public Level level;
        public Inventory inventory;

        public Dictionary<AttributeType, float> attributes;
        public List<Ability> abilities;
        public List<Buff> buffs;

        public bool isPoisonImmune, isImmortal, isStunned;
        public bool isDead { get { return attributes[AttributeType.CurrentHealth] <= 0; } }

        public void ReceiveDamage(float damage, DamageType dt, LiveActor dd)
        {
            if (isDead)
                return;
            if(isImmortal)
                return;
            if(dt == DamageType.Poison && isPoisonImmune)
                return;

            attributes[AttributeType.CurrentHealth] -= damage;
            if (attributes[AttributeType.CurrentHealth] <= 0)
                attributes[AttributeType.CurrentHealth] = 0;
        }
        public void Stun(bool value)
        {
            isStunned = value;
        }

        public void ApplyBuff(Buff buff)
        {
            // TODO: Implement this
        }
        public void UnapplyBuff(Buff buff)
        {
            // TODO: Implement this
        }

        public override void Update(float deltatime)
        {
            nextStatePackage.isPoisonImmune = isPoisonImmune;
            nextStatePackage.isImmortal = isImmortal;
            nextStatePackage.isStunned = isStunned;

            base.Update(deltatime);
        }
    }

    public class Level
    {
        public const int neededXpForNextLevelMultiplicator = 8; // Set right value
        public int currentLevel, currentXp, neededXpForNextLevel;
    }
    public class Inventory
    {
        public List<int> itemIds;
        public List<Item> items;

        public int gold;
    }
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
    public enum DamageType
	{
	    Physical,
        Magical,
        Poison
	}
}
