using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject settingsScreen;

    [Header("Game")]
    [SerializeField] private string gameSceneName = "Game";

    private void Start()
    {
        ShowMainMenu();
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
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        Application.Quit();
    }
}