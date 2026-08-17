using System;
using UnityEngine;

public enum DoorOpen
{
    Outside,
    inside
}

public class Door : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private ItemID _itemRequired;
    [SerializeField] private string _information;

    [SerializeField] private InteractableObject _interactable;
    [SerializeField] private Transform _doorPivot;
    [SerializeField] private DoorOpen _openDirection;

    [Header ("audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _locked;
    [SerializeField] private AudioClip _open;
    public Action OnDoorLock;


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
        if(_information != string.Empty)
        DialogueManager.Instance.Play(_information);

        _audioSource.clip = _locked;
        _audioSource.Play();

        OnDoorLock?.Invoke();
    }
}
