using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class ClientPlayPersonalStatePackage
    {
        public string actorId;
        public ClientPlayPersonalStatePackageType type;

        public ClientPlayPersonalStatePackage(string actorid, ClientPlayPersonalStatePackageType type)
        {
            this.actorId = actorid;
            this.type = type;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class ClientPlayRequestPackage
    {
        public string actorId;
        public ClientPlayRequestPackageType type;

        public ClientPlayRequestPackage(string actorid, ClientPlayRequestPackageType type)
        {
            this.actorId = actorid;
            this.type = type;
        }
    }
}
