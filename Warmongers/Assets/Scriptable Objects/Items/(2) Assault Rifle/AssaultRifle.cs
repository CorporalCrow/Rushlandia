using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    private Gun gun;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public float spreadFactor;

    private float nextTimeToFire;

    private void Start()
    {
        gun = GetComponent<Gun>();

    }

    void Update()
    {
        if (Time.time >= nextTimeToFire && gun.mainHandAbleToShoot)
        {
            nextTimeToFire = Time.time + 1f / gun.fireRate;
            Shoot();
        }
        if (Time.time >= nextTimeToFire && gun.offHandAbleToShoot)
        {
            nextTimeToFire = Time.time + 1f / gun.fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        var direction = transform.forward;
        direction.x += Random.Range(-spreadFactor, spreadFactor);
        direction.z += Random.Range(-spreadFactor, spreadFactor);


        gun.currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().attackDamage = gun.attackDamage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction * gun.bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, 5f);
    }
}
