using System.Collections.Generic;
using UnityEngine;

public class InspectPuzzle : Interactable
{
    [SerializeField] private string _itemID;
    [SerializeField] private string _requiredItems;
    [SerializeField] private List<GameObject> _itemsToActivate;
    [SerializeField] private string _information;

    [SerializeField] private InspectionSystem _inspectionSystem;
    public override void StartInteraction()
    {
        if (_requiredItems != "")
        {
            //ShowDialog
            return;
        }

        //_inspectionSystem.StartInspect();
    }
}