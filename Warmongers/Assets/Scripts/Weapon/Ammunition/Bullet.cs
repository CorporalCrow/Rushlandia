using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
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
                collision.gameObject.GetComponent<Enemy>().TakeDamage(projectile.attackDamage);
                Impact();
                break;
        }
    }

    public void Impact()
    {
        GameObject effect = Instantiate(projectile.hitEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(projectile.intensity, projectile.time);
        Destroy(effect, 1);

        gameObject.SetActive(false);
    }
}
