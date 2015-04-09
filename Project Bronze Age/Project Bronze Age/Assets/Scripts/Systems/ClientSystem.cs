using UnityEngine;
using System.Collections;
using ProjectBronzeAge.Central;
using System.IO;

public class ClientSystem : MonoBehaviour
{
    public const string currentLoginInfoSessionFilePath = "current_logininfo_session";

    private LoginInfo currentLoginInfo;
    private GameInfo currentGameInfo;

    void Awake()
    {
        GetInfos();
    }

    private void GetInfos()
    {
        currentLoginInfo = Serializer.DeserializeAndDecryptString<LoginInfo>(File.ReadAllText(currentLoginInfoSessionFilePath));
        // TODO: Get game info
    }
}
