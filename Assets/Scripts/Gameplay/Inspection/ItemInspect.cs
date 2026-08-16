using UnityEngine;

[System.Serializable]
public class ItemInspect
{
    [SerializeField] public ItemID ItemID;
    [SerializeField] public Sprite Icon;
    [SerializeField] public GameObject Prefab;
}