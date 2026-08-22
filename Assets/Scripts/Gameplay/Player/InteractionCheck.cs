using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCheck : MonoBehaviour
{
    [SerializeField] private List<Interactable> _interactableItem;
    [SerializeField] private float _interactionAngle = 30f;
    [SerializeField] private Interactable _currentInteractable;
    [SerializeField] private float _initialSize = 2;

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

        interactableItem.OnRemoveInteractable += RemoveItem;
        _interactableItem.Add(interactableItem);

        interactableItem._interactionView.ShowPoint();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Interactable>(out var interactableItem))
        {
            return;
        }

        interactableItem.OnRemoveInteractable -= RemoveItem;
        RemoveItem(interactableItem);
    }

    public void RemoveItem(Interactable item)
    {
        if (!_interactableItem.Contains(item))
        {
            Debug.LogWarning("The item does not exist.");
            return;
        }

        if (_currentInteractable == item)
        {
            _currentInteractable = null;
        }

        item._interactionView.Hide();
        _interactableItem.Remove(item);
    }

    public void GetInteractableTarget()
    {
        var closest = GetClosestInteractable();

        if (closest == null)
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
        float closestDistanceSqr = float.MaxValue;

        Vector3 playerPosition = transform.position;

        // Solo nos interesa la dirección horizontal del Player.
        Vector3 playerForward = transform.forward;
        playerForward.y = 0f;
        playerForward.Normalize();

        foreach (var interactable in _interactableItem)
        {
            // Dirección desde el Player hacia el objeto.
            Vector3 directionToObject = interactable.transform.position - playerPosition;

            // Ignoramos completamente la diferencia de altura.
            directionToObject.y = 0f;

            float distanceSqr = directionToObject.sqrMagnitude;

            // Si ya tenemos uno más cercano, no necesitamos comprobar este.
            if (distanceSqr >= closestDistanceSqr)
            {
                continue;
            }

            directionToObject.Normalize();

            float angle = Vector3.Angle(playerForward, directionToObject);

            // El objeto no está suficientemente de frente.
            if (angle > _interactionAngle)
            {
                continue;
            }

            closestDistanceSqr = distanceSqr;
            closest = interactable;
        }

        return closest;
    }

    public void DeactivateInteraction()
    {
        transform.LeanScale(Vector3.zero, 0.15f);
    }

    public void ActivateInteraction()
    {
        transform.LeanScale(Vector3.one * _initialSize, 0.15f);
    }
}
