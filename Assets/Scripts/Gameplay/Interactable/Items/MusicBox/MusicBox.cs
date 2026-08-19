using System;
using Unity.Cinemachine;
using UnityEngine;

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
    [SerializeField] private MusicBoxAudioHandler _musicBoxAudioHandler;
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _key;
    [SerializeField] private GameObject _keyCollectable;
    [SerializeField] private ItemID _itemID;

    private StateMachine<MusicBoxState> _stateMachine;
    private Action OnExitInteraction;
    private Action OnDesactiveInteraction;

    public override bool ShowCursor => false;

    private void Start()
    {
        OnExitInteraction = StopInteraction;
        OnDesactiveInteraction = DesactiveExitInteraction;

        _stateMachine = new StateMachine<MusicBoxState>();

        _stateMachine.AddState(MusicBoxState.Closed, new MusicBoxClosed(_sequence, _musicBoxAudioHandler, OnExitInteraction, _key));
        _stateMachine.AddState(MusicBoxState.Opening, new MusicBoxOpening(_keyCollectable, _musicBoxAudioHandler, _animator, _itemID, OnDesactiveInteraction));
        _stateMachine.AddState(MusicBoxState.Open, new MusicBoxOpen(OnExitInteraction));

        _stateMachine.Initialize(MusicBoxState.Closed);
    }

    public override void OnInteractionStarted()
    {
        _stateMachine.EnterCurrentState();
    }
    public override void OnInteractionEnded()
    {
        _stateMachine.ExitCurrentState();
    }

}