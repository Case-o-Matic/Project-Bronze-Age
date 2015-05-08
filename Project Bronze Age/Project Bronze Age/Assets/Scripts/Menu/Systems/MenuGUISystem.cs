using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuGUISystem : MonoBehaviour
{
    public GameObject mainMenuObject, loadingScreenObject;
    public Text gameDataServerStateText, usernameText, languageText, loadingText;
    public Button reconnectButton;
    public Image progressBarValue;

    public AudioSource musicAudioSource;
    public List<ILoadable> loadables;

    private bool isInitializing = true;
    private float currentAbsoluteProgressbarValue;

    void Awake()
    {
        loadables = new List<ILoadable>();
    }

    void Start()
    {
        loadables.Add(UserSystem.Instance);
        loadables.Add(SettingsSystem.Instance);

        AutoselectLanguage();
        progressBarValue.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
    }

    void Update()
    {
        if (UserSystem.Instance.gameDataServerConnectionMode != GameDataServerConnectionMode.StartUp && !musicAudioSource.isPlaying)
            musicAudioSource.Play();
        if (HasFullyLoaded() && isInitializing)
        {
            loadingScreenObject.SetActive(false);
            mainMenuObject.SetActive(true);

            isInitializing = false;
        }
        else if(UserSystem.Instance.loadErrorOccured && isInitializing) // Check all important things here
        {
            loadingText.text = LanguageSystem.Get("Central servers not reachable");
            Debug.LogError("Central servers not reachable");

            isInitializing = false;
        }
        else if(SettingsSystem.Instance.loadErrorOccured && isInitializing)
        {
            loadingText.text = LanguageSystem.Get("Settings couldnt get loaded");
            Debug.LogError("Settings couldnt get loaded");

            isInitializing = false;
        }

        if(isInitializing)
            loadingText.text = LanguageSystem.Get("Loading");

        if (UserSystem.Instance.gameDataServerConnectionMode == GameDataServerConnectionMode.Aborted)
        {
            reconnectButton.gameObject.SetActive(true);
        }
        else
        {
            reconnectButton.gameObject.SetActive(false);
        }

        gameDataServerStateText.text = LanguageSystem.Get(UserSystem.Instance.gameDataServerConnectionMode.ToString());
        usernameText.text = UserSystem.Instance.caseomaticUsername;

        SetAbsoluteProgressbarValue();
    }

    private void SetAbsoluteProgressbarValue()
    {
        float loadableValues = 0;
        var loadingComponents = loadables.FindAll((l1) => { if (l1.isLoading) return true; else return false; });
        loadingComponents.ForEach((l2) => { loadableValues += l2.progress / 100 * loadingComponents.Count; });

        currentAbsoluteProgressbarValue = Mathf.Lerp(currentAbsoluteProgressbarValue, loadableValues, 0.1f);

        progressBarValue.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentAbsoluteProgressbarValue);
    }
    private void AutoselectLanguage()
    {
        LanguageSystem.Instance.UpdateLocation();
        LanguageSystem.Instance.SetLanguage(LanguageSystem.Instance.currentLocationNameShortcut);

        languageText.text = LanguageSystem.Instance.currentLanguage.name + " (" + LanguageSystem.Instance.currentLocationNameShortcut + ")";
    }
    private bool HasFullyLoaded()
    {
        return loadables.Find((l) => { if (!l.isDone) return true; else return false; }) == null ? true : false;
    }
}
