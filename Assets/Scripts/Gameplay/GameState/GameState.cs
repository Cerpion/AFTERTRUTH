using UnityEngine;

public enum GameStates
{
    Gameplay,
    Puzzle,
}

public class GameState : MonoBehaviour
{
    private StateMachine<GameStates> _stateMachine;
    [SerializeField] private Player _player;

    private void Awake()
    {
        _stateMachine = new StateMachine<GameStates>();
        _stateMachine.AddState(GameStates.Gameplay, new GamePlayState());
        _stateMachine.AddState(GameStates.Puzzle, new InteractionState(_player));
        _stateMachine.Initialize(GameStates.Gameplay);
    }

    public void ChangeState(GameStates newState)
    {
        _stateMachine.ChangeState(newState);
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
    }

    public override void OnExit()
    {
        _player._lockMovement = false;
    }
}