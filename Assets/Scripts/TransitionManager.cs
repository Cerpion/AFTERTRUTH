using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup _fade;
    [SerializeField] private float _fadeDuration = 0.5f;

    [Header("Scenes")]
    [SerializeField] private string _mainMenuScene = "MainMenu";
    [SerializeField] private string _gameplayScene = "Gameplay";

    private void Start()
    {
        _fade.alpha = 0f;
        _fade.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(LoadGame());
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadGame()
    {
        yield return Fade(1f);
        yield return LoadScene(_gameplayScene);
        yield return Fade(0f);
    }

    private IEnumerator LoadMainMenu()
    {
        yield return Fade(1f);
        yield return LoadScene(_mainMenuScene);
        yield return Fade(0f);
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (targetAlpha > 0f)
            _fade.gameObject.SetActive(true);

        bool finished = false;

        LeanTween.alphaCanvas(_fade, targetAlpha, _fadeDuration)
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
