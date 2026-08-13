using UnityEngine;

[CreateAssetMenu(fileName = "InteractiveConfiguration", menuName = "Items/Configuration", order = 1)]
public class InteractiveItemsConfiguration : ScriptableObject
{
    [SerializeField] public ItemInspect[] NormalItems;
    [SerializeField] public ItemInspect[] InteractiveItems;
}
