using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuGUISystem : MonoBehaviour
{
    public Text gameDataServerStateText, usernameText;
    public Image progressBarValue;

    public UserSystem userSystem;
    public AudioSource musicAudioSource;

    private float currentProgressBarValue;

    public void SetLoadingProgress(float value)
    {
        if (value <= 100 && value >= 0)
            currentProgressBarValue = Mathf.Lerp(currentProgressBarValue, value, 0.1f);
        else currentProgressBarValue = 0;
    }

    void Start()
    {
        progressBarValue.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
    }

    void Update()
    {
        if (userSystem.gameDataServerConnectionMode != GameDataServerConnectionMode.StartUp && !musicAudioSource.isPlaying)
            musicAudioSource.Play();

        gameDataServerStateText.text = userSystem.gameDataServerConnectionMode.ToString();
        usernameText.text = userSystem.caseomaticUsername;

        SetAbsoluteProgressVarValue(currentProgressBarValue);
    }

    private void SetAbsoluteProgressVarValue(float relative)
    {
        float screenRelative = (Screen.width / 100) * relative;
        progressBarValue.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenRelative);
    }
}
