using System;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamagable, IAttacker
{
    public Action<Character> OnEnemyDied;

    [SerializeField] protected PlayerConfig config;
    [SerializeField] protected HealthView healthView;
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected Animator animator;

    protected int currentHealth;
    protected float attackCooldown;
    protected Transform attackTarget;

    public bool isDead { get; private set; }

    public virtual void Initialize()
    {
        isDead = false;
        animator = GetComponentInChildren<Animator>();
        currentHealth = config.health;

        healthView.Initialize(config.health);
        healthView.gameObject.SetActive(true);
    }

    public abstract void Attack();

    public virtual void TakeDamage(int value)
    {
        if (isDead)
            return;

        currentHealth -= value;
        healthView.UpdateValue(currentHealth);

        if (currentHealth <= 0)
            Die();
        else
            animator.SetTrigger("3_Damaged");
    }

    protected void Die()
    {
        if (isDead)
            return;

        isDead = true;
        animator.SetTrigger("4_Death");
        OnEnemyDied?.Invoke(this);
    }
}