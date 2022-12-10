using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
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
        bullet.GetComponent<Projectile>().attackDamage = 0;
        bullet.AddComponent<Rocket>().attackDamage = gun.attackDamage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * gun.bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, 5f);
    }
}

public class Rocket : MonoBehaviour
{
    public float attackDamage;

    private void Update()
    {
        if (this.GetComponent<Rigidbody>().velocity.magnitude <= 60)
            this.GetComponent<Rigidbody>().velocity *= 1.1f;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            GameObject tempObject;
            var areaOfEffect = Instantiate(tempObject = new GameObject(), transform.position, Quaternion.identity);
            Destroy(tempObject);
            areaOfEffect.gameObject.AddComponent<SphereCollider>().radius = 5;
            areaOfEffect.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            areaOfEffect.gameObject.AddComponent<AreaOfEffect>();
            areaOfEffect.GetComponent<AreaOfEffect>().attackDamage = attackDamage;
            Destroy(areaOfEffect, 0.1f);
        }
            
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject tempObject;
            var areaOfEffect = Instantiate(tempObject = new GameObject(), transform.position, Quaternion.identity);
            Destroy(tempObject);
            areaOfEffect.gameObject.AddComponent<SphereCollider>().radius = 5;
            areaOfEffect.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            areaOfEffect.gameObject.AddComponent<AreaOfEffect>();
            areaOfEffect.GetComponent<AreaOfEffect>().attackDamage = attackDamage;
            Destroy(areaOfEffect, 0.1f);
        }
    }
}

public class AreaOfEffect : MonoBehaviour
{
    public float attackDamage;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
}
}