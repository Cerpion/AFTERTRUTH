using System.Collections.Generic;
using UnityEngine;

public class InteractionCheck : MonoBehaviour
{
    [SerializeField] private List<Interactable> _interactableItem;
    [SerializeField] private float _interactionAngle = 30f;
    [SerializeField] private Interactable _currentInteractable;

    public Interactable Interactable { get => _currentInteractable; }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out var interactableItem))
        {
            return;
        }

        if (_interactableItem.Contains(interactableItem))
        {
            Debug.LogWarning("The item has already been added.");
            return;
        }

        _interactableItem.Add(interactableItem);
        interactableItem._interactionView.ShowPoint();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out var interactableItem))
        {
            return;
        }

        if (!_interactableItem.Contains(interactableItem))
        {
            Debug.LogWarning("The item does not exist.");
            return;
        }

        if (_currentInteractable == interactableItem)
        {
            _currentInteractable = null;
        }

        interactableItem._interactionView.Hide();
        _interactableItem.Remove(interactableItem);
    }


    public void GetInteractableTarget()
    {
        var closest = GetClosestInteractable();

        if (closest == null)
        {
            return;
        }

        if (!IsLookingAt(closest))
        {
            ClearCurrentInteractable();
            return;
        }

        if (_currentInteractable == closest)
        {
            return;
        }

        _currentInteractable?._interactionView.ShowPoint();
        _currentInteractable = closest;
        _currentInteractable._interactionView.ShowInput();
    }

    private void ClearCurrentInteractable()
    {
        if (_currentInteractable == null)
        {
            return;
        }

        _currentInteractable._interactionView.ShowPoint();
        _currentInteractable = null;
    }



    private Interactable GetClosestInteractable()
    {
        if (_interactableItem.Count == 0)
        {
            return null;
        }

        Interactable closest = null;
        var closestDistanceSqr = float.MaxValue;

        var playerPosition = transform.position;

        foreach (Interactable interactable in _interactableItem)
        {
            float distanceSqr = (interactable.transform.position - playerPosition).sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closest = interactable;
            }
        }

        return closest;
    }

    private bool IsLookingAt(Interactable interactable)
    {
        Vector3 directionToObject =
            interactable.transform.position - transform.position;

        float angle = Vector3.Angle(
            transform.forward,
            directionToObject
        );

        return angle <= _interactionAngle;
    }
}
