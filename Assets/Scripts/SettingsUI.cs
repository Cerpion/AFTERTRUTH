using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider volumeSlider;

    [Header("Language")]
    [SerializeField] private Button _es;
    [SerializeField] private Button _en;
    [SerializeField] private TMP_Text _language;
    private Languages _currentLanguage;

    private void Start()
    {
        _currentLanguage = Enum.Parse<Languages>(ServiceLocator.Instance.GetService<SettingsManager>().Language);

        _language.text = _currentLanguage.ToString();

        _es.onClick.AddListener(NextIdiom);
        _en.onClick.AddListener(PreviousIdiom);
    }

    private void NextIdiom()
    {
        ChangeLanguage();
        ServiceLocator.Instance.GetService<LanguageHandler>().SetLanguage(_currentLanguage.ToString());
    }

    private void PreviousIdiom()
    {
        ChangeLanguage();
        ServiceLocator.Instance.GetService<LanguageHandler>().SetLanguage(_currentLanguage.ToString());
    }

    private void ChangeLanguage()
    {
        _currentLanguage = _currentLanguage == Languages.Spanish
       ? Languages.English
       : Languages.Spanish;

        _language.text = _currentLanguage.ToString();
    }


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