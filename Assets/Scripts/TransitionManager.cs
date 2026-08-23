using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup _fade;

    [Header("Scenes")]
    [SerializeField] private string _mainMenuScene = "MainMenu";
    [SerializeField] private string _gameplayScene = "Gameplay";
    [SerializeField] private string _firstEnding = "FirstEnding";
    [SerializeField] private string _secondEnding = "SecondEnding";

    private void Start()
    {
        _fade.alpha = 0f;
        _fade.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(LoadLevelByName(_gameplayScene));
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadLevelByName(_mainMenuScene));
    }
    public void FirstEnding()
    {
        StartCoroutine(LoadLevelByName(_firstEnding, 0));
    }

    public void SecondEnding()
    {
        StartCoroutine(LoadLevelByName(_secondEnding, 0));
    }

    private IEnumerator LoadLevelByName(string levelName, float fadeIn = 0.5f, float fadeOut = 0.8f)
    {
        yield return Fade(1f, fadeIn);
        yield return LoadScene(levelName);
        yield return Fade(0f, fadeOut);
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
    }

    private IEnumerator Fade(float targetAlpha, float fadeDuration)
    {
        if (targetAlpha > 0f)
            _fade.gameObject.SetActive(true);

        bool finished = false;

        LeanTween.alphaCanvas(_fade, targetAlpha, fadeDuration)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                finished = true;

                if (targetAlpha <= 0f)
                    _fade.gameObject.SetActive(false);
            });

        yield return new WaitUntil(() => finished);
    }

}
