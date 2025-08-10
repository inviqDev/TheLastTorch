using UnityEngine;

public class TorchMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 14.0f;

    private Transform _torchRoot;
    private Vector3 _defaultTorchPosition;
    private Quaternion _defaultTorchRotation;

    private Vector3 _moveDirection;
    private bool _isFlyingForward;
    private bool _isFlyingBackward;

    private void Awake()
    {
        _torchRoot = transform.parent;
        _defaultTorchPosition = transform.localPosition;
        _defaultTorchRotation = transform.localRotation;

        _isFlyingForward = false;
    }

    private void Update()
    {
        if (_isFlyingForward)
        {
            transform.Translate(_moveDirection * (Time.deltaTime * moveSpeed));
            return;
        }

        if (_isFlyingBackward)
        {
            if ((_torchRoot.position - transform.position).sqrMagnitude < 0.1f)
            {
                transform.SetParent(_torchRoot);
                transform.localPosition = _defaultTorchPosition;
                transform.localRotation = _defaultTorchRotation;

                _isFlyingForward = false;
                _isFlyingBackward = false;
                
                return;
            }

            _moveDirection = (_torchRoot.position - transform.position).normalized;
            transform.Translate(_moveDirection * (Time.deltaTime * moveSpeed));
        }
    }

    public void LaunchTorch(Transform target)
    {
        _isFlyingForward = true;
        _isFlyingBackward = false;

        _moveDirection = (target.position - transform.position).normalized;
        _moveDirection.y = 0;

        transform.SetParent(null);
    }

    public void MoveTorchBackToRoot()
    {
        _isFlyingForward = false;
        _isFlyingBackward = true;
    }
}