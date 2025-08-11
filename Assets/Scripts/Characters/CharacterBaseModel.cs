using UnityEngine;

public abstract class CharacterBaseModel : MonoBehaviour
{
    [Header("Health settings")]
    [SerializeField] private float health;
    public float Health => health;
    
    [SerializeField] private float maxHealth;
    public float MaxHealth => maxHealth;

    
    [Header("Movement settings")]
    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    
    [SerializeField] private float gravity;
    public float Gravity => gravity;

    
    [Header("Damage settings")]
    [SerializeField] private float damage;
    public float Damage => damage;

    
    public void TakeDamage(float incomingDamage)
    {
        health = Mathf.Clamp(Health - incomingDamage, 0f, maxHealth);

        if (health > 0) return;
        
        Debug.Log($"{gameObject.name} is DEAD !");
        gameObject.SetActive(false);
    }
}