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
        DialogueManager.Instance.Play(_dialogue);
        OnInteracted?.Invoke();
    }

    public override void OnInteractionEnded()
    {
    }


}

