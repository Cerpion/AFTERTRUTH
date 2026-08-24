using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private CanvasGroup pauseScreen;
    [SerializeField] private CanvasGroup pauseMainScreen;
    [SerializeField] private CanvasGroup pauseSettingsScreen;

    [Header("Transition")]
    [SerializeField] private float transitionDuration = 0.25f;

    private bool isPaused;

    public bool IsPaused => isPaused;

    private void Start()
    {
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        pauseScreen.gameObject.SetActive(true);
        pauseScreen.alpha = 0f;

        pauseMainScreen.gameObject.SetActive(true);
        pauseMainScreen.alpha = 1f;

        pauseSettingsScreen.gameObject.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        LeanTween.cancel(pauseScreen.gameObject);

        LeanTween.alphaCanvas(pauseScreen, 1f, transitionDuration)
            .setIgnoreTimeScale(true);
    }

    public void ResumeGame()
    {
        isPaused = false;

        LeanTween.cancel(pauseScreen.gameObject);

        LeanTween.alphaCanvas(pauseScreen, 0f, transitionDuration)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => { pauseScreen.gameObject.SetActive(false); });

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        ChangeScreen(pauseMainScreen, pauseSettingsScreen);
    }

    public void CloseSettings()
    {
        ChangeScreen(pauseSettingsScreen, pauseMainScreen);
    }

    private void ChangeScreen(CanvasGroup from, CanvasGroup to)
    {
        LeanTween.cancel(from.gameObject);
        LeanTween.cancel(to.gameObject);

        to.gameObject.SetActive(true);

        from.alpha = 1f;
        to.alpha = 0f;

        LeanTween.alphaCanvas(from, 0f, transitionDuration)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>{from.gameObject.SetActive(false);});

        LeanTween.alphaCanvas(to, 1f, transitionDuration)
            .setIgnoreTimeScale(true);
    }


    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        ServiceLocator.Instance.GetService<TransitionManager>().ReturnToMainMenu();
        //SceneManager.LoadScene(mainMenuSceneName);
    }
}