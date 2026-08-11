using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField] private string _itemID;
    [SerializeField] private Player _player;

    public override void StartInteraction()
    {
        Debug.LogWarning("Add item To Inventory");
        gameObject.SetActive(false);
    }
}