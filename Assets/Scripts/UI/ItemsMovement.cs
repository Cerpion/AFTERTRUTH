using UnityEngine;

public class ItemsMovement : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _handleMusicBox;
    [SerializeField] private GameObject _clickAndDrag;
    [SerializeField] private GameObject _rotate;
    [SerializeField] private GameObject _interact;
    [SerializeField] private GameObject _back;

    public void ShowUI()
    {
        LeanTween.cancel(_canvasGroup.gameObject);

        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;

        _canvasGroup.LeanAlpha(1f, 0.25f);
    }

    public void HideUI()
    {
        LeanTween.cancel(_canvasGroup.gameObject);

        _canvasGroup.LeanAlpha(0f, 0.25f)
            .setOnComplete(() =>
            {
                _canvasGroup.gameObject.SetActive(false);
            });
    }

    public void ShowPuzzleControls()
    {
        DisableAll();

        _clickAndDrag.SetActive(true);
        _interact.SetActive(true);
        _back.SetActive(true);
    }

    public void ShowKeyControls()
    {
        DisableAll();

        _interact.SetActive(true);
        _back.SetActive(true);
    }

    public void ShowPhotoControls()
    {
        DisableAll();

        _rotate.SetActive(true);
        _back.SetActive(true);
    }

    public void ShowMusicBoxControls()
    {
        DisableAll();

        _handleMusicBox.SetActive(true);
        _back.SetActive(true);
    }

    public void BackUI()
    {
        DisableAll();

        _back.SetActive(true);
    }

    private void DisableAll()
    {
        _handleMusicBox.SetActive(false);
        _clickAndDrag.SetActive(false);
        _rotate.SetActive(false);
        _interact.SetActive(false);
        _back.SetActive(false);
    }
}
