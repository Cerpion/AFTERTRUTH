using UnityEngine;

public class InteractionView : MonoBehaviour
{
    [SerializeField] CanvasGroup _interactionPoint;
    [SerializeField] CanvasGroup _interactionInput;

    private void Awake()
    {
        Hide();
    }

    public void ShowPoint()
    {
        Fade(_interactionPoint, 1f);
        Fade(_interactionInput, 0f);
    }

    public void ShowInput()
    {
        Fade(_interactionPoint, 0f);
        Fade(_interactionInput, 1f);
    }

    public void Hide()
    {
        Fade(_interactionPoint, 0f);
        Fade(_interactionInput, 0f);
    }

    private void Fade(CanvasGroup canvasGroup, float targetAlpha)
    {
        LeanTween.cancel(canvasGroup.gameObject);

        if (targetAlpha > 0f)
        {
            canvasGroup.gameObject.SetActive(true);
        }

        canvasGroup.LeanAlpha(targetAlpha, 0.25f)
            .setOnComplete(() =>
            {
                if (targetAlpha <= 0f)
                {
                    canvasGroup.gameObject.SetActive(false);
                }
            });
    }
}
