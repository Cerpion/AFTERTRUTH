using UnityEngine;

public class InspectionCollectable : MonoBehaviour
{
    [SerializeField] private ItemID _itemID;
    public void Select()
    {
        transform.LeanScale(Vector3.one * 1.1f, 0.25f);
    }

    public void UnSelect()
    {
        transform.LeanScale(Vector3.one, 0.25f);
    }

    public void Obtained()
    {
        ServiceLocator.Instance.GetService<InspectionSystem>().OnItemObtained?.Invoke();
        ServiceLocator.Instance.GetService<InspectionSystem>().RemoveCurrentInspection();
        ServiceLocator.Instance.GetService<InspectionSystem>().StartInspect(_itemID.ID);
        ServiceLocator.Instance.GetService<Player>().Inventory.TryAdd(_itemID);
        gameObject.SetActive(false);
    }
}