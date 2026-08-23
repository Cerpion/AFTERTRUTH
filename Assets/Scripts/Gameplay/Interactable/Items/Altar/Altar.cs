using System;
using Unity.Cinemachine;
using UnityEngine;

public class Altar : Interactable
{
    [SerializeField] private string _dialogue;
    public override bool ShowCursor => false;
    public Action OnInteracted;

    public override void OnInteractionStarted()
    {

        ServiceLocator.Instance.GetService<ItemsMovement>().ShowUI();
        ServiceLocator.Instance.GetService<ItemsMovement>().BackUI();

        DialogueManager.Instance.Play(_dialogue);
        OnInteracted?.Invoke();
    }

    public override void OnInteractionEnded()
    {
        ServiceLocator.Instance.GetService<ItemsMovement>().HideUI();
    }


}

