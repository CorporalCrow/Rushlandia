using System.Collections;
using UnityEngine;

public class Launcher : Ranged
{
    private Camera mainCamera;
    [SerializeField] private LayerMask mask;

    private float distance;

    void OnEnable()
    {
        mainCamera = Camera.main;
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
                    instance.GetComponent<Rigidbody>().AddForce(bulletSpeed * direction * distance / 10 + transform.up * distance / 6, ForceMode.VelocityChange);
                }
                if (instance.transform.parent.tag == "Off Object Pool")
                {
                    instance.transform.SetParent(GameObject.FindGameObjectWithTag("Off Object Pool").transform, false);
                    instance.transform.localPosition = firePoint.position;
                    instance.GetComponent<Projectile>().attackDamage = attackDamage;
                    instance.GetComponent<Rigidbody>().AddForce(bulletSpeed * direction * distance / 10 + transform.up * distance / 6, ForceMode.VelocityChange);
                }
            }

            yield return new WaitForSeconds(timeBetweenVolley);
        }
        burstAttackFinished = true;
    }
}
