using System.Collections.Generic;
using UnityEngine;

public class InteractiveItemFactory : MonoBehaviour
{
    private Dictionary<string, GameObject> _prefabs;
    private Dictionary<string, ItemInspect> _stats;
    [SerializeField] private InteractiveItemsConfiguration _interactiveItems;

    public Transform _parentFactory;

    public void Configure()
    {
        _interactiveItems = Instantiate(_interactiveItems);

        _prefabs = new Dictionary<string, GameObject>();
        _stats = new Dictionary<string, ItemInspect>();

        foreach (var item in _interactiveItems.NormalItems)
        {
            var currentItem = Instantiate(item.Prefab, _parentFactory);
            currentItem.gameObject.SetActive(false);
            _prefabs.Add(item.ItemID.ID, currentItem);
            _stats.Add(item.ItemID.ID, item);
        }

        foreach (var item in _interactiveItems.InteractiveItems)
        {
            var currentItem = Instantiate(item.Prefab, _parentFactory);
            currentItem.gameObject.SetActive(false);
            _prefabs.Add(item.ItemID.ID, currentItem);
            _stats.Add(item.ItemID.ID, item);
        }
    }

    public GameObject GetItemByID(string id)
    {
        return _prefabs[id];
    }

    public ItemInspect GetStatsByID(string id)
    {
        return _stats[id];
    }
}
