using System;
using System.Collections.Generic;
using UnityEngine;

public class InspectionSystem : MonoBehaviour
{
    private InputHandler _inputHandler;
    [SerializeField] private Transform _objectToInspect;
    [SerializeField] private float _rotationSpeed = 100f;

    [SerializeField] private bool canInspect;
    public Action OnInspectionFinished;

    private void Start()
    {
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();
    }

    public void StartInspect()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        canInspect = true;
        gameObject.SetActive(true);

        var input = ServiceLocator.Instance.GetService<InputHandler>();

        input.OnInteract += EndInspect;
        input.OnLeftMousePressed += TryInteract;

    }
    public void EndInspect()
    {
        Debug.Log("End Inspection");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameObject.SetActive(false);

        var input = ServiceLocator.Instance.GetService<InputHandler>();

        input.OnInteract -= EndInspect;
        input.OnLeftMousePressed -= TryInteract;

        OnInspectionFinished?.Invoke();
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
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out RaycastHit hit))
        //{
        //    // Code to interact with hit.collider.gameObject
        //}
    }
}

