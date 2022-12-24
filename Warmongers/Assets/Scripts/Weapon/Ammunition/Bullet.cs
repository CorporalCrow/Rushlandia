using UnityEngine;

public class Bullet : Projectile
{
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
                collision.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
                Impact();
                break;
        }
    }

    public void Impact()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(intensity, time);
        Destroy(effect, 1);

        gameObject.SetActive(false);
    }
}
