using UnityEngine;

public class InspectItem : Interactable
{
    [SerializeField] private ItemID _itemID;
    [SerializeField] private string _dialogue;

    public override void StartInteraction()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect(_itemID.ID);
        inspection.OnInspectionFinished += ExitInteraction;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);

        if (_dialogue != string.Empty)
        {
        DialogueManager.Instance.Play(_dialogue);
        }
    }

    public override void ExitInteraction()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= ExitInteraction;

        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
    }
}