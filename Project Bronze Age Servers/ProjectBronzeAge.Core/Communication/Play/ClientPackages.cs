using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    [StructLayout(LayoutKind.Sequential)] // select pack value
    [Serializable]
    public struct ClientPlayRequestPackage : IPackage
    {
        public int actorId;
        public sbyte horizontalMove, verticalMove;
        public int invokeAbilityId,

            pickupItemToInvId,
            removeItemFromInvId,

            acceptQuestId,
        
            interactWithActorId;

        public int invokeAbilityTargetActorId;
        public float invokeAbilityTargetPosX, invokeAbilityTargetPosY, invokeAbilityTargetPosZ;

        public ClientPlayRequestPackage(int actorid, sbyte horizontalmove = 0, sbyte verticalmove = 0, int invokeabilityid = 0, int pickupitemtoinvid = 0, int removeitemfrominvid = 0, int acceptquestid = 0, int interactwithactorid = 0, int invokeabilitytargetactorid = 0, float invokeabilitytargetposx = 0, float invokeabilitytargetposy = 0, float invokeabilitytargetposz = 0)
        {
            this.actorId = actorid;
            this.horizontalMove = horizontalmove;
            this.verticalMove = verticalmove;
            this.invokeAbilityId = invokeabilityid;
            this.pickupItemToInvId = pickupitemtoinvid;
            this.removeItemFromInvId = removeitemfrominvid;
            this.acceptQuestId = acceptquestid;
            this.interactWithActorId = interactwithactorid;

            this.invokeAbilityTargetActorId = invokeabilitytargetactorid;
            this.invokeAbilityTargetPosX = invokeabilitytargetposx;
            this.invokeAbilityTargetPosY = invokeabilitytargetposy;
            this.invokeAbilityTargetPosZ = invokeabilitytargetposz;
        }
    }

    //[StructLayout(LayoutKind.Sequential)]
    //[Serializable]
    //public struct ClientPlayRequestPackage
    //{
    //    public int actorId;
    //    public ClientPlayRequestPackageType type;

    //    public ClientPlayRequestPackage(int actorid, ClientPlayRequestPackageType type)
    //    {
    //        this.actorId = actorid;
    //        this.type = type;
    //    }
    //}
}
