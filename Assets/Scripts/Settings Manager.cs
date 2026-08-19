using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Default Values")]
    [SerializeField] private float defaultMouseSensitivity = 1f;
    [SerializeField] private float defaultVolume = 1f;

    private const string MouseSensitivityKey = "MouseSensitivity";
    private const string VolumeKey = "Volume";

    public float MouseSensitivity { get; private set; }
    public float Volume { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

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
    }

    private void ApplySettings()
    {
        AudioListener.volume = Volume;
    }

    public void ResetSettings()
    {
        SetMouseSensitivity(defaultMouseSensitivity);
        SetVolume(defaultVolume);
    }
}