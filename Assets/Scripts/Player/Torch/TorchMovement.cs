using UnityEngine;

public class TorchMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 24.0f;
    
    public bool IsFlying { get; private set; }
    
    private bool _isFlyingForward;
    private Vector3 _moveDirection;

    private void Awake()
    {
        IsFlying = false;
        _isFlyingForward = false;
    }

    private void Update()
    {
        if (_isFlyingForward)
        {
            transform.Translate(_moveDirection * (Time.deltaTime * moveSpeed));
        }
    }

    public void LaunchByPlayerClick(Vector3 direction)
    {
        _isFlyingForward = true;
        _moveDirection = direction;
    }
}
