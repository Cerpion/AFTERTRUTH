using Unity.Cinemachine;
using UnityEngine;

public class Altar : Interactable
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private string _tip;

    public override void StartInteraction()
    {
        _camera.Priority = 100;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);

        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract += ExitInteraction;

        Debug.LogWarning(_tip);

    }
    public override void ExitInteraction()
    {
        _camera.Priority = 0;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);

        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract -= ExitInteraction;
    }

   

}

