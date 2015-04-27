using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ClientDataAuthPackage : IPackage
    {
        public string username;
        public ClientDataAuthPackageType type;

        public string logoutPlayedCharName;
        public string logoutPlayedCharNewInfo;

        public ClientDataAuthPackage(string username, ClientDataAuthPackageType type, string logoutplayedcharname, string logoutplayedcharnewinfo)
        {
            this.username = username;
            this.type = type;
            this.logoutPlayedCharName = logoutplayedcharname;
            this.logoutPlayedCharNewInfo = logoutplayedcharnewinfo;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ClientDataChangePackage : IPackage
    {
        public string username;
        public ClientDataChangePackageType type;

        public string charName;
        public GameDataUserInfo.GameDataUserAccountInfo.GameUserAccountInfoRankType newRank;

        public ClientDataChangePackage(string username, ClientDataChangePackageType type, string charname, GameDataUserInfo.GameDataUserAccountInfo.GameUserAccountInfoRankType newrank)
        {
            this.username = username;
            this.type = type;
            this.charName = charname;
            this.newRank = newrank;
        }
    }
}
