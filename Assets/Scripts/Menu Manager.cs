using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private Button _es;
    [SerializeField] private Button _en;

    [Header("Game")]
    [SerializeField] private string gameSceneName = "Game";

    private void Start()
    {
        ShowMainMenu();
        _es.onClick.AddListener(Spanish);
        _en.onClick.AddListener(English);
    }

    private void Spanish()
    {
        ServiceLocator.Instance.GetService<LanguageHandler>().SetLanguage("Spanish");
    }

    private void English()
    {
        ServiceLocator.Instance.GetService<LanguageHandler>().SetLanguage("English");
    }

    public void ShowMainMenu()
    {
        mainMenuScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    public void OpenSettings()
    {
        mainMenuScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void CloseSettings()
    {
        mainMenuScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(gameSceneName);
        ServiceLocator.Instance.GetService<TransitionManager>().StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        Application.Quit();
    }
}