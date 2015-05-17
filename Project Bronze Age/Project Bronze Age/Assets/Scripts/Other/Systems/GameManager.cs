using UnityEngine;
using System.Collections;
using System;
using ProtoBuf;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GamePlayMode playMode;
    public ServerPlayInfo currentServerPlayInfo;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnPlayLocal()
    {
        playMode = GamePlayMode.PlaysLocal;
        currentServerPlayInfo = new ServerPlayInfo("Local", "", 0, 0, new byte[0]);
    }
    public void OnJoinServer(ServerPlayInfo serverplayinfo)
    {
        playMode = GamePlayMode.PlaysOnServer;
        currentServerPlayInfo = serverplayinfo;
    }
    public void OnHostServer(ServerPlayInfo serverplayinfo)
    {
        currentServerPlayInfo = serverplayinfo;
    }
}

[Serializable]
public enum GamePlayMode
{
    NotPlaying,
    PlaysOnServer,
    PlaysLocal
}
[Serializable]
public struct ServerPlayInfo
{
    public string name;
    public string comment;

    public Texture2D loadingImage;

    public ServerPlayInfo(string name, string comment, Texture2D loadingimage)
    {
        this.name = name;
        this.comment = comment;
        this.loadingImage = loadingimage;
    }
    public ServerPlayInfo(string name, string comment, int loadingimagewidth, int loadingimageheight, byte[] loadingimagedata)
    {
        this.name = name;
        this.comment = comment;

        this.loadingImage = new Texture2D(loadingimagewidth, loadingimageheight);
        using (var mStream = new MemoryStream(loadingimagedata))
            this.loadingImage.SetPixels(Serializer.Deserialize<Color[]>(mStream));
    }
}