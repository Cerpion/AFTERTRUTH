using UnityEngine;

public class CloseDevelopRoom : EventCinematic
{
    [SerializeField] private Door _developRoom;

    public override void Execute()
    {
        _developRoom.CloseDoor();
        DisableEvent();
    }
}