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

        _inventory.Add(item);
        UpdateInventory();
    }

    public void TryRemove(ItemID item)
    {
        //if (_inventory.Contains(item))
        //{
        //    return;
        //}

        _inventory.Remove(item);
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        _inventoryView.Reset();

        for (int i = 0; i < _inventory.Count; i++)
        {
            var stats = ServiceLocator.Instance.GetService<InspectionSystem>().InteractiveItemFactory.GetStatsByID(_inventory[i].ID);
            _inventoryView.UpdateView(i, stats.Icon);
        }
    }

    public bool ContainItem(ItemID item)
    {
        return _inventory.Contains(item);
    }
}
