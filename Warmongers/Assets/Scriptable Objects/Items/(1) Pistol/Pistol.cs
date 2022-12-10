using System.Collections;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    private Gun gun;

    public Transform firePoint;

    public Projectile bulletPrefab;
    private ObjectPool bulletPool;

    public float spreadFactor;
    public int projectilesPerVolley = 1;
    public float timeBetweenVolley = 0f;

    private float nextTimeToFire;
    private bool burstAttackFinished = true;

    private void Awake()
    {
        gun = GetComponent<Gun>();
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, 40);
        if (transform.parent.tag == "Main Hand")
            GameObject.FindGameObjectWithTag("Object Pool").tag = "Main Object Pool";
        if (transform.parent.tag == "Off Hand")
            GameObject.FindGameObjectWithTag("Object Pool").tag = "Off Object Pool";
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

        for (int i = 0; i < projectilesPerVolley; i++)
        {
            var direction = transform.forward;
            direction.x += Random.Range(-spreadFactor, spreadFactor);
            direction.z += Random.Range(-spreadFactor, spreadFactor);

            PoolableObject instance = bulletPool.GetObject();

            if (instance != null)
            {
                if (instance.transform.parent.tag == "Main Object Pool")
                {
                    instance.transform.SetParent(GameObject.FindGameObjectWithTag("Main Object Pool").transform, false);
                    instance.transform.localPosition = firePoint.position;
                    instance.GetComponent<Projectile>().attackDamage = gun.attackDamage;
                    instance.GetComponent<Rigidbody>().AddForce(gun.bulletSpeed * direction, ForceMode.Impulse);
                }
                if (instance.transform.parent.tag == "Off Object Pool")
                {
                    instance.transform.SetParent(GameObject.FindGameObjectWithTag("Off Object Pool").transform, false);
                    instance.transform.localPosition = firePoint.position;
                    instance.GetComponent<Projectile>().attackDamage = gun.attackDamage;
                    instance.GetComponent<Rigidbody>().AddForce(gun.bulletSpeed * direction, ForceMode.Impulse);
                }
            }

            yield return new WaitForSeconds(timeBetweenVolley);
        }
        burstAttackFinished = true;
    }

    private void OnDestroy()
    {
        if (transform.parent.tag == "Main Hand")
            Destroy(GameObject.FindGameObjectWithTag("Main Object Pool"));
        if (transform.parent.tag == "Off Hand")
            Destroy(GameObject.FindGameObjectWithTag("Off Object Pool"));
    }
}
