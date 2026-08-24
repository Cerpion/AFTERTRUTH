using System.Collections.Generic;
using UnityEngine;

public class InitialScreens : MonoBehaviour
{
    [SerializeField] private List<SplashImageConfig> _splashImage;


    private void Awake()
    {
        HideAll();

        var sequence = LeanTween.sequence();

        foreach (var image in _splashImage)
        {
            sequence.append(LeanTween.alphaCanvas(image.CanvasGroup,1,0.5f).setEase(LeanTweenType.easeInOutCubic));
            sequence.append(image.TimeInScreen);
            sequence.append(LeanTween.alphaCanvas(image.CanvasGroup,0,0.5f).setEase(LeanTweenType.easeInOutCubic));
            sequence.append(0.15f);
        }

        sequence.append(0.5f);
        sequence.append(() => { ServiceLocator.Instance.GetService<TransitionManager>().ReturnToMainMenu(); });
    }

    private void HideAll()
    {
        foreach (var image in _splashImage)
        {
            image.CanvasGroup.alpha = 0f;
            image.CanvasGroup.interactable = false;
            image.CanvasGroup.blocksRaycasts = false;
        }
    }

}

[System.Serializable]
public class SplashImageConfig
{
    public CanvasGroup CanvasGroup;
    public float TimeInScreen;
}