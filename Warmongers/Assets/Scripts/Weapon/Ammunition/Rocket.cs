using UnityEngine;

public class Rocket : Projectile
{
    public float areaOfEffectRadius = 4f;
    public float acceleration = 1f;

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude <= 60)
            rb.velocity *= acceleration;
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wall":
                Impact();
                break;
            case "Ground":
                Impact();
                break;
            case "Enemy":
                Impact();
                break;
        }
    }

    public void Impact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaOfEffectRadius);

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(intensity, time);
        Destroy(effect, 1);

        foreach (Collider c in colliders)
        {
            if (c.GetComponent<Enemy>())
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }

        gameObject.SetActive(false);
    }
}
