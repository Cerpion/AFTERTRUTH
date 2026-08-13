using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectionSystem : MonoBehaviour
{
    private InputHandler _inputHandler;
    [SerializeField] private InteractiveItemFactory _interactiveItemFactory;
    public InteractiveItemFactory InteractiveItemFactory { get => _interactiveItemFactory; }

    [SerializeField] private Camera _camera;

    [SerializeField] private Transform _objectToInspect;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private LayerMask _layerMask;

    private bool canInspect;

    public Action OnItemObtained;
    public Action OnInspectionFinished;

    private GameObject _currentItem;
    private InspectionCollectable _currentInspectionCollectable;

    private void Start()
    {
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();
        _interactiveItemFactory.Configure();
    }

    public void StartInspect(string itemID)
    {
        _objectToInspect.rotation = Quaternion.identity;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        canInspect = true;
        _camera.gameObject.SetActive(true);

        var input = ServiceLocator.Instance.GetService<InputHandler>();

        input.OnInteract += EndInspect;
        input.OnLeftMousePressed += TryInteract;

        _currentItem = _interactiveItemFactory.GetItemByID(itemID);
        _currentItem.gameObject.SetActive(true);
    }

    public void EndInspect()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _camera.gameObject.SetActive(false);

        RemoveCurrentInspection();

        OnInspectionFinished?.Invoke();
    }

    public void RemoveCurrentInspection()
    {
        var input = ServiceLocator.Instance.GetService<InputHandler>();

        input.OnInteract -= EndInspect;
        input.OnLeftMousePressed -= TryInteract;

        _currentItem.gameObject.SetActive(false);
        _currentItem = null;
    }

    private void Update()
    {
        if (!canInspect)
            return;

        Raycast();


        if (!_inputHandler.LeftMouseIsPressed())
            return;

        var mouseDelta = _inputHandler.GetMouseMoveInput();

        var rotationX = mouseDelta.y * _rotationSpeed * Time.deltaTime;
        var rotationY = -mouseDelta.x * _rotationSpeed * Time.deltaTime;

        _objectToInspect.Rotate(rotationX, rotationY, 0f, Space.World);
    }

    private void Raycast()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = _camera.ScreenPointToRay(mousePosition);
        InspectionCollectable target = null;

        if (Physics.Raycast(ray, out RaycastHit hit, _layerMask))
        {
            target = hit.collider.GetComponent<InspectionCollectable>();
        }

        if (target != _currentInspectionCollectable)
        {
            _currentInspectionCollectable?.UnSelect();
            _currentInspectionCollectable = target;
            _currentInspectionCollectable?.Select();
        }
    }

    private void TryInteract()
    {
        if (_currentInspectionCollectable != null)
        {
            _currentInspectionCollectable.Obtained();
            _currentInspectionCollectable = null;
        }
    }
}
