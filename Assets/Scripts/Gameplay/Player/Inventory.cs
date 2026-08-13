using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private List<ItemID> _inventory;
    [SerializeField] private int _limit;

    private void Start()
    {
        _inventory = new List<ItemID>();
    }

    public void TryAdd(ItemID item)
    {
        if (_inventory.Count >= _limit)
        {
            return;
        }

        if (_inventory.Contains(item))
        {
            return;
        }

        var stats = ServiceLocator.Instance.GetService<InspectionSystem>().InteractiveItemFactory.GetStatsByID(item.ID);
        _inventoryView.AddItem(_inventory.Count, stats.Icon);
        _inventory.Add(item);
    }

    public void TryRemove(ItemID item)
    {
        //if (_inventory.Contains(item))
        //{
        //    return;
        //}

        var indexItem = _inventory.IndexOf(item);
        _inventoryView.Remove(indexItem);

        _inventory.Remove(item);
    }

    public bool ContainItem(ItemID item)
    {
        return _inventory.Contains(item);
    }
}
