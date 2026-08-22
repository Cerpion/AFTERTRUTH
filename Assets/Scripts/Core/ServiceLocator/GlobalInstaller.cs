using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalInstaller : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private TransitionManager _transitionManager;
    [SerializeField] private LanguageHandler _languageHandler;
    [SerializeField] private SettingsManager _settingsManager;
    [SerializeField] private bool _isDebug;
    //[SerializeField] private DialogueManager _dialogueManager;
    //[SerializeField] private AudioManager _audioManager;
    //[SerializeField] private SettingsManager _settingsManager;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterServices<InputHandler>(_inputHandler);
        DontDestroyOnLoad(_inputHandler);

        ServiceLocator.Instance.RegisterServices<TransitionManager>(_transitionManager);
        DontDestroyOnLoad(_transitionManager);

        ServiceLocator.Instance.RegisterServices<SettingsManager>(_settingsManager);
        DontDestroyOnLoad(_settingsManager);
        _settingsManager.Init();

        ServiceLocator.Instance.RegisterServices<LanguageHandler>(_languageHandler);
        DontDestroyOnLoad(_languageHandler);
        _languageHandler.Init(_settingsManager.Language);

        if (!_isDebug)
        SceneManager.LoadScene("MainMenu");

        //ServiceLocator.Instance.RegisterServices<SettingsManager>(_settingsManager);
        //ServiceLocator.Instance.RegisterServices<AudioManager>(_audioManager);
        //ServiceLocator.Instance.RegisterServices<DialogueManager>(_dialogueManager);
    }

}

