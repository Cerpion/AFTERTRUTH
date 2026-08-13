using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Windows;

public enum MusicBoxDirections
{
    Left, Right
}

public enum MusicBoxState
{
    Closed,
    Opening,
    Open,
}

public class MusicBoxClosed : State<MusicBoxState>
{
    private readonly MusicBoxDirections[] _sequence;
    private readonly MusicBoxAudioHandler _musicBoxAudio;
    private readonly Action OnExitInteraction;
    private readonly Transform _key;

    private int _index;
    private bool _lockInput;


    public MusicBoxClosed(MusicBoxDirections[] sequence, MusicBoxAudioHandler musicBoxAudio, Action onExitInteraction, Transform key)
    {
        _sequence = sequence;
        _musicBoxAudio = musicBoxAudio;
        OnExitInteraction = onExitInteraction;
        _key = key;
    }

    public override void OnEnter()
    {
        var input = ServiceLocator.Instance.GetService<InputHandler>();

        input.OnLeftButton += MoveLeft;
        input.OnRightButton += MoveRight;
        input.OnInteract += ExitInteraction;
    }

    public override void OnExit()
    {
        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnLeftButton -= MoveLeft;
        input.OnRightButton -= MoveRight;
        input.OnInteract -= ExitInteraction;

        Reset();
    }

    public void ExitInteraction()
    {
        OnExitInteraction?.Invoke();
    }

    private void MoveLeft()
    {
        if (_lockInput)
            return;

        VerifySequence(MusicBoxDirections.Left);
    }

    private void MoveRight()
    {
        if (_lockInput)
            return;

        VerifySequence(MusicBoxDirections.Right);
    }

    private void VerifySequence(MusicBoxDirections direction)
    {
        _lockInput = true;

        if (_sequence[_index] == direction)
        {
            _index++;
            AnimKey(direction);
            Debug.Log("god");

            if (_index >= _sequence.Length)
            {
                Debug.Log("Finish");
                ChangeState?.Invoke(MusicBoxState.Opening);
                return;
            }

            return;
        }

        AnimKey(direction);
        Debug.Log("bad");
        _index = 0;
    }

    private void AnimKey(MusicBoxDirections direction)
    {
        _musicBoxAudio.PlayRotate();
        float rotation = direction == MusicBoxDirections.Left ? 90f : -90f;
        var sequence = LeanTween.sequence();
        sequence.append(_key.LeanRotateAround(Vector3.forward, rotation, 0.3f));
        sequence.append(() => { _lockInput = false; });
    }

    private void Reset()
    {
        _index = 0;
        _key.LeanRotateX(0, 0.3f);
        _musicBoxAudio.PlayRotate();
    }
}

public class MusicBoxOpening : State<MusicBoxState>
{
    private readonly GameObject _keyItem;
    private readonly MusicBoxAudioHandler _musicBoxAudio;

    public MusicBoxOpening(GameObject keyItem, MusicBoxAudioHandler musicBoxAudio)
    {
        _keyItem = keyItem;
        _musicBoxAudio = musicBoxAudio;
    }

    public override void OnEnter()
    {
        _musicBoxAudio.PlayOpen();
        _keyItem.gameObject.SetActive(true);

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.StartInspect();
        inspection.OnInspectionFinished += () => { ChangeState?.Invoke(MusicBoxState.Open); };
    }

}

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

public class MusicBox : Interactable
{
    [SerializeField] private MusicBoxAudioHandler _musicBoxAudioHandler;
    [SerializeField] private InspectionSystem _inspectionSystem;
    [SerializeField] private MusicBoxDirections[] _sequence;

    [SerializeField] private  Transform _key;

    [Header("Configuration")]
    [SerializeField] private CinemachineCamera _camera;

    [SerializeField] private StateMachine<MusicBoxState> _stateMachine;
    private Action OnExitInteraction;

    private void Start()
    {
        OnExitInteraction = ExitInteraction;

        _stateMachine = new StateMachine<MusicBoxState>();

        _stateMachine.AddState(MusicBoxState.Closed, new MusicBoxClosed(_sequence, _musicBoxAudioHandler, OnExitInteraction, _key));
        _stateMachine.AddState(MusicBoxState.Opening, new MusicBoxOpening(_key.gameObject, _musicBoxAudioHandler));
        _stateMachine.AddState(MusicBoxState.Open, new MusicBoxOpen(OnExitInteraction));

        _stateMachine.Initialize(MusicBoxState.Closed);
    }

    public override void StartInteraction()
    {
        _camera.Priority = 100;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);
        _stateMachine.EnterCurrentState();
    }
    public override void ExitInteraction()
    {
        _stateMachine.ExitCurrentState();
        _camera.Priority = 0;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
    }

}
