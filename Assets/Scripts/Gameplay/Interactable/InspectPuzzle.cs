using System.Collections.Generic;
using UnityEngine;

public class InspectPuzzle : Interactable
{
    [SerializeField] private ItemID _itemID;
    [SerializeField] private ItemID _requiredItem;
    [SerializeField] private string _information;
    public override bool ShowCursor => false;

    public override void OnInteractionStarted()
    {
        if (_requiredItem  != null)
        {
            //ShowDialog
            return;
        }

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect(_itemID.ID);
        inspection.OnInspectionFinished += StopInteraction;
        inspection.OnItemObtained += ItemObtained;

        ServiceLocator.Instance.GetService<ItemsMovement>().ShowUI();
        ServiceLocator.Instance.GetService<ItemsMovement>().ShowPuzzleControls();
    }

    public override void OnInteractionEnded()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= StopInteraction;
        inspection.OnItemObtained -= ItemObtained;
        ServiceLocator.Instance.GetService<ItemsMovement>().HideUI();

    }

    private void ItemObtained()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }


}
