using UnityEngine;
using System.Collections;

using ProtoBuf;
using System.IO;
using System;

public class SettingsSystem : MonoBehaviour, ILoadable
{
    public const string settingsFilePath = @"data\settings.data";

    public static SettingsSystem Instance;
    public Settings settings;

    public float progress
    {
        get;
        private set;
    }
    public bool isLoading
    {
        get;
        private set;
    }
    public bool isDone
    {
        get;
        private set;
    }
    public bool loadErrorOccured
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        try
        {
            isLoading = true;
            GetSettings();

            progress += 100;
            isDone = true;
        }
        catch (Exception)
        {
            loadErrorOccured = true;
        }
        finally
        {
            isLoading = false;
        }
    }

    public void GetSettings()
    {
        if (!File.Exists(settingsFilePath))
            settings = new Settings();
        else
        {
            using (var fs = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read))
                settings = Serializer.Deserialize<Settings>(fs);
        }
    }
    public void SaveSettings()
    {
        using (var fs = new FileStream(settingsFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            Serializer.Serialize<Settings>(fs, settings);
    }

    public void ApplySettings()
    {
        if (settings != null)
        {
            // TODO: Apply all settings
            LanguageSystem.Instance.SetLanguage(settings.general.currentLanguageNameShortcut);
        }
    }
}

[Serializable]
public class Settings
{
    public GeneralSettings general;
    public AudioSettings audio;
    public GraphicsSettings graphics;
    public NetworkSettings network;

    public Settings()
    {
        general = new GeneralSettings() { currentLanguageNameShortcut = "EN" };
        audio = new AudioSettings() { dialogVolume = 1, environmentVolume = 1, generalVolume = 1, musicVolume = 1 };
        graphics = new GraphicsSettings() { graphicsQuality = 2, screenResolution = GraphicsSettings.GraphicsSettingsScreenResolution.r1270x768 };
        network = new NetworkSettings() { syncQuality = NetworkSettings.NetworkSettingsSyncQuality.Good };
    }

    [Serializable]
    public struct GeneralSettings
    {
        public string currentLanguageNameShortcut;
    }
    [Serializable]
    public struct AudioSettings
    {
        public float dialogVolume, musicVolume, environmentVolume, generalVolume;
    }
    [Serializable]
    public struct GraphicsSettings
    {
        public int graphicsQuality;
        public GraphicsSettingsScreenResolution screenResolution;

        [Serializable]
        public enum GraphicsSettingsScreenResolution
        {
            r600x800,
            r1270x768,
            r1600x900,
            r1920x1080
        }
    }
    [Serializable]
    public struct NetworkSettings
    {
        public NetworkSettingsSyncQuality syncQuality;

        [Serializable]
        public enum NetworkSettingsSyncQuality
        {
            Poor,
            Good
        }
    }
}