using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroSystem : MonoBehaviour
{
    public Canvas introCanvas;
    public Image logoImage;
    public Text caseomaticText, videogamesText;

    public float introFadeTime, introTime;
    public AudioSource audioSource;
    public AudioClip introSound;

    void Awake()
    {
        logoImage.CrossFadeAlpha(0, 0, true);
        caseomaticText.CrossFadeAlpha(0, 0, true);
        videogamesText.CrossFadeAlpha(0, 0, true);
        StartCoroutine(DoFading());
    }

    private void Finish()
    {
        Application.LoadLevel("Menu");
    }
    private IEnumerator DoFading()
    {
        logoImage.CrossFadeAlpha(1, introFadeTime, false);
        caseomaticText.CrossFadeAlpha(1, introFadeTime, false);
        videogamesText.CrossFadeAlpha(1, introFadeTime, false);

        yield return new WaitForSeconds(introFadeTime);
        audioSource.PlayOneShot(introSound);

        yield return new WaitForSeconds(introTime);

        logoImage.CrossFadeAlpha(0, introFadeTime / 2, false);
        caseomaticText.CrossFadeAlpha(0, introFadeTime / 2, false);
        videogamesText.CrossFadeAlpha(0, introFadeTime / 2, false);

        yield return new WaitForSeconds(introFadeTime);
        Finish();
    }
}
