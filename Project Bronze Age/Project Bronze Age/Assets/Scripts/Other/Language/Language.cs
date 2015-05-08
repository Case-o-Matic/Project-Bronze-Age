using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Language
{
    public string name, nameShortcut;
    public List<LanguageItem> items;

    public string GetLanguageTextByEnglish(string englishtext)
    {
        return items.Find((i) => { if (i.englishText == englishtext) return true; else return false; }).languageText ?? "";
    }
}

[Serializable]
public struct LanguageItem
{
    public string englishText, languageText;
}