using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider volumeSlider;

    private void OnEnable()
    {
        if (SettingsManager.Instance == null)
            return;

        mouseSensitivitySlider.value =
            SettingsManager.Instance.MouseSensitivity;

        volumeSlider.value =
            SettingsManager.Instance.Volume;
    }

    public void SetMouseSensitivity(float value)
    {
        SettingsManager.Instance.SetMouseSensitivity(value);
    }

    public void SetVolume(float value)
    {
        SettingsManager.Instance.SetVolume(value);
    }
}