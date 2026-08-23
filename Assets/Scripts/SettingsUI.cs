using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider volumeSlider;

    private void OnEnable()
    {
        var settings = ServiceLocator.Instance.GetService<SettingsManager>();

        mouseSensitivitySlider.value = settings.MouseSensitivity;
        volumeSlider.value = settings.Volume;

        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
    }

    public void SetMouseSensitivity(float value)
    {
        ServiceLocator.Instance.GetService<SettingsManager>().SetMouseSensitivity(value);
        //SettingsManager.Instance.SetMouseSensitivity(value);
    }

    public void SetVolume(float value)
    {
        ServiceLocator.Instance.GetService<SettingsManager>().SetVolume(value);
        //SettingsManager.Instance.SetVolume(value);
    }
}