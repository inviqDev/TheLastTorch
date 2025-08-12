using UnityEngine;

public class Attack22TriggerLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (!other.TryGetComponent<EnemyModel>(out var enemy)) return;
        
        PlayerManager.Instance.AddAttackableEnemy(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (!other.TryGetComponent<EnemyModel>(out var enemy)) return;
        
        PlayerManager.Instance.RemoveAllAttackableEnemy(enemy);
    }
}