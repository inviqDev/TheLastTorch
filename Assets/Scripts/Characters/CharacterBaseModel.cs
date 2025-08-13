using System;
using UnityEngine;

public abstract class CharacterBaseModel : MonoBehaviour
{
    public event Action<CharacterBaseModel> OnDeath;
    
    [Header("Health settings")] [SerializeField]
    private float health;

    public float Health => health;
    private float _maxHealth;


    [Header("Movement settings")] [SerializeField]
    private float moveSpeed;

    public float MoveSpeed => moveSpeed;

    [SerializeField] private float gravity;
    public float Gravity => gravity;


    [Header("Damage settings")] [SerializeField]
    private float damage;
    protected float Damage => damage;
    
    public bool IsAlive { get; private set; }


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        IsAlive = true;
        _maxHealth = health;
    }

    public void TakeDamage(float incomingDamage)
    {
        health = Mathf.Clamp(Health - incomingDamage, 0f, _maxHealth);

        if (health > 0) return;
        Die();
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} is DEAD !");
        OnDeath?.Invoke(this);
        
        IsAlive = false;
        gameObject.SetActive(false);
    }
}