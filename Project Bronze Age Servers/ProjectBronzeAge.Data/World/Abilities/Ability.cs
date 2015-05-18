using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class Ability : Unit
    {
        public string name, description;
        public float cooldown, castTime, mana;

        public float currentCooldown, currentCastTime;
        public bool canExecute { get { return (currentCastTime == 0 && currentCooldown == 0); } }

        protected AbilityInvokationInfo currentAbilityInvokationInfo { get; private set; }

        public void OnExecute(AbilityInvokationInfo invokationinfo)
        {
            if(canExecute)
            {
                currentCastTime = castTime;
                currentAbilityInvokationInfo = invokationinfo;
            }
        }
        public override void Update(float deltatime)
        {
            if (currentCastTime > 0)
            {
                currentCastTime -= deltatime;
                if (currentCastTime <= 0)
                {
                    currentCastTime = 0;
                    currentCooldown = cooldown;

                    if (currentAbilityInvokationInfo.castInvoker.attributes[AttributeType.CurrentMana] >= mana)
                    {
                        currentCooldown = cooldown;
                        currentAbilityInvokationInfo.castInvoker.attributes[AttributeType.CurrentMana] -= mana; // TODO: Create a method for these things

                        OnInvoke();
                    }
                }
            }

            base.Update(deltatime);
        }

        protected virtual void OnInvoke()
        {
            
        }
    }

    public struct AbilityInvokationInfo
    {
        public LiveActor castInvoker;
        public Vector3 castPosition;
        public LiveActor castTarget;

        public AbilityInvokationInfo(LiveActor castinvoker, Vector3 castposition, LiveActor casttarget)
        {
            castInvoker = castinvoker;
            castPosition = castposition;
            castTarget = casttarget;
        }
    }
}
