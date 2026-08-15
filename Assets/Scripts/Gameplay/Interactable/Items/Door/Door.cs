using UnityEngine;

public enum DoorOpen
{
    Outside,
    inside
}

public class Door : MonoBehaviour
{
    [SerializeField] private ItemID _itemRequired;
    [SerializeField] private InteractableObject _interactable;
    [SerializeField] private Transform _doorPivot;
    [SerializeField] private DoorOpen _openDirection;
    [SerializeField] private string _information;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _locked;
    [SerializeField] private AudioClip _open;


    private void Start()
    {
        _interactable.SetItemsRequired(_itemRequired);
        _interactable.OnInteracted += OpenDoor;
        _interactable.OnLocked += DoorLock;
    }

    private void OpenDoor()
    {
        var rotation = _openDirection == DoorOpen.Outside ? 90 : -90;
        _doorPivot.LeanRotateAroundLocal(Vector3.up, rotation, 1f);

        _audioSource.clip = _open;
        _audioSource.Play();
    }

    private void DoorLock()
    {
        //ShowDialog
        Debug.LogWarning(_information);

        _audioSource.clip = _locked;
        _audioSource.Play();
    }
}
