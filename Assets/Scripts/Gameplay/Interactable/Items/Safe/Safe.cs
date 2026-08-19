using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Safe : Interactable
{
    [Header("Solution")]
    [SerializeField] private string _correctCode = "1234";

    [Header("UI")]
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TMP_Text _inputText;

    [Header("Number Buttons")]
    [SerializeField] private Button[] _numberButtons;

    [Header("Result")]
    [SerializeField] private GameObject _failState;
    [SerializeField] private GameObject _winState;

    private string _currentInput = "";
    public override bool ShowCursor { get => true; }


    private void Start()
    {
        _canvas.SetActive(false);

        if (_failState != null)
            _failState.SetActive(false);

        if (_winState != null)
            _winState.SetActive(false);

        SetupNumberButtons();
    }

    private void SetupNumberButtons()
    {
        for (int i = 0; i < _numberButtons.Length; i++)
        {
            int number = i;

            _numberButtons[i].onClick.AddListener(() => AddDigit(number));
        }
    }

    public override void OnInteractionStarted()
    {
        _canvas.SetActive(true);

        _currentInput = "";
        UpdateInputText();

        if (_failState != null)
            _failState.SetActive(false);

        if (_winState != null)
            _winState.SetActive(false);
    }

    public override void OnInteractionEnded()
    {
        _canvas.SetActive(false);

        _currentInput = "";
        UpdateInputText();

       
    }

    private void AddDigit(int digit)
    {
        if (_currentInput.Length >= 4)
            return;

        _currentInput += digit.ToString();

        UpdateInputText();

        if (_currentInput.Length == 4)
        {
            CheckCode();
        }
    }

    private void UpdateInputText()
    {
        if (_inputText != null)
            _inputText.text = _currentInput;
    }

    private void CheckCode()
    {
        if (_currentInput == _correctCode)
        {
            Win();
        }
        else
        {
            Fail();
        }
    }

    private void Fail()
    {
        if (_failState != null)
            _failState.SetActive(true);

        _currentInput = "";
        UpdateInputText();

        Debug.Log("Fallaste");
        DialogueManager.Instance.Play("Hola");
        StopInteraction();
    }

    private void Win()
    {
        if (_winState != null)
            _winState.SetActive(true);

        Debug.Log("Ganaste");
        DialogueManager.Instance.Play("Hola");
        StopInteraction();
    }

}