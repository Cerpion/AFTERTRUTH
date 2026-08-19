using System;
using UnityEngine;

public class MusicBoxClosed : State<MusicBoxState>
{
    private readonly MusicBoxDirections[] _sequence;
    private readonly MusicBoxAudioHandler _musicBoxAudio;
    private readonly Action OnExitInteraction;
    private readonly Transform _key;

    private int _index;
    private bool _lockInput;


    public MusicBoxClosed(MusicBoxDirections[] sequence, MusicBoxAudioHandler musicBoxAudio, Action onExitInteraction, Transform key)
    {
        _sequence = sequence;
        _musicBoxAudio = musicBoxAudio;
        OnExitInteraction = onExitInteraction;
        _key = key;
    }

    public override void OnEnter()
    {
        var input = ServiceLocator.Instance.GetService<InputHandler>();

        input.OnLeftButton += MoveLeft;
        input.OnRightButton += MoveRight;
        input.OnInteract += ExitInteraction;
    }

    public override void OnExit()
    {
        var input = ServiceLocator.Instance.GetService<InputHandler>();
        input.OnLeftButton -= MoveLeft;
        input.OnRightButton -= MoveRight;
        input.OnInteract -= ExitInteraction;

        Reset();
    }

    public void ExitInteraction()
    {
        OnExitInteraction?.Invoke();
    }

    private void MoveLeft()
    {
        if (_lockInput)
            return;

        VerifySequence(MusicBoxDirections.Left);
    }

    private void MoveRight()
    {
        if (_lockInput)
            return;

        VerifySequence(MusicBoxDirections.Right);
    }

    private void VerifySequence(MusicBoxDirections direction)
    {
        _lockInput = true;

        if (_sequence[_index] == direction)
        {
            _index++;
            AnimKey(direction);
            //Debug.Log("god");

            if (_index >= _sequence.Length)
            {
                //Debug.Log("Finish");
                ChangeState?.Invoke(MusicBoxState.Opening);
                return;
            }

            return;
        }

        AnimKey(direction);
        //Debug.Log("bad");
        _index = 0;
    }

    private void AnimKey(MusicBoxDirections direction)
    {
        _musicBoxAudio.PlayRotate();
        float rotation = direction == MusicBoxDirections.Left ? 90f : -90f;
        var sequence = LeanTween.sequence();
        sequence.append(_key.LeanRotateAround(Vector3.right, rotation, 0.3f));
        sequence.append(() => { _lockInput = false; });
    }

    private void Reset()
    {
        _index = 0;
        _key.LeanRotateX(0, 0.3f);
        _musicBoxAudio.PlayRotate();
    }
}
