using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]public InteractionView _interactionView;

    public virtual void CanInteract() {}
}

//public class Collectable : Interactable
//{
//    public override void CanInteract()
//    {

//    }
//}

//public class FixedPuzzle : Interactable
//{
//    public override void CanInteract()
//    {
//    }
//}
