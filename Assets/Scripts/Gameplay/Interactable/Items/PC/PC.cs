using System;
using UnityEngine;

public class PC : Interactable
{
    [SerializeField] GlobalLightHandler _globalLightHandler;

    public override bool ShowCursor => true;
    public PcScreen _pcScreen;
    public CanvasGroup _canvasGroup;
    public string _password;
    public Action OnComplete;
    public string _dayToNight;

    public override void OnInteractionStarted()
    {
        OnEnterBlendFinished += ShowCanvas;
        _pcScreen.OnInputFieldSelected += DesactiveInteraction;
        _pcScreen.OnComprobatePassword += VerifyPassword;
    }

    public override void OnInteractionEnded()
    {
        OnEnterBlendFinished -= ShowCanvas;
        _pcScreen.OnInputFieldSelected -= DesactiveInteraction;
        _pcScreen.OnComprobatePassword -= VerifyPassword;
        _canvasGroup.LeanAlpha(0,0.25f).setOnComplete( () => { _pcScreen.gameObject.SetActive(false); }) ;
    }

    private void ShowCanvas()
    {
        _pcScreen.gameObject.SetActive(true);
        _canvasGroup.LeanAlpha(1, 0.25f);
    }

    private void DesactiveInteraction(bool value)
    {
        if (value)
        {
            PauseInteraction = true;
            return;
        }

        PauseInteraction = false;
    }

    private void VerifyPassword(string value)
    {
        if (_password == value)
        {
            _pcScreen.ShowDesktopTwo();
            Debug.Log("victory");
            return;
        }

        _pcScreen.ShowPasswordError();
    }

    public void EndDay()
    {
        DialogueManager.Instance.Play(_dayToNight);
        _globalLightHandler.SetNight();
    }
}
