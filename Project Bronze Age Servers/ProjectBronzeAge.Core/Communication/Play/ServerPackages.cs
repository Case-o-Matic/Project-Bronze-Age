using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectBronzeAge.Core.Communication.Play
{
    [StructLayout(LayoutKind.Sequential)] // Find out pack value
    [Serializable]
    public struct ServerPlayStatePackage : IPackage
    {
        public int actorId;
        public float posX, posY, posZ;
        public float rotX, rotY, rotZ;

        public bool isPoisonImmune, isImmortal, isStunned;
        public float[] attributes;

        public ServerPlayStatePackage(int actorid, float posx, float posy, float posz, float rotx, float roty, float rotz, bool ispoisonimmune, bool isimmortal, bool isstunned, float[] attributes)
        {
            this.actorId = actorid;
            this.posX = posx;
            this.posY = posy;
            this.posZ = posz;
            this.rotX = rotx;
            this.rotY = roty;
            this.rotZ = rotz;
            this.isPoisonImmune = ispoisonimmune;
            this.isImmortal = isimmortal;
            this.isStunned = isstunned;
            this.attributes = attributes;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ServerPlayEventPackage : IPackage
    {
        public int actorId;

        public int acceptQuestId, finishQuestId;
        public int pickUpItemId, removeItemId;

        public int usedAbilityId;
        public float abilityTargetPosX, abilityTargetPosY, abilityTargetPosZ;
        public int abilityTargetActorId;

        public ServerPlayEventPackage(int actorid, int acceptquestid = 0, int finishquestid = 0, int pickupitemid = 0, int removeitemid = 0, int usedabilityid = 0, float abilitytargetposx = 0, float abilitytargetposy = 0, float abilitytargetposz = 0, int abilitytargetactorid = 0)
        {
            this.actorId = actorid;
            this.acceptQuestId = acceptquestid;
            this.finishQuestId = finishquestid;
            this.pickUpItemId = pickupitemid;
            this.removeItemId = removeitemid;
            this.usedAbilityId = usedabilityid;
            this.abilityTargetPosX = abilitytargetposx;
            this.abilityTargetPosY = abilitytargetposy;
            this.abilityTargetPosZ = abilitytargetposz;
            this.abilityTargetActorId = abilitytargetactorid;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ServerWorldSnapshotPackage : IPackage
    {
        public ActorInfo[] players;
        public ActorInfo[] npcs;

        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct ActorInfo
        {
            public int actorResourceId, actorNetworkId;
            public float posX, posY, posZ, rotX, rotY, rotZ;

            public ActorInfo(int actorresourceid, int actornetworkid, float posx, float posy, float posz, float rotx, float roty, float rotz)
            {
                this.actorResourceId = actorresourceid;
                this.actorNetworkId = actornetworkid;
                this.posX = posx;
                this.posY = posy;
                this.posZ = posz;
                this.rotX = rotx;
                this.rotY = roty;
                this.rotZ = rotz;
            }
        }
    }
}
