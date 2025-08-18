using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform target;
    private Character owner;
    private float damage;

    public void Initialize(Character owner, Transform target, float damage)
    {
        this.owner = owner;
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        if (target == null || owner.isDead)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPos = new Vector3(target.position.x, target.position.y + 5f, target.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.3f)
        {
            if (target.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}