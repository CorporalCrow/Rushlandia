using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float areaOfEffectRadius = 4f;
    public float delay;

    float countdown;

    private Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    private void OnEnable()
    {
        countdown = delay;
        projectile.rb.AddForce(transform.up * 2f, ForceMode.VelocityChange);
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
        GameObject effect = Instantiate(projectile.hitEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(projectile.intensity, projectile.time);
        Destroy(effect, 1);
        Debug.Log("explode");

        // Damage nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaOfEffectRadius);

        foreach (Collider c in colliders)
        {
            if (c.GetComponent<Enemy>())
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(projectile.attackDamage);
            }
        }

        // Remove grenade
        gameObject.SetActive(false);
    }
}
