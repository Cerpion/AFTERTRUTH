using UnityEngine;

[CreateAssetMenu(fileName = "ItemID", menuName = "Items/ItemID", order = 0)]
public class ItemID : ScriptableObject
{
    [SerializeField] public string ID;
}
