using System.Collections.Generic;
using Unity.VisualScripting;
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
    }

    public override void OnInteractionEnded()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= StopInteraction;
        inspection.OnItemObtained -= ItemObtained;
    }

    private void ItemObtained()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }


}
