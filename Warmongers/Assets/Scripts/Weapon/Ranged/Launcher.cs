using System.Collections;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    Ranged ranged;
    private Camera mainCamera;

    public float upthrust;
    public float forwardthrust;

    [SerializeField] private LayerMask mask;

    [HideInInspector] public bool isReloading = false;

    private float nextTimeToFire;
    private bool burstAttackFinished = true;

    [HideInInspector] public bool mainHandAbleToShoot = false;
    [HideInInspector] public bool offHandAbleToShoot = false;

    private float distance;

    private void Awake()
    {
        ranged = GetComponent<Ranged>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, mask))
        {
            distance = Vector3.Distance(raycastHit.point, transform.position);
        }
            

        //Shooting
        if (Time.time >= nextTimeToFire && mainHandAbleToShoot && burstAttackFinished)
        {
            nextTimeToFire = Time.time + 1f / ranged.fireRate;
            StartCoroutine(Shoot());
        }
        if (Time.time >= nextTimeToFire && offHandAbleToShoot && burstAttackFinished)
        {
            nextTimeToFire = Time.time + 1f / ranged.fireRate;
            StartCoroutine(Shoot());
        }

        //Reloading by pressing R/T
        if (Input.GetKeyDown(KeyCode.R) && ranged.currentAmmo != ranged.maxAmmo && this.transform.parent.CompareTag("Main Hand"))
        {
            ranged.currentAmmo = 0;
        }
        if (Input.GetKeyDown(KeyCode.T) && ranged.currentAmmo != ranged.maxAmmo && this.transform.parent.CompareTag("Off Hand"))
        {
            ranged.currentAmmo = 0;
        }

        if (isReloading)
            return;

        //Reloading
        if (ranged.currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //Shooting input
        if (Input.GetMouseButton(0) && this.transform.parent.CompareTag("Main Hand") && ranged.inventory.gameObject.activeSelf == false)
        {
            mainHandAbleToShoot = true;
        }
        else mainHandAbleToShoot = false;

        if (Input.GetMouseButton(1) && this.transform.parent.CompareTag("Off Hand") && ranged.inventory.gameObject.activeSelf == false)
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

        yield return new WaitForSeconds(ranged.reloadTime);

        ranged.currentAmmo = ranged.maxAmmo;
        isReloading = false;
    }

    IEnumerator Shoot()
    {
        burstAttackFinished = false;

        ranged.currentAmmo--;

        for (int i = 0; i < ranged.projectilesPerVolley; i++)
        {
            var direction = transform.forward;
            direction.x += Random.Range(-ranged.spreadFactor, ranged.spreadFactor);
            direction.z += Random.Range(-ranged.spreadFactor, ranged.spreadFactor);

            PoolableObject instance = ranged.bulletPool.GetObject();

            if (instance != null)
            {
                if (instance.transform.parent.tag == "Main Object Pool")
                {
                    instance.transform.SetParent(GameObject.FindGameObjectWithTag("Main Object Pool").transform, false);
                    instance.transform.localPosition = ranged.firePoint.position;
                    instance.GetComponent<Projectile>().attackDamage = ranged.attackDamage;
                    instance.GetComponent<Rigidbody>().AddForce(ranged.bulletSpeed * direction * distance / 10 + transform.up * distance / 6, ForceMode.VelocityChange);
                }
                if (instance.transform.parent.tag == "Off Object Pool")
                {
                    instance.transform.SetParent(GameObject.FindGameObjectWithTag("Off Object Pool").transform, false);
                    instance.transform.localPosition = ranged.firePoint.position;
                    instance.GetComponent<Projectile>().attackDamage = ranged.attackDamage;
                    instance.GetComponent<Rigidbody>().AddForce(ranged.bulletSpeed * direction * distance / 10 + transform.up * distance / 6, ForceMode.VelocityChange);
                }
            }

            yield return new WaitForSeconds(ranged.timeBetweenVolley);
        }
        burstAttackFinished = true;
    }
}
