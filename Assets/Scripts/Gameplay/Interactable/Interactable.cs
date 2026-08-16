using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public InteractionView _interactionView;
    public Action<Interactable> OnRemoveInteractable;

    public virtual void StartInteraction() {}
    public virtual void ExitInteraction() {}

    public void RemoveInteractable()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        OnRemoveInteractable?.Invoke(this);
    }
}