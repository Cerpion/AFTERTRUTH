using Unity.VisualScripting;
using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private InspectionSystem _inspectionSystem;
    [SerializeField] private GameState _gameState;

    private void Awake()
    {
       //ServiceLocator.Instance.RegisterServices<IlevelController>(_levelController);
       ServiceLocator.Instance.RegisterServices<InputHandler>(_inputHandler);
       ServiceLocator.Instance.RegisterServices<InspectionSystem>(_inspectionSystem);
       ServiceLocator.Instance.RegisterServices<GameState>(_gameState);
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.UnregisterService<InputHandler>();
        ServiceLocator.Instance.UnregisterService<InspectionSystem>();
        ServiceLocator.Instance.UnregisterService<GameState>();
    }
}
