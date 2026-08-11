using System;
using Unity.Cinemachine;
using UnityEngine;

public class FixedPuzzle : Interactable
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] GameState _gameState;

    public override void StartInteraction()
    {
        _camera.Priority = 100;
        _gameState.ChangeState(GameStates.Puzzle);
    }

    public override void ExitInteraction()
    {
        _camera.Priority = 0;
        _gameState.ChangeState(GameStates.Gameplay);
    }
}
