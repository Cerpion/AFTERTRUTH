using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Image[] _itemImage;
    public void AddItem(int index, Sprite sprite)
    {
        _itemImage[index].sprite = sprite;
    }

    public void Remove(int index)
    {
        _itemImage[index].sprite = null;
    }
}
