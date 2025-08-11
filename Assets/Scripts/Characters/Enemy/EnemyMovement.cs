using UnityEngine;

using Interfaces;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour, IMovable
{
    [Header("Movement settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = -15.0f;
    
    private CharacterController _controller;
    
    private Transform _playerTransform;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerTransform = PlayerManager.Instance.transform;
    }

    private void Update()
    {
        if (!PlayerManager.Instance.PlayerIsAlive) return;
        
        MoveInDirection(_playerTransform.position - transform.position);
    }

    public void MoveInDirection(Vector3 direction)
    {
        direction = direction.normalized;
        
        _moveDirection = new Vector3(direction.x, gravity, direction.z).normalized;
        _controller.Move(_moveDirection * moveSpeed);
    }
}