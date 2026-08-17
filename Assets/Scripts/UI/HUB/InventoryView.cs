using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Image[] _itemImage;
    public void UpdateView (int index, Sprite sprite)
    {
        _itemImage[index].sprite = sprite;
    }

    public void Reset()
    {
        foreach (var item in _itemImage)
        {
            item.sprite = null;
        } 
    }
}
