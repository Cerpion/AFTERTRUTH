using System;
using Unity.Cinemachine;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;

    public InteractionView _interactionView;
    public Action<Interactable> OnRemoveInteractable;

    public abstract bool ShowCursor { get;}
    //public abstract bool OneShot { get; set; }

    public void StartInteraction()
    {
        if (ShowCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (_camera != null)
        {
            _camera.Priority = 100;
        }

        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Puzzle);
        ServiceLocator.Instance.GetService<InputHandler>().OnInteract += StopInteraction;

        OnInteractionStarted();
    }

    public void StopInteraction() 
    {
        if (ShowCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (_camera != null)
        {
            _camera.Priority = 0;
        }

        //if (OneShot)
        //{
        //    gameObject.GetComponent<BoxCollider>().enabled = false;
        //    OnRemoveInteractable?.Invoke(this);
        //}

        ServiceLocator.Instance.GetService<GameState>().ChangeState(GameStates.Gameplay);
        ServiceLocator.Instance.GetService<InputHandler>().OnInteract -= StopInteraction;

        OnInteractionEnded();
    }

    public void DesactiveExitInteraction()
    {
        ServiceLocator.Instance.GetService<InputHandler>().OnInteract -= StopInteraction;
    }

    public abstract void OnInteractionStarted();
    public abstract void OnInteractionEnded();


    public void RemoveInteractable()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        OnRemoveInteractable?.Invoke(this);
    }
}