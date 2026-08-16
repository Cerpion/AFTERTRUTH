using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InspectPuzzle : Interactable
{
    [SerializeField] private ItemID _itemID;
    [SerializeField] private ItemID _requiredItem;
    [SerializeField] private string _information;

    public override void StartInteraction()
    {
        if (_requiredItem  != null)
        {
            //ShowDialog
            return;
        }

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect(_itemID.ID);
        inspection.OnInspectionFinished += ExitInteraction;
        inspection.OnItemObtained += ItemObtained;

        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);
    }

    public override void ExitInteraction()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= ExitInteraction;
        inspection.OnItemObtained -= ItemObtained;

        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
    }

    private void ItemObtained()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
