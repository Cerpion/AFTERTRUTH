using System;
using Unity.Cinemachine;
using UnityEngine;

public class FixedPuzzle : Interactable
{
    [SerializeField] private CinemachineCamera _camera;

    public override void StartInteraction()
    {
        _camera.Priority = 100;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);
    }

    public override void ExitInteraction()
    {
        _camera.Priority = 0;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
    }
}
