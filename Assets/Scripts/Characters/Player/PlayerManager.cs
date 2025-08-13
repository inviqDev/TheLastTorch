using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Light source settings")] [SerializeField]
    private LightSource lightSource;
    
    public PlayerModel PlayerModel { get; private set; }
    
    private Input_Actions _inputActions;
    public Input_Actions InputActions => _inputActions ??= new Input_Actions();

    private List<EnemyModel> _attackableEnemies;
    
    public bool PlayerIsAlive { get; private set; }
    public bool IsAbleToAttack {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        
        PlayerModel = GetComponent<PlayerModel>();

        PlayerIsAlive = true;
        _inputActions ??= new Input_Actions();
        
        _attackableEnemies ??= new List<EnemyModel>();
    }

    public void AddAttackableEnemy(EnemyModel enemy)
    {
        _attackableEnemies?.Add(enemy);
        IsAbleToAttack = true;
    }

    public void RemoveAllAttackableEnemy(EnemyModel enemy)
    {
        _attackableEnemies?.Remove(enemy);
        IsAbleToAttack = _attackableEnemies?.Count == 0;
    }

    public EnemyModel GetClosestAttackableEnemy()
    {
        if (_attackableEnemies?.Count == 0) return null;
        
        var closestEnemy = _attackableEnemies?[0];
        var minSqrDistance = float.MaxValue;
        
        for (var i = 0; i < _attackableEnemies?.Count; i++)
        {
            var distance = Vector3.SqrMagnitude(_attackableEnemies[i].transform.position - transform.position);
            if (!(distance < minSqrDistance)) continue;
            
            minSqrDistance = distance;
            closestEnemy = _attackableEnemies[i];
        }

        return closestEnemy;
    }

    private void Start()
    {
        lightSource.TurnOnLights();
    }

    private void OnEnable()
    {
        _inputActions ??= new Input_Actions();
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions?.Enable();
    }

    private void OnDestroy()
    {
        _inputActions?.Enable();
    }
}