using UnityEngine;

public class EnemyModel : CharacterBaseModel
{
        [SerializeField] private string playerTag = "Player";

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
                if (!hit.gameObject.CompareTag(playerTag)) return;

                var playerModel = PlayerManager.Instance.PlayerModel;
                playerModel.TakeDamage(Damage);
                
                gameObject.SetActive(false);
        }
}