using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PcScreen: MonoBehaviour
{
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Button _password;
    [SerializeField] GameObject _passwordError;
    [SerializeField] GameObject _desktopOne;
    [SerializeField] GameObject _desktopTwo;
    public Action<bool> OnInputFieldSelected;
    public Action<string> OnComprobatePassword;

    public void Start()
    {
        _inputField.onSelect.AddListener(Selected);
        _inputField.onDeselect.AddListener(OnDeselected);
        _password.onClick.AddListener(Password);
    }

    private void Selected(string value)
    {
        OnInputFieldSelected?.Invoke(true);
    }

    private void OnDeselected(string value)
    {
        OnInputFieldSelected?.Invoke(false);
    }

    private void Password()
    {
        OnComprobatePassword?.Invoke(_inputField.text);
    }

    public void ShowPasswordError()
    {
        _passwordError.gameObject.SetActive(true);
    }

    public void ShowDesktopTwo()
    {
        _desktopOne.gameObject.SetActive(false);
        _desktopTwo.gameObject.SetActive(true);
    }
}