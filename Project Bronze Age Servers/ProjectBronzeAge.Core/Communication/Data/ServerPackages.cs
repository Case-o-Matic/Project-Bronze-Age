using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ServerDataAuthPackage : IPackage
    {
        public ServerDataAuthPackageType type;
        public string gameDataUserInfo;

        public ServerDataAuthPackage(ServerDataAuthPackageType type, string gamedatauserinfo)
        {
            this.type = type;
            this.gameDataUserInfo = gamedatauserinfo;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ServerDataChangePackage : IPackage
    {
        public string username;
        public ServerDataChangePackageType type;

        public GameDataUserInfo userInfo;

        public ServerDataChangePackage(string username, ServerDataChangePackageType type, GameDataUserInfo userinfo)
        {
            this.username = username;
            this.type = type;
            this.userInfo = userinfo;
        }
    }
}
