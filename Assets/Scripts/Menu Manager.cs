using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private CanvasGroup mainMenuScreen;
    [SerializeField] private CanvasGroup settingsScreen;
    [SerializeField] private CanvasGroup _creditsScreen;
    private const float FadeDuration = 0.25f;

    public void ShowMainMenu()
    {
        ChangeScreen(settingsScreen, mainMenuScreen);
    }

    public void OpenSettings()
    {
        ChangeScreen(mainMenuScreen, settingsScreen);
    }

    public void CloseSettings()
    {
        ChangeScreen(settingsScreen, mainMenuScreen);
    }

    public void OpenCredits()
    {
        LeanTween.cancel(_creditsScreen.gameObject);

        _creditsScreen.gameObject.SetActive(true);
        _creditsScreen.alpha = 0f;

        LeanTween.alphaCanvas(_creditsScreen, 1f, FadeDuration);
    }

    public void CloseCredits()
    {
        LeanTween.cancel(_creditsScreen.gameObject);

        LeanTween.alphaCanvas(_creditsScreen, 0f, FadeDuration)
            .setOnComplete(() =>
            {
                _creditsScreen.gameObject.SetActive(false);
            });
    }

    private void ChangeScreen(CanvasGroup from, CanvasGroup to)
    {
        LeanTween.cancel(from.gameObject);
        LeanTween.cancel(to.gameObject);

        to.gameObject.SetActive(true);

        from.alpha = 1f;
        to.alpha = 0f;

        LeanTween.alphaCanvas(from, 0f, FadeDuration) .setOnComplete(() => { from.gameObject.SetActive(false); });
        LeanTween.alphaCanvas(to, 1f, FadeDuration);
    }


    public void StartGame()
    {
        ServiceLocator.Instance.GetService<TransitionManager>().StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        Application.Quit();
    }
}