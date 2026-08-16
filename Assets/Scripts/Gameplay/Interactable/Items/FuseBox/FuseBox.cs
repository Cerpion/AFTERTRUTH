using Unity.Cinemachine;
using UnityEngine;

public class FuseBox : Interactable
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private FlowGame _flowGame;
    private bool _canUpdate;

    public override void StartInteraction()
    {
        _canUpdate = true;

        _camera.Priority = 100;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract += ExitInteraction;
    }

    public override void ExitInteraction()
    {
        _canUpdate = false;
         
        _camera.Priority = 0;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract -= ExitInteraction;
    }

    private void Update()
    {
        if (!_canUpdate)
        {
            return;
        }

        _flowGame.UpdateFlowGame();
    }
}
