using UnityEngine;

public enum DoorOpen
{
    Outside,
    inside
}

public class Door : MonoBehaviour
{
    [SerializeField] private InteractableObject _interactable ;
    [SerializeField] private Transform _doorPivot ;
    [SerializeField] private DoorOpen _openDirection ;

    private void Start()
    {
        _interactable.OnInteracted += OpenDoor;
    }

    private void OpenDoor()
    {
        var rotation = _openDirection == DoorOpen.Outside ? 90 : -90;
        _doorPivot.LeanRotateAroundLocal(Vector3.up, rotation, 1f);
    }
}
