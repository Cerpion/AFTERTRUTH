using System;
using Unity.Cinemachine;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum MusicBoxDirections
{
    Left,
    Right
}

public enum MusicBoxState
{
    Closed,
    Opening,
    Open,
}

public class MusicBox : Interactable
{
    [Header("Solution")]
    [SerializeField] private MusicBoxDirections[] _sequence;

    [Header("Configuration")]
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private MusicBoxAudioHandler _musicBoxAudioHandler;
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _key;
    [SerializeField] private GameObject _keyCollectable;
    [SerializeField] private ItemID _itemID;

    private StateMachine<MusicBoxState> _stateMachine;
    private Action OnExitInteraction;

    private void Start()
    {
        OnExitInteraction = ExitInteraction;

        _stateMachine = new StateMachine<MusicBoxState>();

        _stateMachine.AddState(MusicBoxState.Closed, new MusicBoxClosed(_sequence, _musicBoxAudioHandler, OnExitInteraction, _key));
        _stateMachine.AddState(MusicBoxState.Opening, new MusicBoxOpening(_keyCollectable, _musicBoxAudioHandler, _animator, _itemID));
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