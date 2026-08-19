using Unity.Cinemachine;
using UnityEngine;

public class Altar : Interactable
{
    [SerializeField] private string _dialogue;
    public override bool ShowCursor => false;

    public override void OnInteractionStarted()
    {
        DialogueManager.Instance.Play(_dialogue);

    }
    public override void OnInteractionEnded()
    {
    }


}

