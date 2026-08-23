using UnityEngine;

public class InteractableInformation : Interactable
{
    public override bool ShowCursor => false;
    public int _indexInteractions;
    [SerializeField] private string _information;
    [SerializeField] private string _safeInteraction2;

    public override void OnInteractionStarted()
    {
        if (_indexInteractions >= 2)
        {
            DialogueManager.Instance.Play(_safeInteraction2);
            StopInteraction();
            return;
        }

        DialogueManager.Instance.Play(_information);
        _indexInteractions++;
        StopInteraction();
    }

    public override void OnInteractionEnded()
    {
    }
}
