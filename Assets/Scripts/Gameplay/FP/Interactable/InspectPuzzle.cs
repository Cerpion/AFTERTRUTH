using System.Collections.Generic;
using UnityEngine;

public class InspectPuzzle : Interactable
{
    [SerializeField] private string _itemID;
    [SerializeField] private string _requiredItems;
    [SerializeField] private List<GameObject> _itemsToActivate;
    [SerializeField] private string _information;

    public override void StartInteraction()
    {
        if (_requiredItems != "")
        {
            //ShowDialog
            return;
        }
        ServiceLocator.Instance.GetService<InspectionSystem>().StartInspect();
    }
}