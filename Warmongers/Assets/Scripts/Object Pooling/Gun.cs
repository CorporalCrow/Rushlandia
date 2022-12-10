using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private GameObject inventory;

    [HideInInspector] public float attackDamage;
    [HideInInspector] public float bulletSpeed;

    [HideInInspector] public float fireRate;

    [HideInInspector] public float maxAmmo;
    [ReadOnly] public float currentAmmo;
    [HideInInspector] public float reloadTime;
    [HideInInspector] public bool isReloading = false;

    [HideInInspector] public bool mainHandAbleToShoot = false;
    [HideInInspector] public bool offHandAbleToShoot = false;

    private void Start()
    {
        currentAmmo = 0;
        inventory = GameObject.FindGameObjectWithTag("Inventory Canvas");
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
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

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

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
}
