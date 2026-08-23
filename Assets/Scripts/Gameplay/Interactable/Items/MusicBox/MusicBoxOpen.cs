using System;

public class MusicBoxOpen : State<MusicBoxState>
{
    private readonly Action OnExitInteraction;

    public MusicBoxOpen(Action onExitInteraction)
    {
        OnExitInteraction = onExitInteraction;
    }

    public override void OnEnter()
    {
        ServiceLocator.Instance.GetService<ItemsMovement>().ShowUI();
        ServiceLocator.Instance.GetService<ItemsMovement>().BackUI();

        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract += ExitInteraction;
    }

    public override void OnExit()
    {
        ServiceLocator.Instance.GetService<ItemsMovement>().HideUI();

        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract -= ExitInteraction;
    }

    public void ExitInteraction()
    {
        OnExitInteraction?.Invoke();
    }
}
