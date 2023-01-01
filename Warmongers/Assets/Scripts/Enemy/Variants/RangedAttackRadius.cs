using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttackRadius : AttackRadius
{
    public NavMeshAgent agent;
    public EnemyBullet bulletPrefab;
    public Vector3 bulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask mask;
    private ObjectPool bulletPool;
    [SerializeField]
    private float spherecastRadius = 0.1f;
    private RaycastHit hit;
    private IDamageable targetDamageable;
    private EnemyBullet bullet;

    public void CreateBulletPool()
    {
        if (bulletPool == null)
        {
            bulletPool = ObjectPool.CreateInstance(bulletPrefab, Mathf.CeilToInt((1 / attackDelay) * bulletPrefab.autoDestroyTime));
        }
    }

    protected override IEnumerator Attack()
    {
        WaitForSeconds wait = new WaitForSeconds(attackDelay);

        yield return wait;

        while (damageables.Count > 0)
        {
            for (int i = 0; i < damageables.Count; i++)
            {
                if (HasLineOfSightTo(damageables[i].GetTransform()))
                {
                    targetDamageable = damageables[i];
                    onAttack?.Invoke(damageables[i]);
                    agent.enabled = false;
                    break;
                }
            }

            if (targetDamageable != null)
            {
                PoolableObject poolableObject = bulletPool.GetObject();
                if (poolableObject != null)
                {
                    bullet = poolableObject.GetComponent<EnemyBullet>();

                    bullet.transform.position = transform.position + bulletSpawnOffset;
                    bullet.transform.rotation = agent.transform.rotation;

                    bullet.Spawn(agent.transform.forward, damage, targetDamageable.GetTransform());
                }
            }
            else
            {
                agent.enabled = true; // no target in line of sight, keep trying to get closer
            }

            yield return wait;

            if (targetDamageable == null || !HasLineOfSightTo(targetDamageable.GetTransform()))
            {
                agent.enabled = true;
            }

            damageables.RemoveAll(DisabledDamageables);
        }

        agent.enabled = true;
        attackCoroutine = null;
    }

    private bool HasLineOfSightTo(Transform target)
    {
        if (Physics.SphereCast(transform.position + bulletSpawnOffset, spherecastRadius, ((target.position + bulletSpawnOffset) - (transform.position + bulletSpawnOffset)).normalized, out hit, Collider.radius, mask))
        {
            IDamageable damageable;
            if (hit.collider.TryGetComponent<IDamageable>(out damageable))
            {
                return damageable.GetTransform() == target;
            }
        }

        return false;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (attackCoroutine == null)
        {
            agent.enabled = true;
        }
    }
}