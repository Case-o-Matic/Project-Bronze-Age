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
        public Inventory inventory;

        public Dictionary<AttributeType, float> attributes;
        public List<Ability> abilities;
        public List<Buff> buffs;
        public Level level;

        public bool isPoisonImmune, isImmortal, isStunned;
        public bool isDead { get { return attributes[AttributeType.CurrentHealth] <= 0; } }

        public float totalMovementspeed { get { return attributes[AttributeType.BaseMovementspeed] + attributes[AttributeType.BonusMovementspeed]; } }

        public void UseAbility(Ability ability, AbilityInvokationInfo invokationinfo)
        {
            // Implement this
        }
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
            if (buffs.Contains(buff) && !buff.isStackable)
                return;
            else
            {
                buff.OnApply(this);
                buffs.Add(buff);
            }
        }
        public void UnapplyBuff(Buff buff, bool force)
        {
            if(buffs.Contains(buff))
            {
                if ((buff.isUnremovable && force) || !buff.isUnremovable)
                {
                    buff.OnUnapply(this);
                    buffs.Remove(buff);
                }
            }
        }

        public override void Update(float deltatime)
        {
            nextStatePackage.currentLevel = level.currentLevel;
            nextStatePackage.currentXp = level.currentXp;

            nextStatePackage.isPoisonImmune = isPoisonImmune;
            nextStatePackage.isImmortal = isImmortal;
            nextStatePackage.isStunned = isStunned;

            UpdateBuffs(deltatime);
            UpdateAbilities(deltatime);

            base.Update(deltatime);
        }

        private void UpdateBuffs(float deltatime)
        {
            for (int i = 0; i < buffs.Count; i++)
                buffs[i].Update(deltatime);
        }
        private void UpdateAbilities(float deltatime)
        {
            for (int i = 0; i < abilities.Count; i++)
                abilities[i].Update(deltatime);
        }

        private void InitializeAttributes()
        {
            attributes = new Dictionary<AttributeType, float>();
            // TODO: Add attributes
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
