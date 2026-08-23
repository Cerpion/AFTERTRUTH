using UnityEngine;

public class SafeBoxEvent : MonoBehaviour
{
    [SerializeField] Altar _altar;
    [SerializeField] GameObject _oldSafe;
    [SerializeField] GameObject _originalSafe;

    private void Start()
    {
        _altar.OnInteracted += UnlockSafe;
    }

    private void OnDestroy()
    {
        _altar.OnInteracted -= UnlockSafe;
    }

    private void UnlockSafe()
    {
        _oldSafe.SetActive(false);
        _originalSafe.SetActive(true);
        _altar.OnInteracted -= UnlockSafe;
        gameObject.SetActive(false);
    }
}