using UnityEngine;

public class MusicBoxOpening : State<MusicBoxState>
{
    private readonly GameObject _keyItem;
    private readonly MusicBoxAudioHandler _musicBoxAudio;
    private readonly Animator _animator;
    private readonly ItemID _itemID;
    private Player _player;

    public MusicBoxOpening(GameObject keyItem, MusicBoxAudioHandler musicBoxAudio, Animator animator, ItemID itemID)
    {
        _keyItem = keyItem;
        _musicBoxAudio = musicBoxAudio;
        _animator = animator;
        _itemID = itemID;
    }

    public override void OnEnter()
    {
        _musicBoxAudio.PlayOpen();
        _keyItem.gameObject.SetActive(true);

        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished +=  FinishInspection;


        _animator.Play("Open");
        var clip = _animator.GetCurrentAnimatorClipInfo(0);
        LeanTween.delayedCall(clip.Length, () =>inspection.StartInspect(_itemID.ID));
    }

    public override void OnExit()
    {
        var inspection = ServiceLocator.Instance.GetService<InspectionSystem>();
        inspection.OnInspectionFinished -= FinishInspection;
    }

    public void FinishInspection()
    {
        ServiceLocator.Instance.GetService<Player>().Inventory.TryAdd(_itemID);
        ChangeState?.Invoke(MusicBoxState.Open);
    }

}
