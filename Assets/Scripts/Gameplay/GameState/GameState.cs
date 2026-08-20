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

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SpawnPlayer();

        _stateMachine = new StateMachine<GameStates>();
        _stateMachine.AddState(GameStates.Gameplay, new GamePlayState());
        _stateMachine.AddState(GameStates.Puzzle, new InteractionState(_player));
        _stateMachine.AddState(GameStates.Cinematic, new InteractionState(_player));
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
    public InteractionState(Player player)
    {
        _player = player;
    }

    public override void OnEnter()
    {
        _player._lockMovement = true;
        _player.StopInput();
    }

    public override void OnExit()
    {
        _player._lockMovement = false;
        _player.StartInput();
    }
}

public class CinematicState : State<GameStates>
{
    private readonly Player _player;
    public CinematicState(Player player)
    {
        _player = player;
    }

    public override void OnEnter()
    {
        _player._lockMovement = true;
        _player.StopInput();
    }

    public override void OnExit()
    {
        _player._lockMovement = false;
        _player.StartInput();
    }
}