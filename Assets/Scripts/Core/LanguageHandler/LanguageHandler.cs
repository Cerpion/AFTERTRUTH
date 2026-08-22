using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageHandler : MonoBehaviour
{
    [SerializeField] private TextAsset translationFile;

    private Dictionary<string, string> _translations;
    private CSVLoader _csvLoader;

    public string CurrentLanguage { get; private set; }

    public event Action LanguageChanged;

    public void Init(string Language)
    {
        _csvLoader = new CSVLoader();
        SetLanguage(Language);
    }

    public void SetLanguage(string language)
    {
        Debug.Log("Change");
        _translations = _csvLoader.LoadLanguage(translationFile,language);
        CurrentLanguage = language;
        LanguageChanged?.Invoke();
    }

    public string Get(string id)
    {
        if (_translations.TryGetValue(id, out string translation))
            return translation;

        Debug.LogWarning($"Translation not found: {id}");

        return $"[{id}]";
    }
}
