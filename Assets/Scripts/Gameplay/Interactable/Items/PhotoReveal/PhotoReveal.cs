using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PhotoReveal : Interactable
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private ItemID _requiredItem;
    [SerializeField] private ItemID _itemToInspect;
    [SerializeField] private GameObject _photo;
    [SerializeField] private float _duration = 2;
    [SerializeField] private string _tip;
    [SerializeField] private InteractableObject _interactable;

    private void Start()
    {
        _photo.GetComponent<MeshRenderer>().material.color = Color.black;
        _photo.SetActive(false);
    }

    public override void StartInteraction()
    {
        if (!ContainItem())
        {
            return;
        }

        _camera.Priority = 100;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);
        _photo.SetActive(true);

        StartCoroutine(ChangeColor());
    }
    public override void ExitInteraction()
    {
        _camera.Priority = 0;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
        _interactable.SetItemsRequired(null);

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= ExitInteraction;
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

        yield return new WaitForSeconds(duration);

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect(_itemToInspect.ID);
        inspection.OnInspectionFinished += ExitInteraction;
    }

    public bool ContainItem()
    {
        var player = ServiceLocator.Instance.GetService<Player>();
        if (!player.Inventory.ContainItem(_requiredItem))
        {
            Debug.Log(_tip);
            return false;
        }

        player.Inventory.TryRemove(_requiredItem);
        RemoveInteractable();
        return true;
    }

}
