using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    #region Client
    [Serializable]
    public enum ClientDataAuthPackageType : byte
    {
        Login = 1,
        Logout = 2,
        Register = 3,
        Unregister = 4
    }
    [Serializable]
    public enum ClientDataChangePackageType : byte
    {
        GetData = 1,
        CreateChar = 2,
        RemoveChar = 3,
        SetRank = 4
    }
    #endregion

    #region Server
    [Serializable]
    public enum ServerDataAuthPackageType : byte
    {
        LoginValid = 1,
        LoginInvalid = 2,
        Registered = 3,
        Unregistered = 4
    }
    [Serializable]
    public enum ServerDataChangePackageType : byte
    {
        Data = 1,
        ChangeValid = 2,
        ChangeInvalid = 3
    }
    #endregion
}
