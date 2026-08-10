using UnityEngine;

public class ObjectExamination : MonoBehaviour
{
    [SerializeField] private PlayerInputAction _inputs;
    [SerializeField] private Transform _objectToInspect;
    [SerializeField] private float _rotationSpeed = 100f;

    private void Awake()
    {
        _inputs = new PlayerInputAction();
        _inputs.Enable();
    }

    private void Update()
    {
        if (!_inputs.Player.Interact.IsPressed())
            return;

        Vector2 mouseDelta = _inputs.Player.Look.ReadValue<Vector2>();

        float rotationX = mouseDelta.y * _rotationSpeed * Time.deltaTime;
        float rotationY = -mouseDelta.x * _rotationSpeed * Time.deltaTime;

        _objectToInspect.Rotate(rotationX, rotationY, 0f, Space.World);
    }
}
