using UnityEngine;

public class Photo : MonoBehaviour
{
    public Liquids LiquidType { get; private set; }

    [SerializeField] private Liquids _initialLiquidType;
    [SerializeField] private float _slotOffset = 0.1f;

    public SlotLiquidPhoto CurrentSlot { get; private set; }

    private Vector3 _originalPosition;

    private void Awake()
    {
        LiquidType = _initialLiquidType;
        _originalPosition = transform.position;
    }

    public void Select()
    {
        transform.LeanScale(Vector3.one * 1.1f, 0.25f);
    }

    public void UnSelect()
    {
        transform.LeanScale(Vector3.one, 0.25f);
    }

    public void Drag(Vector3 position)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.position = new Vector3(position.x, _originalPosition.y, position.z);
    }

    public void Drop()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void SetInSlot(SlotLiquidPhoto slot)
    {
        CurrentSlot = slot;

        transform.position = slot.transform.position;
        transform.position += Vector3.up * _slotOffset;
    }

    public void RemoveFromSlot()
    {
        CurrentSlot = null;
    }

    public void ChangeLiquidType(Liquids next)
    {
        LiquidType = next;
    }

    public void ReturnToOriginalPosition()
    {
        transform.position = _originalPosition;
    }
}