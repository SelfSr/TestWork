using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private bool isRange;
    public void SetTarget(Transform target)
    {
        attackTarget = target;
    }

    private void Update()
    {
        if (attackTarget == null || isDead)
            return;

        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;

        if (Vector3.Distance(transform.position, attackTarget.position) <= config.attackRange)
        {
            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = 1f / config.attackSpeed;

                animator.SetBool("1_Move", false);
            }
        }
        else
        {
            animator.SetBool("1_Move", true);
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, attackTarget.position, config.speed * Time.deltaTime);
    }

    public override void Attack()
    {
        if (isRange)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.audioClips.range, volume: 0.5f);
            animator.SetTrigger("Attack_Pl");
            var projObj = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z), Quaternion.identity, transform.root);
            projObj.Initialize(this, attackTarget, config.damage);
        }
        else
        {
            animator.SetTrigger("2_Attack");
            Invoke(nameof(MeleeDamadeDelay), 0.3f);
        }
    }

    private void MeleeDamadeDelay()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.audioClips.melee, volume: 0.5f);
        if (attackTarget.TryGetComponent(out IDamagable target))
            target.TakeDamage(config.damage);
    }
}