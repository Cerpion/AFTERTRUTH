using System;
using UnityEngine;

public class InteractableObject : Interactable
{
    private ItemID _requiredItem;

    public Action OnInteracted;
    public Action OnLocked;
    public override bool ShowCursor => true;

    public void SetItemsRequired(ItemID item)
    {
        _requiredItem = item;
    }

    public override void OnInteractionStarted()
    {
        var player = ServiceLocator.Instance.GetService<Player>();

        if (_requiredItem == null)
        {
            OnInteracted?.Invoke();
            RemoveInteractable();
            StopInteraction();
            return;
        }

        if (!player.Inventory.ContainItem(_requiredItem))
        {
            OnLocked?.Invoke();
            StopInteraction();
            return;
        }

        player.Inventory.TryRemove(_requiredItem);
        OnInteracted?.Invoke();

        RemoveInteractable();
        StopInteraction();
    }

    public override void OnInteractionEnded(){}
}