using System.Collections.Generic;
using UnityEngine;

public class ChatInformation : MonoBehaviour
{
    [SerializeField] private PC _pcScreen;
    [SerializeField] private List<PCPopup> _popup;
    [SerializeField] private string _chat;
    [SerializeField] private int _index;

    private void Awake()
    {
        foreach (var item in _popup)
        {
            item.OnClose += ClosePopup;
        }
    }
    private void OnDestroy()
    {
        foreach (var item in _popup)
        {
            item.OnClose -= ClosePopup;
        }

    }

    public void ClosePopup() 
    {
        _index++;

        if (_index < 2)
            return;

        DialogueManager.Instance.Play(_chat);
        _pcScreen.OnComplete.Invoke();
    }
}