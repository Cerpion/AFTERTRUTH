using System;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectExamination : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private GameState _gameState;

    [SerializeField] private Transform _objectToInspect;
    [SerializeField] private float _rotationSpeed = 100f;

    private void Awake()
    {
        _inputHandler.OnCancel += HideInpection;
    }

    public void HideInpection()
    {
        gameObject.SetActive(false);
        _gameState.ChangeState(GameStates.Gameplay);
    }

    private void Update()
    {
        if (!_inputHandler.LeftMouseIsPressed())
            return;

        Vector2 mouseDelta = _inputHandler.GetMouseMoveInput();

        float rotationX = mouseDelta.y * _rotationSpeed * Time.deltaTime;
        float rotationY = -mouseDelta.x * _rotationSpeed * Time.deltaTime;

        _objectToInspect.Rotate(rotationX, rotationY, 0f, Space.World);
    }
}

