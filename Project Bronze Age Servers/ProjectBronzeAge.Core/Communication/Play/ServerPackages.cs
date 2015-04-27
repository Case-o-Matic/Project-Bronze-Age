using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectBronzeAge.Core.Communication.Play
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ServerPlayStatePackage
    {
        public string actorId;
        public ServerPlayStatePackageType type;

        public float posX, posY, posZ;
        public float rotX, rotY, rotZ;

        public string[] abilities, buffs;
        public int[] attributes;

        public ServerPlayStatePackage(string actorid, ServerPlayStatePackageType type, float posx, float posy, float posz, float rotx, float roty, float rotz, string[] abilities, string[] buffs, int[] attributes)
        {
            this.actorId = actorid;
            this.type = type;
            this.posX = posx;
            this.posY = posy;
            this.posZ = posz;
            this.rotX = rotx;
            this.rotY = roty;
            this.rotZ = rotz;
            this.abilities = abilities;
            this.buffs = buffs;
            this.attributes = attributes;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ServerPlayUseAbilityPackage
    {
        public string casterActorId, usedAbility;
        public float targetPosX, targetPosY, targetPosZ;
        public string targetActorId;

        public ServerPlayUseAbilityPackage(string casteractorid, string usedability, float targetposx, float targetposy, float targetposz, string targetactorid)
        {
            this.casterActorId = casteractorid;
            this.usedAbility = usedability;
            this.targetPosX = targetposx;
            this.targetPosY = targetposy;
            this.targetPosZ = targetposz;
            this.targetActorId = targetactorid;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ServerPlayItemStatePackage
    {
        public string affectedActorId, itemName;
        public bool itemAdded;

        public ServerPlayItemStatePackage(string affectedactorid, string itemname, bool itemadded)
        {
            this.affectedActorId = affectedactorid;
            this.itemName = itemname;
            this.itemAdded = itemadded;
        }
    }
}
