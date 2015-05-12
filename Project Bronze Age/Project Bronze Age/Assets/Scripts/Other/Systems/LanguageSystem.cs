using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ProtoBuf;
using System;

public class LanguageSystem : MonoBehaviour, ILoadable
{
    public const string languagesFolder = "languages", languageFileExtension = ".data";
    public static LanguageSystem Instance;
    private static YamlDotNet.Serialization.Serializer yamlSerializer = new YamlDotNet.Serialization.Serializer();
    private static YamlDotNet.Serialization.Deserializer yamlDeserializer = new YamlDotNet.Serialization.Deserializer();

    public string currentLocationNameShortcut;
    public List<Language> languages;
    public Language currentLanguage;

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
        isLoading = true;
        CreateLanguagesByList();
        LoadAllLanguages();
    }

    public void SetLanguage(string nameshortcut)
    {
        currentLanguage = languages.Find((l) => { if (l.nameShortcut == nameshortcut) return true; else return false; });
        if (currentLanguage == null)
            SetLanguage("EN");

        progress += 50;
        isDone = true;
        isLoading = false;
    }
    public void UpdateLocation()
    {
        currentLocationNameShortcut = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.ToUpper();
    }

    private void LoadAllLanguages()
    {
        var languageFiles = new DirectoryInfo(languagesFolder).GetFiles("*" + languageFileExtension);
        foreach (var languageFile in languageFiles)
        {
            using (var sr = new StreamReader(File.Open(languageFile.FullName, FileMode.OpenOrCreate)))
                languages.Add(yamlDeserializer.Deserialize<Language>(sr));
        }

        progress += 50;
    }
    private void CreateLanguagesByList()
    {
        foreach (var language in languages)
        {
            using (var sw = new StreamWriter(File.Create(languagesFolder + @"\" + language.name + languageFileExtension)))
                yamlSerializer.Serialize(sw, language);
        }
    }

    public static string Get(string text)
    {
        if (Instance.currentLanguage.nameShortcut == "EN")
            return text;
        else return Instance.currentLanguage != null ? Instance.currentLanguage.GetLanguageTextByEnglish(text) : text;
    }
}
