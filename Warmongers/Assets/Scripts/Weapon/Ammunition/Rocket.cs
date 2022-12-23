using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float areaOfEffectRadius = 4f;
    public float acceleration = 1f;

    private Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    private void FixedUpdate()
    {
        if (projectile.rb.velocity.magnitude <= 60)
            projectile.rb.velocity *= acceleration;
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

        GameObject effect = Instantiate(projectile.hitEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(projectile.intensity, projectile.time);
        Destroy(effect, 1);

        foreach (Collider c in colliders)
        {
            if (c.GetComponent<Enemy>())
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(projectile.attackDamage);
            }
        }

        gameObject.SetActive(false);
    }
}
