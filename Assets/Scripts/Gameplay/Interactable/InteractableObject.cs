using System;
using UnityEngine;

public class InteractableObject : Interactable
{
    private ItemID _requiredItem;

    public Action OnInteracted;
    public Action OnLocked;

    public void SetItemsRequired(ItemID item)
    {
        _requiredItem = item;
    }

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
            OnLocked?.Invoke();
            return;
        }

        player.Inventory.TryRemove(_requiredItem);
        OnInteracted?.Invoke();

        RemoveInteractable();
    }

}