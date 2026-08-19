using Unity.VisualScripting;
using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField] private ItemID _itemID;
    [SerializeField] private string _dialog;
    public override bool ShowCursor => true;

    public override void OnInteractionStarted()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect(_itemID.ID);
        inspection.OnInspectionFinished += StopInteraction;

        if(_dialog != string.Empty)
        DialogueManager.Instance.Play(_dialog);
    }

    public override void OnInteractionEnded()
    {
        gameObject.SetActive(false);
        ServiceLocator.Instance.GetService<Player>().Inventory.TryAdd(_itemID);

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= StopInteraction;
    }
}