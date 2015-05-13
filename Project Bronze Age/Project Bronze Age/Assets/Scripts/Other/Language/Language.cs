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
        var text = items.Find((i) => { if (i.englishText == englishtext) return true; else return false; }).languageText;
        if (text == null)
        {
            Debug.Log("The translation text for \"" + englishtext + "\" in " + nameShortcut + " language is nonexistant.");
            return englishtext;
        }
        else return text;
    }
}

[Serializable]
public struct LanguageItem
{
    public string englishText, languageText;
}