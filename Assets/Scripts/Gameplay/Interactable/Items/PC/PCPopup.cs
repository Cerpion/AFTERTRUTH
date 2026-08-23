using System;
using UnityEngine;
using UnityEngine.UI;

public class PCPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _min;
    [SerializeField] private Button _max;
    [SerializeField] private Button _close;

    public Action OnClose;

    private void Awake()
    {
        _min.onClick.AddListener(Min);
        _max.onClick.AddListener(Max);
        _close.onClick.AddListener(Close);
    }

    private void Min()
    {
        Debug.Log("SoundError");
    }

    private void Max()
    {
        Debug.Log("SoundError");
    }

    private void Close()
    {
        _canvasGroup.LeanAlpha(0, 0.25f).setOnComplete( () => gameObject.SetActive(false));
        OnClose?.Invoke();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        _canvasGroup.LeanAlpha(1, 0.15f);
    }
}
