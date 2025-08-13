using UnityEngine;

public class TorchModel : MonoBehaviour
{
    [SerializeField] private string enemyLayer = "Enemy";
    
    [SerializeField] private protected float moveSpeed;
    [SerializeField] private protected float damage;

    private TorchMovement _torchMovement;

    private void Awake()
    {
        _torchMovement = gameObject.GetComponent<TorchMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(enemyLayer)) return;

        var enemyModel = other.GetComponent<EnemyModel>();
        
        enemyModel.TakeDamage(damage);
        Debug.Log($"Attacker {gameObject.name} deals to receiver {enemyModel.name} following : {damage} damage amount");
        
        _torchMovement.MoveTorchBackToRoot();
    }
}