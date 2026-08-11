using Unity.Cinemachine;
using UnityEngine;

public enum Directions
{
    Up, Down, Left, Right
}

public class MusicBox : Interactable
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private InputHandler _inputHandler;

    [Header("Configuration")]
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private Directions[] _sequence;
    private int _index;
    private bool _lockInput;

    [Header("Visual")]
    [SerializeField] private Transform _key;
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioClip _keyMove;
    [SerializeField] private AudioClip _open;

    [SerializeField] private Interactable _keyItem;

    public override void StartInteraction()
    {
        _inputHandler.OnLeftButton += MoveLeft;
        _inputHandler.OnRightButton += MoveRight;
        _inputHandler.OnCancel += ExitInteraction;

        _interactionView.Hide();
        _camera.Priority = 100;
        _gameState.ChangeState(GameStates.Puzzle);
    }
    public override void ExitInteraction()
    {
        _inputHandler.OnLeftButton -= MoveLeft;
        _inputHandler.OnRightButton -= MoveRight;
        _inputHandler.OnCancel -= ExitInteraction;

        _interactionView.ShowInput();
        _camera.Priority = 0;
        _gameState.ChangeState(GameStates.Gameplay);
    }

    private void MoveLeft()
    {
        if (_lockInput)
            return;

        VerifySequence(Directions.Left);
    }

    private void MoveRight()
    {
        if (_lockInput)
            return;

        VerifySequence(Directions.Right);
    }

    private void VerifySequence(Directions direction)
    {
        _lockInput = true;

        if (_sequence[_index] == direction)
        {
            _index++;
            AnimKey(direction);
            Debug.Log("god");

            if (_index >= _sequence.Length)
            {
                Debug.Log("Finish");
                _music.PlayOneShot(_open);
                OpenMusicBox();
                return;
            }

            return;
        }

        AnimKey(direction);
        Debug.Log("bad");
        _index = 0;
    }

    private void AnimKey(Directions direction)
    {
        _music.PlayOneShot(_keyMove);
        float rotation = direction == Directions.Left ? 90f : -90f;
        var sequence = LeanTween.sequence();
        sequence.append(_key.LeanRotateAround(Vector3.forward, rotation, 0.3f));
        sequence.append(() => { _lockInput = false; });
    }

    private void OpenMusicBox()
    {
        _inputHandler.OnLeftButton -= MoveLeft;
        _inputHandler.OnRightButton -= MoveRight;
        _inputHandler.OnCancel -= ExitInteraction;

        _keyItem.gameObject.SetActive(true);
        _keyItem.StartInteraction();
        _camera.Priority = 0;
        _gameState.ChangeState(GameStates.Gameplay);
        GetComponent<BoxCollider>().enabled = false ;
    }
}
