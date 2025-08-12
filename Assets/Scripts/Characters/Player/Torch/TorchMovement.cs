using UnityEngine;

public class TorchMovement : MonoBehaviour
{
    [SerializeField] private Transform torchRoot;
    [SerializeField] private float moveSpeed = 30.0f;

    private Vector3 _defaultTorchPosition;
    private Quaternion _defaultTorchRotation;

    private Vector3 _moveDirection;
    private bool _isLaunchedForward;
    private bool _isReturningBack;

    private void Start()
    {
        SetTorchDefaultSettings();
    }

    private void Update()
    {
        if (!_isLaunchedForward && !_isReturningBack) return;

        if (_isLaunchedForward)
        {
            transform.Translate(_moveDirection * (Time.deltaTime * moveSpeed));
            return;
        }

        if (_isReturningBack)
        {
            if ((torchRoot.position - transform.position).sqrMagnitude < 0.1f)
            {
                SetTorchDefaultSettings();
                return;
            }

            _moveDirection = (torchRoot.position - transform.position).normalized;
            transform.Translate(_moveDirection * (Time.deltaTime * moveSpeed));
        }
    }

    private void SetTorchDefaultSettings()
    {
        transform.SetParent(torchRoot);
        transform.localPosition = _defaultTorchPosition;
        transform.localRotation = _defaultTorchRotation;

        _isLaunchedForward = false;
        _isReturningBack = false;
    }

    public void LaunchTorch(Transform target)
    {
        if (_isReturningBack) return;

        if (_isLaunchedForward)
        {
            // implement "Call torch back" method
            // to return it back even in the middle of the path
            return;
        }

        _isLaunchedForward = true;
        _isReturningBack = false;

        _moveDirection = (target.position - transform.position).normalized;
        _moveDirection.y = 0;

        transform.SetParent(null);
    }

    public void MoveTorchBackToRoot()
    {
        _isLaunchedForward = false;
        _isReturningBack = true;
    }
}