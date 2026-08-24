using UnityEngine;
using UnityEngine.Playables;

public class CinematicNaiden : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;

    private void Awake()
    {
        _director.stopped += AnimationEnded;
    }

    private void AnimationEnded(PlayableDirector obj)
    {
        ServiceLocator.Instance.GetService<TransitionManager>().ReturnToMainMenu();
    }
}
