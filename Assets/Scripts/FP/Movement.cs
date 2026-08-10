using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Movement : MonoBehaviour
{
    [SerializeField] private PlayerInputAction _inputs;
    [SerializeField] private CharacterController _characterController;

    [SerializeField] private float _speed;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Vector3 _movement;
    [SerializeField] private float _yVelocity;
    [SerializeField] private float _gravity;

    [SerializeField] private float _sensitivity;
    [SerializeField] private float _pitch;
    [SerializeField] private float _acceleration;

    void Awake()
    {
        _inputs = new PlayerInputAction();
        _inputs.Enable();
    }

    void Update()
    {
        var move = _inputs.Player.Move.ReadValue<Vector2>();

        var cameraForward = transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        var cameraRight = transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 targetMovement = cameraForward * move.y + cameraRight * move.x;
        targetMovement *= _speed;

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

        var look = _inputs.Player.Look.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, look.x * _sensitivity * Time.deltaTime);

        _pitch -= look.y * _sensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        _cameraTarget.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}
