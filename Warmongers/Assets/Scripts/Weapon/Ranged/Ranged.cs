using System.Collections;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    [HideInInspector] public GameObject inventory;

    [HideInInspector] public int attackDamage;
    [HideInInspector] public float bulletSpeed;

    [HideInInspector] public float fireRate;

    [HideInInspector] public int maxAmmo;
    [ReadOnly] public int currentAmmo;

    [HideInInspector] public float reloadTime;


    [HideInInspector] public float spreadFactor = 0f;

    [HideInInspector] public int projectilesPerVolley = 1;
    [HideInInspector] public float timeBetweenVolley = 0f;

    [HideInInspector] public Transform firePoint;

    public Projectile bulletPrefab;
    [HideInInspector]  public ObjectPool bulletPool;

    private void Awake()
    {
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, 40);
        
        var newBulletPool = GameObject.FindGameObjectsWithTag("Object Pool");

        if (transform.parent.tag == "Main Hand")
            for (int i = 0; i < newBulletPool.Length; i++)
            {
                if (newBulletPool[i].name == bulletPrefab.name + " Pool")
                    newBulletPool[i].tag = "Main Object Pool";
            }
        if (transform.parent.tag == "Off Hand")
            for (int i = 0; i < newBulletPool.Length; i++)
            {
                if (newBulletPool[i].name == bulletPrefab.name + " Pool")
                    newBulletPool[i].tag = "Off Object Pool";
            }
    }

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory Canvas");

        //Finding fire point
        GameObject FindChildWithTag(GameObject parent, string tag)
        {
            GameObject child = null;
            foreach (Transform t in transform.parent.parent)
                if (t.tag == "Fire Point")
                {
                    child = t.transform.gameObject;
                    break;
                }
            return child;
        }
        firePoint = FindChildWithTag(gameObject, "Fire Point").transform;
    }

    private void OnDestroy()
    {
        if (transform.parent.tag == "Main Hand")
            Destroy(GameObject.FindGameObjectWithTag("Main Object Pool"));
        if (transform.parent.tag == "Off Hand")
            Destroy(GameObject.FindGameObjectWithTag("Off Object Pool"));
    }
}
