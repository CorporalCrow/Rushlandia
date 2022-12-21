using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : PoolableObject
{
    public float autoDestroyTime = 5f;
    public float moveSpeed = 2f;
    public int damage = 5;
    public Rigidbody rb;

    private const string DISABLE_METHOD_NAME = "Disable";

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        Invoke(DISABLE_METHOD_NAME, autoDestroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Bullet")
        {
            IDamageable damageable;

            if (other.TryGetComponent<IDamageable>(out damageable))
            {
                damageable.TakeDamage(damage);
            }

            Disable();
        }
    }

    private void Disable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}