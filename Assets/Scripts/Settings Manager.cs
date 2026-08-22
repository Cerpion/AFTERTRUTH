using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Default Values")]
    [SerializeField] private float defaultMouseSensitivity = 1f;
    [SerializeField] private float defaultVolume = 1f;
    [SerializeField] private string defaultLanguage = "English";

    private const string MouseSensitivityKey = "MouseSensitivity";
    private const string VolumeKey = "Volume";
    private const string LanguageKey = "Language";

    public float MouseSensitivity { get; private set; }
    public float Volume { get; private set; }
    public string Language { get; private set; }

    public void Init()
    {
        LoadSettings();
        ApplySettings();
    }

    public void SetMouseSensitivity(float value)
    {
        MouseSensitivity = value;

        PlayerPrefs.SetFloat(MouseSensitivityKey, value);
        PlayerPrefs.Save();
    }

    public void SetVolume(float value)
    {
        Volume = value;

        AudioListener.volume = value;

        PlayerPrefs.SetFloat(VolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetLanguage(string language)
    {
        Language = language;

        PlayerPrefs.SetString(LanguageKey, language);
        PlayerPrefs.Save();
    }


    private void LoadSettings()
    {
        MouseSensitivity = PlayerPrefs.GetFloat(
            MouseSensitivityKey,
            defaultMouseSensitivity
        );

        Volume = PlayerPrefs.GetFloat(
            VolumeKey,
            defaultVolume
        );

        Language = PlayerPrefs.GetString(
           LanguageKey,
           defaultLanguage
       );
    }

    private void ApplySettings()
    {
        AudioListener.volume = Volume;
    }

    public void ResetSettings()
    {
        SetMouseSensitivity(defaultMouseSensitivity);
        SetVolume(defaultVolume);
        SetLanguage(defaultLanguage);
    }
}