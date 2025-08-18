using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private List<Enemy> enemies;

    private float finalDamage;
    private float finalHealth;
    private float finalAttackSpeed;

    public override void Initialize()
    {
        isDead = false;

        finalDamage = config.damage + SaveSystem.PlayerData.damageModifier;
        finalHealth = config.health + SaveSystem.PlayerData.healthModifier;
        finalAttackSpeed = config.attackSpeed + SaveSystem.PlayerData.attackSpeedModifier;

        currentHealth = finalHealth;
        healthView.Initialize(finalHealth);
        healthView.gameObject.SetActive(true);

        UIEnevts.OnUpdateStats?.Invoke(finalDamage, finalHealth, finalAttackSpeed);
    }

    public void SetTarget(List<Enemy> enemiesList)
    {
        enemies = enemiesList;
    }

    private void Update()
    {
        if (enemies != null && enemies.Count != 0)
        {
            attackTarget = enemies[0].transform;
        }
        else
        {
            return;
        }

        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;

        if (Vector3.Distance(transform.position, attackTarget.position) <= config.attackRange && !isDead)
        {
            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = 1f / finalAttackSpeed;
            }
        }
    }

    public void Move()
    {
        animator.SetBool("1_Move", true);
    }

    public void StopMove()
    {
        animator.SetBool("1_Move", false);
    }

    public override void Attack()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.audioClips.range, volume: 0.5f);
        animator.SetTrigger("Attack_Pl");
        var projObj = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z), Quaternion.identity, transform.root);
        projObj.Initialize(this, attackTarget, finalDamage);
    }
}