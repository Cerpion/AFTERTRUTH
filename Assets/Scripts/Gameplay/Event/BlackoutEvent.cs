using UnityEngine;

public class BlackoutEvent : EventCinematic
{
    [SerializeField] GlobalLightHandler _globalLightHandler;
    [SerializeField] ItemID _keyObsessionRoom;

    [SerializeField] GameObject _normalSwitch;
    [SerializeField] GameObject _InteractiveSwitch;

    [SerializeField] GameObject _normalDoor;
    [SerializeField] GameObject _lockDoor;

    [SerializeField] string _dialog;

    public override void Execute()
    {
        var player = ServiceLocator.Instance.GetService<Player>();
        if (!player.Inventory.ContainItem(_keyObsessionRoom))
        {
            return;
        }

        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Cinematic);

        _normalSwitch.SetActive(false);
        _InteractiveSwitch.SetActive(true);

        _globalLightHandler.SetNight();
        gameObject.SetActive(false);

        player.OnLight();

        _normalDoor.SetActive(false);
        _lockDoor.SetActive(true);

        DialogueManager.Instance.Play(_dialog);

        LeanTween.delayedCall(1.5f, () => { ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay); });
    }

}
