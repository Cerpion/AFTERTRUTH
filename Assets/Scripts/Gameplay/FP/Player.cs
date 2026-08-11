using UnityEngine;
using UnityEngine.InputSystem;

//public class Inventory : MonoBehaviour
//{

//}

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private InteractionCheck _interactionCheck;
    [SerializeField] private InputHandler _inputHandler;

    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _normalSpeed = 2.6f;
    [SerializeField] private float _runSpeed = 3.6f;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Vector3 _movement;
    [SerializeField] private float _yVelocity;
    [SerializeField] private float _gravity;

    [SerializeField] private float _sensitivity;
    [SerializeField] private float _pitch;
    [SerializeField] private float _acceleration;

    public bool _lockMovement;

    private void Start()
    {
        _inputHandler.OnInteract += Interact;
        _inputHandler.OnSprintStart += Running;
        _inputHandler.OnSprintCanceled += Walk;

        _currentSpeed = _normalSpeed;
    }

    public void OnDestroy()
    {
        _inputHandler.OnInteract -= Interact;
        _inputHandler.OnSprintStart -= Running;
        _inputHandler.OnSprintCanceled -= Walk;

    }

    public void Interact()
    {
        _interactionCheck.Interactable?.StartInteraction();
    }

    public void Running()
    {
        _currentSpeed = _runSpeed;
    }
    public void Walk()
    {
        _currentSpeed = _normalSpeed;
    }

    void Update()
    {
        if (_lockMovement)
        {
            return;
        }

        var move = _inputHandler.GetMovementInput();

        var cameraForward = transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        var cameraRight = transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 targetMovement = cameraForward * move.y + cameraRight * move.x;
        targetMovement *= _currentSpeed;

        _movement = Vector3.Lerp( _movement, targetMovement, _acceleration * Time.deltaTime);

        //_movement = _camera.forward * move.y + _camera.right * move.x;
        //_movement *= _speed;

        if (_characterController.isGrounded && _yVelocity < 0f)
        {
            _yVelocity = -2f;
        }

        _yVelocity += _gravity * Time.deltaTime;
        _movement.y = _yVelocity;

        _characterController.Move(_movement  * Time.deltaTime);

        var look = _inputHandler.GetMouseMoveInput();

        transform.Rotate(Vector3.up, look.x * _sensitivity * Time.deltaTime);

        _pitch -= look.y * _sensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        _cameraTarget.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

        _interactionCheck.GetInteractableTarget();
    }

   
}
