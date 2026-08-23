    using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatInformation : MonoBehaviour
{
    [SerializeField] private PC _pcScreen;
    [SerializeField] private List<PCPopup> _popup;
    [SerializeField] private string _chat;
    [SerializeField] private int _index;

    [SerializeField] private TMP_Text _repliAppTitle;

    [SerializeField] private TMP_Text _chatGirl01;
    [SerializeField] private TMP_Text _chatGirl02;

    [SerializeField] private TMP_Text _chat01;
    [SerializeField] private TMP_Text _chat02;
    [SerializeField] private TMP_Text _chat03;
    [SerializeField] private TMP_Text _chat04;
    [SerializeField] private TMP_Text _chat05;


    private void Awake()
    {
        foreach (var item in _popup)
        {
            item.OnClose += ClosePopup;
        }

        _repliAppTitle.text = ServiceLocator.Instance.GetService<LanguageHandler>().Get("REPLIAPP");

        _chat01.text = ServiceLocator.Instance.GetService<LanguageHandler>().Get("CHAT_01");
        _chatGirl01.text = ServiceLocator.Instance.GetService<LanguageHandler>().Get("CHAT_02");
        _chat02.text = ServiceLocator.Instance.GetService<LanguageHandler>().Get("CHAT_03");
        _chat03.text = ServiceLocator.Instance.GetService<LanguageHandler>().Get("CHAT_04");
        _chat04.text = ServiceLocator.Instance.GetService<LanguageHandler>().Get("CHAT_05");
        _chat05.text = ServiceLocator.Instance.GetService<LanguageHandler>().Get("CHAT_06");
        _chatGirl02.text = ServiceLocator.Instance.GetService<LanguageHandler>().Get("CHAT_07");
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