using UnityEngine;

public class PcChatEvent : MonoBehaviour
{
    [SerializeField] PC _pc;
    [SerializeField] GameObject _cabinetInformation;
    [SerializeField] GameObject _cabinetOriginal;

    private void Start()
    {
        _pc.OnComplete += UnlockCabinet;
    }

    private void OnDestroy()
    {
        _pc.OnComplete -= UnlockCabinet;
    }

    private void UnlockCabinet()
    {
        _cabinetInformation.gameObject.SetActive(false);
        _cabinetOriginal.gameObject.SetActive(true);
    }
}
