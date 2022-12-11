using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private GameObject inventory;

    [HideInInspector] public float attackDamage;
    [HideInInspector] public float bulletSpeed;

    [HideInInspector] public float fireRate;

    [HideInInspector] public int maxAmmo;
    [ReadOnly] public int currentAmmo;
    [HideInInspector] public float reloadTime;
    [HideInInspector] public bool isReloading = false;

    [HideInInspector] public bool mainHandAbleToShoot = false;
    [HideInInspector] public bool offHandAbleToShoot = false;

    [HideInInspector] public float spreadFactor = 0f;

    [HideInInspector] public int projectilesPerVolley = 1;
    [HideInInspector] public float timeBetweenVolley = 0f;

    public Transform firePoint;

    public Projectile bulletPrefab;
    private ObjectPool bulletPool;

    private float nextTimeToFire;
    private bool burstAttackFinished = true;

    private void Awake()
    {
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, 40);
        if (transform.parent.tag == "Main Hand")
            GameObject.FindGameObjectWithTag("Object Pool").tag = "Main Object Pool";
        if (transform.parent.tag == "Off Hand")
            GameObject.FindGameObjectWithTag("Object Pool").tag = "Off Object Pool";
    }

    private void Start()
    {
        currentAmmo = 0;

        inventory = GameObject.FindGameObjectWithTag("Inventory Canvas");

        //Finding fire point
        GameObject FindChildWithTag(GameObject parent, string tag)
        {
            GameObject child = null;
            foreach (Transform t in transform)
                if (t.tag == "Fire Point")
                {
                    child = t.transform.gameObject;
                    break;
                }
            return child;
        }
        firePoint = FindChildWithTag(gameObject, "Fire Point").transform;
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        //Shooting
        if (Time.time >= nextTimeToFire && mainHandAbleToShoot && burstAttackFinished)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            StartCoroutine(Shoot());
        }
        if (Time.time >= nextTimeToFire && offHandAbleToShoot && burstAttackFinished)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            StartCoroutine(Shoot());
        }

        //Reloading by pressing R/T
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo && this.transform.parent.CompareTag("Main Hand"))
        {
            currentAmmo = 0;
        }
        if (Input.GetKeyDown(KeyCode.T) && currentAmmo != maxAmmo && this.transform.parent.CompareTag("Off Hand"))
        {
            currentAmmo = 0;
        }

        if (isReloading)
            return;

        //Reloading
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //Shooting input
        if (Input.GetMouseButton(0) && this.transform.parent.CompareTag("Main Hand") && inventory.gameObject.activeSelf == false)
        {
            mainHandAbleToShoot = true;
        }
        else mainHandAbleToShoot = false;

        if (Input.GetMouseButton(1) && this.transform.parent.CompareTag("Off Hand") && inventory.gameObject.activeSelf == false)
        {
            offHandAbleToShoot = true;
        }
        else offHandAbleToShoot = false;
    }

    IEnumerator Reload()
    {
        isReloading = true;

        mainHandAbleToShoot = false;
        offHandAbleToShoot = false;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    IEnumerator Shoot()
    {
        burstAttackFinished = false;

        currentAmmo--;

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
                    instance.GetComponent<Projectile>().attackDamage = attackDamage;
                    instance.GetComponent<Rigidbody>().AddForce(bulletSpeed * direction, ForceMode.Impulse);
                }
                if (instance.transform.parent.tag == "Off Object Pool")
                {
                    instance.transform.SetParent(GameObject.FindGameObjectWithTag("Off Object Pool").transform, false);
                    instance.transform.localPosition = firePoint.position;
                    instance.GetComponent<Projectile>().attackDamage = attackDamage;
                    instance.GetComponent<Rigidbody>().AddForce(bulletSpeed * direction, ForceMode.Impulse);
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
