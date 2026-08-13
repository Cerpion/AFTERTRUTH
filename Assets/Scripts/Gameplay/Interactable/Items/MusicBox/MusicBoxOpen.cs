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
        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract += ExitInteraction;
    }

    public override void OnExit()
    {
        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract -= ExitInteraction;
    }

    public void ExitInteraction()
    {
        OnExitInteraction?.Invoke();
    }
}
