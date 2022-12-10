using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstRifle : MonoBehaviour
{
    private Gun gun;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public float spreadFactor;

    private float nextTimeToFire;
    private bool burstAttackFinished = true;

    private void Start()
    {
        gun = GetComponent<Gun>();

    }

    void Update()
    {
        if (Time.time >= nextTimeToFire && gun.mainHandAbleToShoot && burstAttackFinished)
        {
            nextTimeToFire = Time.time + 1f / gun.fireRate;
            StartCoroutine(Shoot());
        }
        if (Time.time >= nextTimeToFire && gun.offHandAbleToShoot && burstAttackFinished)
        {
            nextTimeToFire = Time.time + 1f / gun.fireRate;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        burstAttackFinished = false;

        gun.currentAmmo--;

        for (int i = 0; i < 3; i++)
        {
            var direction = transform.forward;
            direction.x += Random.Range(-spreadFactor, spreadFactor);
            direction.z += Random.Range(-spreadFactor, spreadFactor);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Projectile>().attackDamage = gun.attackDamage;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(direction * gun.bulletSpeed, ForceMode.Impulse);
            Destroy(bullet, 5f);

            yield return new WaitForSeconds(0.05f);
        }
        burstAttackFinished = true;
    }
}
