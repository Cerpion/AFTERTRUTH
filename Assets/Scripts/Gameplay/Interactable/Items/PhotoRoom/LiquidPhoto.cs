using System;
using UnityEngine;

public interface DragDrop
{
    public void Select();
    public void UnSelect();
    public void Drag(Vector3 mouseWorldPosition);
    public void Drop();
    public void ReturnToOriginalPosition();
}

public class LiquidPhoto : MonoBehaviour, DragDrop
{
    public Liquids LiquidType;
    [SerializeField] private float _yOffset = 0.5f;

    private Vector3 _originalPosition;

    public void Start()
    {
        _originalPosition = transform.position;
    }

    public void Select()
    {
        transform.LeanScale(Vector3.one * 1.1f, 0.25f);
    }

    public void UnSelect()
    {
        transform.LeanScale(Vector3.one , 0.25f);
    }

    public void Drag(Vector3 mouseWorldPosition)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.position = new Vector3(mouseWorldPosition.x,_originalPosition.y + _yOffset,mouseWorldPosition.z);
    }

    public void Drop()
    {
        gameObject.SetActive(false);
    }

    public void ReturnToOriginalPosition()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        transform.position = _originalPosition;
    }
}
