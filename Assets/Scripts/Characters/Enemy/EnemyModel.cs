using UnityEngine;

public class EnemyModel : CharacterBaseModel
{
    [SerializeField] private string playerTag = "Player";
   // [SerializeField] private string torchTag = "Torch";

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;
    
        if (!hit.gameObject.CompareTag(playerTag)) return;
        
        Debug.Log("Hit player !");
        // playerModel.TakeDamage(Damage);
    }
}