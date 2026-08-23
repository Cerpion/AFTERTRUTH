using UnityEngine;

public enum GameStates
{
    Gameplay,
    Puzzle,
    Cinematic,
}

public class GameState : MonoBehaviour
{
    private StateMachine<GameStates> _stateMachine;
    [SerializeField] private Player _player;
    [SerializeField] private CanvasGroup _cinematic;
    [SerializeField] private CanvasGroup _inventory;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SpawnPlayer();

        _stateMachine = new StateMachine<GameStates>();
        _stateMachine.AddState(GameStates.Gameplay, new GamePlayState());
        _stateMachine.AddState(GameStates.Puzzle, new InteractionState(_player, _inventory));
        _stateMachine.AddState(GameStates.Cinematic, new CinematicState(_player, _cinematic, _inventory));
        _stateMachine.Initialize(GameStates.Gameplay);
    }

    public void ChangeState(GameStates newState)
    {
        _stateMachine.ChangeState(newState);
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn = GameObject.FindGameObjectWithTag("Respawn");

        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
        controller.enabled = true;
    }

}

public class GamePlayState : State<GameStates>
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }
}

public class InteractionState : State<GameStates>
{
    private readonly Player _player;
    private readonly CanvasGroup _inventory;

    public InteractionState(Player player, CanvasGroup inventory)
    {
        _player = player;
        _inventory = inventory;
    }

    public override void OnEnter()
    {
        _player._lockMovement = true;
        _player.StopInput();
        _inventory.LeanAlpha(0, 0.25f);
    }

    public override void OnExit()
    {
        _player._lockMovement = false;
        _player.StartInput();
        _inventory.LeanAlpha(1, 0.25f);
    }
}

public class CinematicState : State<GameStates>
{
    private readonly Player _player;
    private readonly CanvasGroup _cinematic;
    private readonly CanvasGroup _inventory;

    public CinematicState(Player player, CanvasGroup cinematic, CanvasGroup inventory)
    {
        _player = player;
        _cinematic = cinematic;
        _inventory = inventory;
    }

    public override void OnEnter()
    {
        _player._lockMovement = true;
        _player.StopInput();

        _cinematic.alpha = 0;
        _cinematic.gameObject.SetActive(true);
        _cinematic.LeanAlpha(1, 0.25f);

        _inventory.LeanAlpha(0, 0.25f);

    }

    public override void OnExit()
    {
        _player._lockMovement = false;
        _player.StartInput();

        _cinematic.LeanAlpha(0, 0.25f).setOnComplete(() => _cinematic.gameObject.SetActive(false));
        _inventory.LeanAlpha(1, 0.25f);

    }
}