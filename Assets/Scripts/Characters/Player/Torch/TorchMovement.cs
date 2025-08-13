using UnityEngine;

public enum TorchState
{
    InHand,
    MovingToLight,
    MovingToEnemy,
    MovingBackward
}

public class TorchMovement : TorchModel
{
    [SerializeField] private AttackableEnemiesCollector enemiesCollector;
    [SerializeField] private Transform torchRoot;

    private Vector3 _defaultTorchPosition;
    private Quaternion _defaultTorchRotation;

    public TorchState State { get; private set; }

    private Vector3 _moveDirection;
    private EnemyModel _currentEnemy;

    private void Start()
    {
        SetTorchDefaultSettings();
    }

    private void Update()
    {
        if (State == TorchState.InHand)
        {
            if (enemiesCollector.AttackableEnemies?.Count == 0) return;
            _currentEnemy = enemiesCollector.GetClosestEnemy();

            if (_currentEnemy)
            {
                State = TorchState.MovingToEnemy;
                transform.SetParent(null);
            }
        }

        if (State == TorchState.MovingToEnemy && _currentEnemy)
        {
            _moveDirection = (_currentEnemy.transform.position - transform.position).normalized;
        }

        if (State == TorchState.MovingBackward)
        {
            if ((torchRoot.position - transform.position).sqrMagnitude < 1.0f)
            {
                SetTorchDefaultSettings();
                return;
            }

            _moveDirection = (torchRoot.position - transform.position).normalized;
        }

        _moveDirection.y = 0.0f;
        transform.Translate(_moveDirection * (moveSpeed * Time.deltaTime));
    }

    public void LaunchToLightSource(Vector3 lightSourcePos)
    {
        if (State != TorchState.InHand)
        {
            Debug.Log("Cannot be lauched : Not in hand state");
            return;
        }

        State = TorchState.MovingToLight;

        _moveDirection = (lightSourcePos - transform.position).normalized;
        _moveDirection.y = 0.0f;

        transform.SetParent(null);
    }

    private void SetTorchDefaultSettings()
    {
        State = TorchState.InHand;

        transform.SetParent(torchRoot);
        transform.localPosition = _defaultTorchPosition;
        transform.localRotation = _defaultTorchRotation;
    }

    public void MoveTorchBackToRoot()
    {
        State = TorchState.MovingBackward;
    }
}