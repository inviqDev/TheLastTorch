using UnityEngine;

using Interfaces;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour, IMovable
{
    private EnemyModel _enemyModel;
    private CharacterController _controller;
    
    private Transform _playerTransform;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _enemyModel = GetComponent<EnemyModel>();
        _controller = GetComponent<CharacterController>();
        _playerTransform = PlayerManager.Instance.transform;
    }

    private void Update()
    {
        if (!_enemyModel.IsAlive && !PlayerManager.Instance.PlayerIsAlive) return;
        MoveInDirection((_playerTransform.position - transform.position).normalized);
    }

    public void MoveInDirection(Vector3 direction)
    {
        if (!_enemyModel.IsAlive) enabled = false;
        
        _moveDirection = new Vector3(direction.x, _enemyModel.Gravity, direction.z);
        _controller?.Move(_moveDirection * (_enemyModel.MoveSpeed * Time.deltaTime));
    }
}