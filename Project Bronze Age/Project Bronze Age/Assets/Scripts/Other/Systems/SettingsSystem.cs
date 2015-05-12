using UnityEngine;
using System.Collections;

using ProtoBuf;
using System.IO;
using System;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

public class SettingsSystem : MonoBehaviour, ILoadable
{
    // TODO: Change the serializer?
    private static YamlDotNet.Serialization.Serializer yamlSerializer = new YamlDotNet.Serialization.Serializer();
    private static YamlDotNet.Serialization.Deserializer yamlDeserializer = new YamlDotNet.Serialization.Deserializer();
    public const string settingsFilePath = @"data\settings.data";

    public static SettingsSystem Instance;
    public Settings currentSettings;

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
        catch (Exception ex)
        {
            Debug.LogException(ex);
            loadErrorOccured = true;
        }
        finally
        {
            isLoading = false;
        }

        SaveSettings();
    }

    public void GetSettings()
    {
        if (!File.Exists(settingsFilePath))
            currentSettings = new Settings();
        else
        {
            using (var sr = new StreamReader(File.Open(settingsFilePath, FileMode.Open)))
                currentSettings = yamlDeserializer.Deserialize<Settings>(sr);
        }        
    }
    public void SaveSettings()
    {
        using (var sw = new StreamWriter(File.Create(settingsFilePath)))
            yamlSerializer.Serialize(sw, currentSettings);
    }

    public void ApplySettings()
    {
        if (currentSettings != null)
        {
            // TODO: Apply all settings
            LanguageSystem.Instance.SetLanguage(currentSettings.general.currentLanguageNameShortcut);
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