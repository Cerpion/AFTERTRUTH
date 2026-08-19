using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PhotoReveal : Interactable
{
    [SerializeField] private ItemID _requiredItem;
    [SerializeField] private ItemID _itemToInspect;
    [SerializeField] private GameObject _photo;
    [SerializeField] private float _duration = 2;
    [SerializeField] private string _dialogLock;
    [SerializeField] private string _dialogReveal;
    [SerializeField] private InteractableObject _interactable;
    public override bool ShowCursor => false;

    private void Start()
    {
        _photo.GetComponent<MeshRenderer>().material.color = Color.black;
        _photo.SetActive(false);
    }

    public override void OnInteractionStarted()
    {
        if (!ContainItem())
        {
            return;
        }

        _photo.SetActive(true);

        StartCoroutine(ChangeColor());
    }
    public override void OnInteractionEnded()
    {
        _interactable.SetItemsRequired(null);

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= StopInteraction;
    }

    private IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(1.5f);

        float time = 0;
        float duration = _duration;

        while (time < _duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            _photo.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.black, Color.white, t);
            yield return null;
        }

        DialogueManager.Instance.Play(_dialogReveal);
        yield return new WaitForSeconds(duration);

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect(_itemToInspect.ID);
        inspection.OnInspectionFinished += StopInteraction;
    }

    public bool ContainItem()
    {
        var player = ServiceLocator.Instance.GetService<Player>();
        if (!player.Inventory.ContainItem(_requiredItem))
        {
            DialogueManager.Instance.Play(_dialogLock);
            return false;
        }

        player.Inventory.TryRemove(_requiredItem);
        RemoveInteractable();
        return true;
    }

}
