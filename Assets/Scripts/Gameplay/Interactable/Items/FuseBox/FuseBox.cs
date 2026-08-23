using Unity.Cinemachine;
using UnityEngine;

public class FuseBox : Interactable
{
    [SerializeField] private GlobalLightHandler _globalLightHandler;
    [SerializeField] private FlowGame _flowGame;

    [SerializeField] private GameObject _normalDoor;
    [SerializeField] private GameObject _lockedDoor;
    private bool _canUpdate;
    public override bool ShowCursor => true;

    public override void OnInteractionStarted()
    {
        _canUpdate = true;
        _flowGame.Finish += OnLight;

        ServiceLocator.Instance.GetService<ItemsMovement>().ShowUI();
        ServiceLocator.Instance.GetService<ItemsMovement>().ShowPuzzleControls();
    }

    public override void OnInteractionEnded()
    {
        ServiceLocator.Instance.GetService<ItemsMovement>().HideUI();

        _canUpdate = false;
        _flowGame.Finish -= OnLight;
    }

    public void OnLight()
    {
        StopInteraction();
        _globalLightHandler.SetNight();
        var player = ServiceLocator.Instance.GetService<Player>();
        player.OffLight();

        _normalDoor.SetActive(true);
        _lockedDoor.SetActive(false);

        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        if (!_canUpdate)
        {
            return;
        }

        _flowGame.UpdateFlowGame();
    }

}
