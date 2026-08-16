using UnityEngine;

public class SlotLiquidPhoto : MonoBehaviour
{
    public Liquids SlotLiquidType;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
    }

    public void FillLiquidType()
    {
        // visual fill slot
    }

    [SerializeField] private float _processTime = 5f;
    [SerializeField] private float _errorTime = 0.5f;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _greenBar;
    [SerializeField] private RectTransform _redBar;

    private float _timer;
    private Photo _photo;
    private bool _processing;

    public bool IsReady => _timer >= _processTime;
    public bool IsProcessing => _processing;

    public bool CanReceive(Photo photo)
    {
        return !_processing &&
               photo.LiquidType == SlotLiquidType;
    }

    public void Insert(Photo photo)
    {
        _photo = photo;
        _timer = 0f;
        _processing = true;
    }

    public bool UpdateProcess(float delta)
    {
        if (!_processing)
            return false;

        _timer += delta;
        UpdateBars();

        if (_timer >= _processTime + _errorTime)
            return true;

        return false;
    }

    private void UpdateBars()
    {
        float greenProgress = Mathf.Clamp01(_timer / _processTime);
        float redProgress = Mathf.Clamp01((_timer - _processTime) / _errorTime);
        _greenBar.localScale = new Vector3(greenProgress,1f,1f);
        _redBar.localScale = new Vector3(redProgress,1f,1f);
    }


    public bool TryComplete()
    {
        if (!_processing || !IsReady)
            return false;

        _processing = false;
        _photo = null;

        return true;
    }

    public void Remove()
    {
        _processing = false;
        _photo = null;
        _timer = 0f;
    }
    public void Show()
    {
        _canvasGroup.LeanAlpha(1f, 0.25f);
    }

    public void Hide()
    {
        _canvasGroup.LeanAlpha(0f, 0.25f);
    }
}