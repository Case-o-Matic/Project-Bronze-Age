using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    #region Client
    [Serializable]
    public enum ClientPlayPersonalStatePackageType : byte
    {
        GetTransform = 1,
        GetAttributes = 2,
        GetAbilities = 3,
        GetItems = 4,
        GetQuests = 5,
        GetCharacterLook = 6,

        Login = 7,
        Logout = 8
    }
    [Serializable]
    public enum ClientPlayRequestPackageType : byte
    {
        Position = 1, // Tis aint no command, the client moves itself and says the server so
        Rotation = 2,
        PickupItem = 3,
        Removeitem = 4,
        UseAbility = 5,
    }
    #endregion

    #region Server
    [Serializable]
    public enum ServerPlayStatePackageType : byte
    {
        Move = 1,
        Attributes = 2,
        Abilities = 3,
        Items = 4,
        PickedupItem = 5,
        RemovedItem = 6,
        UsedAbility = 7,

        LoginSuccess = 8,
        LoginFail = 9,
        LogoutApproved = 10
    }
    #endregion
}
