using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    private Gun gun;

    public Transform firePoint;
    public GameObject bulletPrefab;

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
        gun.currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().attackDamage = gun.attackDamage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * gun.bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, 5f);
    }
}
