using System.Collections.Generic;
using UnityEngine;

public class InspectionSystem : MonoBehaviour
{
    [SerializeField] Transform _inspectPivot;
    [SerializeField] List<GameObject> _itemsToSpawn;
    Dictionary<string, GameObject> _idToItem;

    [SerializeField] GameObject _currentItem;
    [SerializeField] GameState _gameState;


    public void SpawnItems()
    {
        _idToItem = new Dictionary<string, GameObject>();

        foreach (var item in _itemsToSpawn)
        {
            _idToItem.Add(item.name, item);
        }
    }

    public void StartInspect(string itemID)
    {
        _gameState.ChangeState(GameStates.Puzzle);
        gameObject.SetActive(true);
        //_currentItem = _idToItem[itemID];
        //_currentItem.SetActive(true);
    }

}

