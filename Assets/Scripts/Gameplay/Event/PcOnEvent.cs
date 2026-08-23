using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class PcOnEvent : EventCinematic
{
    [SerializeField] string _introContext;

    public override void Execute()
    {
        DialogueManager.Instance.Play(_introContext);
        DisableEvent();
    }
}
