using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FinalEvent : EventCinematic
{
    [SerializeField] private Safe _safe;
    [SerializeField] private string _finalDialogue;
    [SerializeField] private GameObject _finalDesicionUI;
    [SerializeField] private GameObject _enemy;

    [SerializeField] private Button _badFinal;
    [SerializeField] private Button _goodFinal;

    [SerializeField] private Transform _point01;
    [SerializeField] private Transform _point02;

    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _pointShoot;
    [SerializeField] private GameObject _blackCanvas;
    [SerializeField] private EnemyScared _enemyScared;


    private float _moveSpeed = 1f;
    private float _rotationDuration = 1f;

    private void Awake()
    {
        _badFinal.onClick.AddListener(FirstEnding);
        _goodFinal.onClick.AddListener(SecondEnding);
    }

    public override void Execute()
    {
        Play();
        DisableEvent();
    }

    private void Play()
    {
        _enemy.gameObject.SetActive(true);
        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Cinematic);

        var player = ServiceLocator.Instance.GetService<Player>();
        var sequence = LeanTween.sequence();

        sequence.append(LeanTween.move(player.gameObject, _point01.position, GetMoveDuration(player.transform,_point01)).setEase(LeanTweenType.easeInOutSine));
        sequence.append(LeanTween.rotate(player.gameObject, GetRotation(_point01), _rotationDuration).setEase(LeanTweenType.easeInOutSine));

        sequence.append(() => { DialogueManager.Instance.Play(_finalDialogue); });

        sequence.append(LeanTween.move(player.gameObject, _point02.position, GetMoveDuration(player.transform,_point02)).setEase(LeanTweenType.easeInOutSine));
        sequence.append(LeanTween.rotate(player.gameObject, GetRotation(_point02), _rotationDuration).setEase(LeanTweenType.easeInOutSine));



        sequence.append(() => { player.ShowGun(); });
        sequence.append(1.5f);
        sequence.append(() => { _finalDesicionUI.SetActive(true); });
        sequence.append(() => { Cursor.visible = true; Cursor.lockState = CursorLockMode.Confined; });
    }

    private float GetMoveDuration(Transform player,Transform target)
    {
        float distance = Vector3.Distance(player.position, target.position );
        return distance / _moveSpeed;
    }

    private Vector3 GetRotation(Transform target)
    {
        Vector3 forward = target.forward;
        forward.y = 0f;
        return Quaternion.LookRotation(forward).eulerAngles;
    }

    public void FirstEnding()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
        _finalDesicionUI.SetActive(false);
        _inventory.SetActive(false);

        var player = ServiceLocator.Instance.GetService<Player>();

        player.ShooterMode();
        player.OnTargetHit += ExecuteFinal;
        _pointShoot.SetActive(true);

        LeanTween.delayedCall(0.25f, () => { _enemyScared.StartScape = true; _enemyScared.ChooseNewHidePoint(); });
    }

    private void ExecuteFinal()
    {
        var sequence = LeanTween.sequence();

        sequence.append(() => { _blackCanvas.gameObject.SetActive(true); });
        sequence.append(1f);
        sequence.append(() => { AudioManager.Instance.PlaySFX("Shot"); });
        sequence.append(1.5f);

        sequence.append(() => {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        });

        sequence.append(() => { ServiceLocator.Instance.GetService<TransitionManager>().FirstEnding(); });
    }

    public void SecondEnding()
    {
        //Dont shoot
        var sequence = LeanTween.sequence();
        var player = ServiceLocator.Instance.GetService<Player>();

        sequence.append(() => { player.HideGun(); });
        sequence.append(1.5f);
        sequence.append(() => { ServiceLocator.Instance.GetService<TransitionManager>().SecondEnding(); });
    }
}
