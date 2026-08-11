using System.Collections.Generic;
using UnityEngine;

public class InspectionSystem : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private GameState _gameState;

    [SerializeField] private Transform _objectToInspect;
    [SerializeField] private float _rotationSpeed = 100f;

    [SerializeField] private bool canInspect;

    public void StartInspect()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        _gameState.ChangeState(GameStates.Puzzle);

        canInspect = true;
        gameObject.SetActive(true);

        _inputHandler.OnCancel += EndInspect;
        _inputHandler.OnLeftMousePressed += TryInteract;

    }
    public void EndInspect()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameObject.SetActive(false);
        _gameState.ChangeState(GameStates.Gameplay);
        _inputHandler.OnCancel -= EndInspect;
        _inputHandler.OnLeftMousePressed -= TryInteract;
    }

    private void Update()
    {
        if (!canInspect)
            return;

        if (!_inputHandler.LeftMouseIsPressed())
            return;

        var mouseDelta = _inputHandler.GetMouseMoveInput();

        var rotationX = mouseDelta.y * _rotationSpeed * Time.deltaTime;
        var rotationY = -mouseDelta.x * _rotationSpeed * Time.deltaTime;

        _objectToInspect.Rotate(rotationX, rotationY, 0f, Space.World);
    }

    private void TryInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Code to interact with hit.collider.gameObject
        }
    }
}

