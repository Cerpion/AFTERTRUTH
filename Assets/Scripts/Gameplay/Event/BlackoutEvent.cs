using UnityEngine;

public class BlackoutEvent : MonoBehaviour
{
    [SerializeField] GlobalLightHandler _globalLightHandler;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _globalLightHandler.SetNight();
        gameObject.SetActive(false);
    }
}
