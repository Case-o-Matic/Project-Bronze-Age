using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.GamePlayServer.Core
{
    public class Ability
    {
        public string name;
        public float cooldown, castTime;

        public float currentCooldown, currentCastTime;
        public bool canExecute { get { return (currentCastTime == 0 && currentCooldown == 0); } }

        public void OnExecute()
        {
            if(canExecute)
            {
                currentCastTime = castTime;
            }
        }
        public void Update(float deltatime)
        {
            if (currentCastTime > 0)
            {
                currentCastTime -= deltatime;
                if (currentCastTime <= 0)
                {
                    currentCastTime = 0;
                    currentCooldown = cooldown;

                    // Cast
                }
            }
        }
    }
}
