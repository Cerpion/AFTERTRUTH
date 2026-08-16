using Unity.VisualScripting;
using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField] private ItemID _itemID;
    [SerializeField] private Player _player;

    public override void StartInteraction()
    {
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect(_itemID.ID);
        inspection.OnInspectionFinished += ExitInteraction;
    }

    public override void ExitInteraction()
    {
        gameObject.SetActive(false);
        ServiceLocator.Instance.GetService<Player>().Inventory.TryAdd(_itemID);
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
    }


}