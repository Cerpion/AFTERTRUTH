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

    [SerializeField] CanvasGroup _noticePhone;

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
        DisableEvent();

        DialogueManager.Instance.Play(_introContext);
        _gateway.CloseDoor();

        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Cinematic);
        var player = ServiceLocator.Instance.GetService<Player>();

        var sequence = LeanTween.sequence();

        sequence.append(() => { player.ShowPhone(); });
        sequence.append(1);

        sequence.append(() => { _noticePhone.alpha = 0;  _noticePhone.gameObject.SetActive(true); _noticePhone.LeanAlpha(1, 0.25f); });
        sequence.append(3f);
        sequence.append(() => { _noticePhone.LeanAlpha(0, 0.25f).setOnComplete( () => _noticePhone.gameObject.SetActive(false)); });

        sequence.append(() => { player.HidePhone(); });
        sequence.append(1);
        sequence.append(() => { ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay); });

    }
}
