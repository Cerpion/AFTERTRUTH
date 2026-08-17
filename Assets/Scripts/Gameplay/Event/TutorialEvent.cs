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
}

public class TutorialEvent : EventCinematic
{
    [SerializeField] Door _gateway;
    [SerializeField] InspectPuzzle _rug;
    [SerializeField] string _dialog;

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
        DialogueManager.Instance.Play(_dialog);
    }

    public override void Execute()
    {

        gameObject.SetActive(false);
    }
}
