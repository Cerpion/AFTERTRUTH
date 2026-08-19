using UnityEngine;

public class InspectItem : Interactable
{
    [SerializeField] private ItemID _itemID;
    [SerializeField] private string _dialogue;
    public override bool ShowCursor => false;

    public override void OnInteractionStarted()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect(_itemID.ID);
        inspection.OnInspectionFinished += StopInteraction;

        if (_dialogue != string.Empty)
        {
            DialogueManager.Instance.Play(_dialogue);
        }
    }

    public override void OnInteractionEnded()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= StopInteraction;
    }
}