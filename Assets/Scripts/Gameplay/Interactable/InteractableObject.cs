using System;
using UnityEngine;

public class InteractableObject : Interactable
{
    [SerializeField] private ItemID _requiredItem;
    [SerializeField] private string _information;
    public Action OnInteracted;

    public override void StartInteraction()
    {
        var player = ServiceLocator.Instance.GetService<Player>();

        if (_requiredItem == null)
        {
            OnInteracted?.Invoke();
            RemoveInteractable();
            return;
        }

        if (!player.Inventory.ContainItem(_requiredItem))
        {
            Debug.LogWarning(_information);
            //ShowDialog
            return;
        }

        player.Inventory.TryRemove(_requiredItem);
        OnInteracted?.Invoke();

        RemoveInteractable();
    }

}