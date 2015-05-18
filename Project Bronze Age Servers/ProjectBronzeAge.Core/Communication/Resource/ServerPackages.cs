using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ServerResourceAnswerPackage : IPackage
    {
        public ClientServerResourcePackageType type;
        public byte[] rowData;

        public ServerResourceAnswerPackage(ClientServerResourcePackageType type, byte[] rowdata)
        {
            this.type = type;
            rowData = rowdata;
        }
    }
}
