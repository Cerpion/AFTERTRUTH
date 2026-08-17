using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PhotoDeveloperState
{
    Empty,
    FillingLiquids,
    Developing,
    Completed,
}

public enum Liquids
{
    Developer,
    StopBath,
    Fixer
}

public class PhotoDeveloper : Interactable
{
    [Header("Unlock minigame")]
    [SerializeField] private ItemID _photoRequired;
    [SerializeField] private string _photoDialogue;
    [SerializeField] private ItemID _liquidsRequired;
    [SerializeField] private string _liquidDialogue;
    private bool _unlock;

    [Header("FillLiquids")]
    [SerializeField] private LiquidPhoto[] _liquids;
    [SerializeField] private LayerMask _liquidLayer;

    [Header("Photo")]
    [SerializeField] private SlotLiquidPhoto[] _slots;
    [SerializeField] private ItemID _itemReward;
    [SerializeField] private Photo _photo;

    [SerializeField] private CinemachineCamera _camera;
    private StateMachine<PhotoDeveloperState> _stateMachine;
    private Action OnExitInteraction;
    private bool _activateUpdate;

    private void Start()
    {
        OnExitInteraction = ExitInteraction;

        _stateMachine = new StateMachine<PhotoDeveloperState>();

        _stateMachine.AddState(PhotoDeveloperState.FillingLiquids, new FillingLiquidsState(OnExitInteraction, _liquidLayer, _liquids));
        _stateMachine.AddState(PhotoDeveloperState.Developing, new DevelopingState(OnExitInteraction, _liquidLayer, _slots, _photo));
        _stateMachine.AddState(PhotoDeveloperState.Completed, new CompletedState(OnExitInteraction, _itemReward, _photo));

        _stateMachine.Initialize(PhotoDeveloperState.FillingLiquids);

        _photo.gameObject.SetActive(false);
        foreach (var item in _liquids)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (!_activateUpdate)
            return;

        _stateMachine.Update(Time.deltaTime);
    }

    public bool LockPhotoDeveloper()
    {
        if (_unlock)
        {
            return true;
        }

        var player = ServiceLocator.Instance.GetService<Player>();


        if (!player.Inventory.ContainItem(_photoRequired))
        {
            DialogueManager.Instance.Play(_photoDialogue);

            return false;
        }

        if (!player.Inventory.ContainItem(_liquidsRequired))
        {
            DialogueManager.Instance.Play(_liquidDialogue);

            return false;
        }

        player.Inventory.TryRemove(_photoRequired);
        player.Inventory.TryRemove(_liquidsRequired);

        _photo.gameObject.SetActive(true);
        foreach (var item in _liquids)
        {
            item.gameObject.SetActive(true);
        }

        _unlock = true;

        return true;
    }




    public override void StartInteraction()
    {

        if (!LockPhotoDeveloper())
        {
            return;
        }


        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
 
        _camera.Priority = 100;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);
        _stateMachine.EnterCurrentState();

        _activateUpdate = true;
    }
    public override void ExitInteraction()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _stateMachine.ExitCurrentState();
        _camera.Priority = 0;
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);

        _activateUpdate = false;
    }
}

public class DevelopingState : State<PhotoDeveloperState>
{
    private readonly Action OnExitInteraction;
    private readonly LayerMask _liquidLayer;
    private readonly Photo _photoGo;

    private readonly SlotLiquidPhoto[] _slots;

    private Photo _photo;
    private Camera _camera;
    private bool _isDragging;
    private int _completedSlots;

    public DevelopingState(Action onExitInteraction, LayerMask liquidLayer, SlotLiquidPhoto[] slots, Photo photo)
    {
        OnExitInteraction = onExitInteraction;
        _liquidLayer = liquidLayer;
        _slots = slots;
        _camera = Camera.main;
        _photoGo = photo;
    }

    public override void OnEnter()
    {
        _isDragging = false;
        _photo = null;
        _completedSlots = 0;

        _photoGo.gameObject.SetActive(true);
        _photoGo.ResetPhoto();

        foreach (var slot in _slots)
        {
            slot.ResetSlot();
        }

        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract += ExitInteraction;
    }

    public override void OnExit()
    {
        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract -= ExitInteraction;
    }

    public void ExitInteraction()
    {
        OnExitInteraction?.Invoke();
    }

    public override void OnUpdate(float delta)
    {
        UpdateSlots(delta);

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (_isDragging)
        {
            UpdateDrag(ray);

            if (Mouse.current.leftButton.wasReleasedThisFrame)
                Drop();

            return;
        }

        UpdateHover(ray);

        if (_photo != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            TakePhoto();
        }
    }

    private void UpdateSlots(float delta)
    {
        foreach (var slot in _slots)
        {
            if (slot.UpdateProcess(delta))
            {
                //Debug.Log("RECETA DAÑADA");
                ExitInteraction();
                return;
            }
        }
    }

    private void UpdateHover(Ray ray)
    {
        Photo target = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _liquidLayer))
            target = hit.collider.GetComponent<Photo>();

        if (target == _photo)
            return;

        _photo?.UnSelect();
        _photo = target;
        _photo?.Select();
    }

    private void TakePhoto()
    {
        if (_photo.CurrentSlot != null)
        {
            if (!_photo.CurrentSlot.TryComplete())
                return;

            _photo.CurrentSlot.Hide();

            _photo.ChangeLiquidType(GetNextLiquid());
            _photo.CurrentSlot.Remove();
            _photo.RemoveFromSlot();


            _completedSlots++;

            if (_completedSlots >= 3)
            {
                //Debug.Log("VICTORIA");
                ChangeState(PhotoDeveloperState.Completed);
                return;
            }
        }

        _isDragging = true;
    }

    private void UpdateDrag(Ray ray)
    {
        Plane plane = new Plane(
            Vector3.up,
            _photo.transform.position
        );

        if (!plane.Raycast(ray, out float distance))
            return;

        _photo.Drag(ray.GetPoint(distance));
    }

    private void Drop()
    {
        _isDragging = false;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        SlotLiquidPhoto slot = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _liquidLayer))
            slot = hit.collider.GetComponent<SlotLiquidPhoto>();


        if (slot == null || !slot.CanReceive(_photo))
        {
            _photo.ReturnToOriginalPosition();
            _photo.Drop();
            return;
        }

        _photo.SetInSlot(slot);
        _photo.Drop();
        slot.Insert(_photo);
        slot.Show();
    }

    private Liquids GetNextLiquid()
    {
        return _photo.LiquidType switch
        {
            Liquids.Developer => Liquids.StopBath,
            Liquids.StopBath => Liquids.Fixer,
            _ => Liquids.Fixer
        };
    }
}

public class CompletedState : State<PhotoDeveloperState>
{
    private readonly Action OnExitInteraction;
    private readonly ItemID _item;
    private readonly Photo _photo;

    public CompletedState(Action onExitInteraction, ItemID item, Photo photo)
    {
        OnExitInteraction = onExitInteraction;
        _item = item;
        _photo = photo;
    }

    public override void OnEnter()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished += FinishInspection;
        inspection.StartInspect(_item.ID);
    }

    public override void OnExit()
    {
        _photo.gameObject.SetActive(false);
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= FinishInspection;
    }

    public void FinishInspection()
    {
        ServiceLocator.Instance.GetService<Player>().Inventory.TryAdd(_item);
        ExitInteraction();
    }

    public void ExitInteraction()
    {
        OnExitInteraction?.Invoke();
    }

}


public class FillingLiquidsState : State<PhotoDeveloperState>
{
    private readonly Action OnExitInteraction;
    private readonly LayerMask _liquidLayer;
    private readonly LiquidPhoto[] _liquids;
    private Camera _camera;
    private LiquidPhoto _currentLiquid;
    private bool _isDragging;
    private int _points;

    public FillingLiquidsState(Action onExitInteraction, LayerMask liquidLayer, LiquidPhoto[] liquids)
    {
        OnExitInteraction = onExitInteraction;
        _camera = Camera.main;
        _liquidLayer= liquidLayer;
        _liquids = liquids;
    }

    public override void OnEnter()
    {
        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract += ExitInteraction;
    }

    public override void OnExit()
    {
        if (_points < 3)
        {
            foreach (var liquid in _liquids)
            {
                liquid.gameObject.SetActive(true);
                liquid.ReturnToOriginalPosition();
            }

            _points = 0;
        }

        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnInteract -= ExitInteraction;
    }

    public void ExitInteraction()
    {
        OnExitInteraction?.Invoke();
    }

    public override void OnUpdate(float delta)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (_isDragging)
        {
            UpdateDrag(ray);

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Drop();
                return;
            }

            return;
        }

        UpdateHover(ray);

        if (_currentLiquid != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isDragging = true;
            _currentLiquid.Select();
        }
    }

    private void UpdateHover(Ray ray)
    {
        LiquidPhoto target = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _liquidLayer))
        {
            Debug.Log(hit.collider.name);
            target = hit.collider.GetComponent<LiquidPhoto>();
        }

        if (target == _currentLiquid)
        {
            return;
        }

        _currentLiquid?.UnSelect();
        _currentLiquid = target;
        _currentLiquid?.Select();
    }

    private void UpdateDrag(Ray ray)
    {
        Plane plane = new Plane(Vector3.up, _currentLiquid.transform.position);

        if (!plane.Raycast(ray, out float distance))
            return;

        Vector3 worldPosition = ray.GetPoint(distance);

        _currentLiquid.Drag(worldPosition);
    }

    private void Drop()
    {
        _isDragging = false;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        SlotLiquidPhoto slot = null;

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue, 2f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _liquidLayer))
        {
            slot = hit.collider.GetComponent<SlotLiquidPhoto>();
        }

        if (slot == null)
        {
            _currentLiquid.ReturnToOriginalPosition();
            return;
        }

        if (slot.SlotLiquidType != _currentLiquid.LiquidType)
        {
            _currentLiquid.ReturnToOriginalPosition();
            ExitInteraction();
            return;
        }

        _points++;
        slot.FillLiquidType();
        _currentLiquid.Drop();
        _currentLiquid = null;

        if (_points >= _liquids.Length)
        {
            ChangeState.Invoke(PhotoDeveloperState.Developing);
            return;
        }
    }
}
