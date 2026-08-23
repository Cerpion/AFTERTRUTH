using UnityEngine;

public class EventCinematic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Execute();
    }

    public virtual void Execute()
    {

    }

    public void DisableEvent()
    {
        gameObject.SetActive(false);
    }
}

public class TutorialEvent : EventCinematic
{
    [SerializeField] Door _gateway;
    [SerializeField] InspectPuzzle _rug;
    [SerializeField] string _introContext;

    private void Start()
    {
        _gateway.OnDoorLock += interaction;
    }

    private void OnDestroy()
    {
        _gateway.OnDoorLock -= interaction;
    }


    private void interaction()
    {
        _rug.GetComponent<BoxCollider>().enabled = true;
    }

    public override void Execute()
    {
        DialogueManager.Instance.Play(_introContext);
        _gateway.CloseDoor();
        DisableEvent();
    }
}
