using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField] private ItemID _itemID;
    [SerializeField] private Player _player;

    public override void StartInteraction()
    {
        //Open Investigation
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);
        ServiceLocator.Instance.GetService<InspectionSystem>().StartInspect(_itemID.ID);
    }

    public override void ExitInteraction()
    {
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
        gameObject.SetActive(false);
        Debug.LogWarning("Add item To Inventory");
        //Close Investigation
        //Add Item
        //se puede mover

    }


}