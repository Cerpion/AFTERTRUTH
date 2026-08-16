using Unity.VisualScripting;
using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private InspectionSystem _inspectionSystem;
    [SerializeField] private GameState _gameState;
    [SerializeField] private Player _player;

    private void Awake()
    {
       //ServiceLocator.Instance.RegisterServices<IlevelController>(_levelController);
       ServiceLocator.Instance.RegisterServices<InputHandler>(_inputHandler);
       ServiceLocator.Instance.RegisterServices<InspectionSystem>(_inspectionSystem);
       ServiceLocator.Instance.RegisterServices<GameState>(_gameState);
       ServiceLocator.Instance.RegisterServices<Player>(_player);
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.UnregisterService<InputHandler>();
        ServiceLocator.Instance.UnregisterService<InspectionSystem>();
        ServiceLocator.Instance.UnregisterService<GameState>();
        ServiceLocator.Instance.UnregisterService<Player>();
    }
}
