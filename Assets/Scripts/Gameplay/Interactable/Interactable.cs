using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]public InteractionView _interactionView;

    public virtual void StartInteraction() {}
    public virtual void ExitInteraction() {}
}