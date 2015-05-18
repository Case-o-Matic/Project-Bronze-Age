using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ClientResourceRequestPackage : IPackage
    {
        public ClientServerResourcePackageType type;
        public int[] resourceIds;

        public ClientResourceRequestPackage(ClientServerResourcePackageType type, int[] resourceids = null)
        {
            this.type = type;
            resourceIds = resourceids;
        }
    }
}
