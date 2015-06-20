using ProjectBronzeAge.Core.Communication;
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
            if(horizontalMove != 0)
                position = new Vector3(totalMovementspeed / (1000 / 15), position.y, position.z);
            if (vertivalMove != 0)
                position = new Vector3(position.x, totalMovementspeed / (1000 / 15), position.z);
            base.Update(deltatime);
        }

        public void ApplyClientRequest(ClientPlayRequestPackage msg)
        {
            horizontalMove = msg.horizontalMove;
            vertivalMove = msg.verticalMove;
        }
    }
}
