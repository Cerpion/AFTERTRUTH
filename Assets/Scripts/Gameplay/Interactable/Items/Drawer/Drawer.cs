using UnityEngine;

public class Drawer : Interactable
{
    private bool _open;
    [SerializeField] private Transform _drawer;
    [SerializeField] private Collectable[] _collectable;

    private void Start()
    {
        ActivateItems(false);
    }

    public override void StartInteraction()
    {
        _open = !_open;

        if (_open)
        {
            Open();
            return;
        }

        Close();
    }

    private void Open()
    {
        LeanTween.cancel(_drawer.gameObject);
        _drawer.LeanMoveLocalX(0.5f, 0.5f).setOnComplete(() => { ActivateItems(true); });
    }

    private void Close()
    {
        LeanTween.cancel(_drawer.gameObject);
        ActivateItems(false);
        _drawer.LeanMoveLocalX(0, 0.5f);
    }

    private void ActivateItems(bool activate)
    {
        foreach (var item in _collectable)
        {
            item.GetComponent<BoxCollider>().enabled = activate;
        }
    }
}
