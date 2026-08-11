using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]public InteractionView _interactionView;

    public virtual void StartInteraction() {}
    public virtual void ExitInteraction() {}
}

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    private Interactable _currentItem;

    public void StartInteraction()
    {

    }

    public void CancelInteraction()
    {

    }

}