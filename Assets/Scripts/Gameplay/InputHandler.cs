using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInputAction _inputs;
    public Action OnInteract;
    public Action OnCancel;
    public Action OnLeftMousePressed;
    public Action OnSprintStart;
    public Action OnSprintCanceled;

    public Action OnLeftButton;
    public Action OnRightButton;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _inputs = new PlayerInputAction();

        _inputs.Player.Interact.performed += Interact;
        _inputs.Player.Cancel.performed += Cancel;
        _inputs.Player.Sprint.performed += SprintStart;
        _inputs.Player.Sprint.canceled += SprintCanceled;
        _inputs.Player.InteractMouse.performed += LeftMousePressed;

        _inputs.Player.Move.performed += MovePressed;

        _inputs.Enable();
    }



    private void OnDestroy()
    {
        _inputs.Player.Interact.performed -= Interact;
        _inputs.Player.Cancel.performed -= Cancel;
    }

    private void MovePressed(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<Vector2>().x > 0)
        {
            OnRightButton?.Invoke();
            return;
        }

        if (obj.ReadValue<Vector2>().x < 0)
        {
            OnLeftButton?.Invoke();
            return;
        }
    }
    private void LeftMousePressed(InputAction.CallbackContext obj)
    {
        OnLeftMousePressed?.Invoke();
    }
 

    public void SprintStart(InputAction.CallbackContext ctx)
    {
        OnSprintStart?.Invoke();
    }

    public void SprintCanceled(InputAction.CallbackContext ctx)
    {
        OnSprintCanceled?.Invoke();
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }

    public void Cancel(InputAction.CallbackContext ctx)
    {
        OnCancel?.Invoke();
    }

    public Vector2 GetMovementInput()
    {
        return _inputs.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseMoveInput()
    {
        return _inputs.Player.Look.ReadValue<Vector2>();
    }

    public bool LeftMouseIsPressed()
    {
        return _inputs.Player.InteractMouse.IsPressed();
    }
}
