using System.Collections.Generic;
using UnityEngine;

public class AttackableEnemiesCollector : MonoBehaviour
{
    // [SerializeField] private BoxCollider torchBoxCollider;
    
    public List<EnemyModel> AttackableEnemies { get; private set; }

    private void Awake()
    {
        AttackableEnemies ??= new List<EnemyModel>();
    }

    public EnemyModel GetClosestEnemy()
    {
        AttackableEnemies.RemoveAll(enemy => !enemy || !enemy.IsAlive);
        
        EnemyModel closestEnemy = null;
        var bestSqrMagnitude = float.MaxValue;
        foreach (var x in AttackableEnemies)
        {
            var sqrMagnitude = Vector3.SqrMagnitude(x.transform.position - transform.position);
            if (!(sqrMagnitude < bestSqrMagnitude)) continue;
            
            bestSqrMagnitude = sqrMagnitude;
            closestEnemy = x;
        }

        return closestEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (!other.TryGetComponent<EnemyModel>(out var enemy)) return;

        if (!AttackableEnemies.Contains(enemy))
        {
            enemy.OnDeath += OnEnemyDeath;
            AttackableEnemies.Add(enemy);
        }
    }

    private void OnEnemyDeath(CharacterBaseModel ctx)
    {
        ctx.OnDeath -= OnEnemyDeath;
        AttackableEnemies.Remove(ctx as EnemyModel);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (!other.TryGetComponent<EnemyModel>(out var enemy)) return;

        enemy.OnDeath -= OnEnemyDeath;
        AttackableEnemies.Remove(enemy);
        
    }
    
    private void OnDisable()
    {
        AttackableEnemies?.Clear();
    }
}