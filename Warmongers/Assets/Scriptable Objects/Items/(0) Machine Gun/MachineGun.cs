using UnityEngine;

public class MachineGun : MonoBehaviour
{
    private Gun gun;
    public Transform firePoint;
    public Projectile bulletPrefab;
    private float nextTimeToFire;
    private ObjectPool bulletPool;

    public float spreadFactor;

    private void Awake()
    {
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, 200);
        gun = GetComponent<Gun>();
        if (transform.parent.tag == "Main Hand")
            GameObject.FindGameObjectWithTag("Object Pool").tag = "Main Object Pool";
        if (transform.parent.tag == "Off Hand")
            GameObject.FindGameObjectWithTag("Object Pool").tag = "Off Object Pool";
    }

    void Update()
    {
        if (Time.time >= nextTimeToFire && gun.mainHandAbleToShoot)
        {
            nextTimeToFire = Time.deltaTime + 1f / gun.fireRate;
            Shoot();
        }
        if (Time.time >= nextTimeToFire && gun.offHandAbleToShoot)
        {
            nextTimeToFire = Time.deltaTime + 1f / gun.fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        var direction = transform.forward;
        direction.x += Random.Range(-spreadFactor, spreadFactor);
        direction.z += Random.Range(-spreadFactor, spreadFactor);

        gun.currentAmmo--;

        PoolableObject instance = bulletPool.GetObject();

        if (instance != null)
        {
            if (instance.transform.parent.tag == "Main Object Pool")
            {
                instance.transform.SetParent(GameObject.FindGameObjectWithTag("Main Object Pool").transform, false);
                instance.transform.localPosition = firePoint.position;
                instance.GetComponent<Projectile>().attackDamage = gun.attackDamage;
                instance.GetComponent<Rigidbody>().AddForce(direction * gun.bulletSpeed, ForceMode.Impulse);
            }
            if (instance.transform.parent.tag == "Off Object Pool")
            {
                instance.transform.SetParent(GameObject.FindGameObjectWithTag("Off Object Pool").transform, false);
                instance.transform.localPosition = firePoint.position;
                instance.GetComponent<Projectile>().attackDamage = gun.attackDamage;
                instance.GetComponent<Rigidbody>().AddForce(direction * gun.bulletSpeed, ForceMode.Impulse);
            }
        }
    }

    private void OnDestroy()
    {
        if (transform.parent.tag == "Main Hand")
            Destroy(GameObject.FindGameObjectWithTag("Main Object Pool"));
        if (transform.parent.tag == "Off Hand")
            Destroy(GameObject.FindGameObjectWithTag("Off Object Pool"));
    }
}
