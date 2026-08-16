using UnityEngine;

public class FlowCell : MonoBehaviour
{
    public Vector2Int Position { get; private set; }
    public FlowColor FlowColor;

    [SerializeField] private GameObject _highlight;
    [SerializeField] private Renderer _pointRenderer;

    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {

        Position = new Vector2Int(Mathf.RoundToInt(transform.localPosition.y),Mathf.RoundToInt(transform.localPosition.z));
        name = $"{Position.x};{Position.y}";

        UpdatePointColor();
    }

    private void UpdatePointColor()
    {
        if (_pointRenderer == null)
            return;

        _propertyBlock = new MaterialPropertyBlock();

        _pointRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor("_BaseColor", GetColor(FlowColor));
        _pointRenderer.SetPropertyBlock(_propertyBlock);
    }

    private Color GetColor(FlowColor color)
    {
        return color switch
        {
            FlowColor.Blue => Color.blue,
            FlowColor.Red => Color.red,
            FlowColor.Yellow => Color.yellow,
            FlowColor.Orange => new Color(1f, 0.5f, 0f),
            FlowColor.Green => Color.green,
            _ => Color.clear
        };
    }

    public void ShowHighlight()
    {
        _highlight.SetActive(true);
    }

    public void HideHighlight()
    {
        _highlight.SetActive(false);
    }
}