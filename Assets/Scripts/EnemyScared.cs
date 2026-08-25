using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScared : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _player;

    [Header("Hide Points")]
    [SerializeField] private Transform[] _hidePoints;
    [SerializeField] private float _minimumDistanceFromPlayer = 8f;

    [Header("Behavior")]
    [SerializeField] private float _playerDetectionDistance = 5f;
    [SerializeField] private float _waitTimeBeforeEscape = 2f;

    [SerializeField] private float _rotationSpeed = 5f;

    private Transform _currentHidePoint;
    private float _waitTimer;

    private bool _isScared;
    public bool StartScape;

    private void Update()
    {
        if (StartScape == false)
            return;

        if (!_isScared)
        {
            CheckIfReachedHidePoint();
            return;
        }

        CheckPlayerDistance();
    }

    private void CheckIfReachedHidePoint()
    {
        if (_agent.pathPending)
            return;

        if (_agent.remainingDistance > _agent.stoppingDistance)
            return;

        _agent.isStopped = true;

        _isScared = true;
        _waitTimer = 0f;

        _animator.Play("IdleScared");
    }

    private void CheckPlayerDistance()
    {
        RotateTowardsPlayer();

        float distance = Vector3.Distance(
            transform.position,
            _player.position
        );

        if (distance > _playerDetectionDistance)
        {
            _waitTimer = 0f;
            return;
        }

        _waitTimer += Time.deltaTime;

        if (_waitTimer >= _waitTimeBeforeEscape)
        {
            _waitTimer = 0f;

            ChooseNewHidePoint();
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = _player.position - transform.position;

        // Evita que el enemigo incline el cuerpo hacia arriba/abajo.
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }

    public void ChooseNewHidePoint()
    {
        Transform selectedPoint = GetValidHidePoint();

        if (selectedPoint == null)
        {
            Debug.LogWarning("No hay un punto de escondite válido.");
            return;
        }

        _currentHidePoint = selectedPoint;

        _isScared = false;

        _agent.isStopped = false;
        _agent.SetDestination(_currentHidePoint.position);

        _animator.Play("RunEnemy");
    }

    private Transform GetValidHidePoint()
    {
        List<Transform> validPoints = new List<Transform>();

        foreach (Transform point in _hidePoints)
        {
            if (point == null)
                continue;

            // No volver al mismo punto.
            if (point == _currentHidePoint)
                continue;

            float distance = Vector3.Distance(
                _player.position,
                point.position
            );

            // El punto debe estar suficientemente lejos del jugador.
            if (distance >= _minimumDistanceFromPlayer)
            {
                validPoints.Add(point);
            }
        }

        if (validPoints.Count == 0)
            return null;

        return validPoints[Random.Range(0, validPoints.Count)];
    }
}
