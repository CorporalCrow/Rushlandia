using UnityEngine;

public class Grenade : Projectile
{
    public float areaOfEffectRadius = 4f;
    public float delay;

    float countdown;

    private float velocityNeededToReachDestination;

    public override void OnEnable()
    {
        countdown = delay;
        rb.AddForce(transform.up * velocityNeededToReachDestination, ForceMode.VelocityChange);
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Show effect
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(intensity, time);
        Destroy(effect, 1);

        // Damage nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaOfEffectRadius);

        foreach (Collider c in colliders)
        {
            if (c.GetComponent<Enemy>())
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }

        // Remove grenade
        gameObject.SetActive(false);
    }
}
