using Unity.Cinemachine;
using UnityEngine;

public class FuseBox : Interactable
{
    [SerializeField] private GlobalLightHandler _globalLightHandler;
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private FlowGame _flowGame;

    [SerializeField] private GameObject _normalDoor;
    [SerializeField] private GameObject _lockedDoor;
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
        _flowGame.Finish += OnLight;
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
        _flowGame.Finish -= OnLight;

    }

    public void OnLight()
    {
        ExitInteraction();
        _globalLightHandler.SetDay();
        var player = ServiceLocator.Instance.GetService<Player>();
        player.OffLight();

        _normalDoor.SetActive(true);
        _lockedDoor.SetActive(false);

        gameObject.GetComponent<BoxCollider>().enabled = false;
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
