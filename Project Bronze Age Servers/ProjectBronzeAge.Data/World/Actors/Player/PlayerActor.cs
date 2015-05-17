using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class PlayerActor : LiveActor
    {
        public sbyte horizontalMove, vertivalMove;
        public List<Quest> acceptedQuests;

        public override void Update(float deltatime)
        {
            base.Update(deltatime);
        }
    }
}
