//using System.Collections;
//using UnityEngine;

//public class GrenadeLauncher : MonoBehaviour
//{
//    private Gun gun;

//    public Transform firePoint;
//    public GameObject bulletPrefab;

//    private float nextTimeToFire;

//    private void Start()
//    {
//        gun = GetComponent<Gun>();
//    }

//    void Update()
//    {
//        if (Time.time >= nextTimeToFire && gun.mainHandAbleToShoot)
//        {
//            nextTimeToFire = Time.time + 1f / gun.fireRate;
//            Shoot();
//        }
//        if (Time.time >= nextTimeToFire && gun.offHandAbleToShoot)
//        {
//            nextTimeToFire = Time.time + 1f / gun.fireRate;
//            Shoot();
//        }
//    }

//    void Shoot()
//    {
//        gun.currentAmmo--;

//        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

//        var _bullet = bullet.GetComponent<Projectile>();
//        _bullet.attackDamage = 0;

//        var grenade = bullet.AddComponent<Grenade>();
//        grenade.hitEffect = _bullet.hitEffect;
//        grenade.intensity = _bullet.intensity;
//        grenade.time = _bullet.time;
//        grenade.attackDamage = gun.attackDamage;

//        Rigidbody rb = bullet.GetComponent<Rigidbody>();
//        rb.useGravity = true;
//        rb.mass = 10;
//        rb.AddForce(firePoint.forward * gun.bulletSpeed * 1.8f, ForceMode.VelocityChange);
//        rb.AddForce(firePoint.up * gun.bulletSpeed * 3, ForceMode.Impulse);
//    }
//}

//public class Grenades : MonoBehaviour
//{
//    public GameObject hitEffect;
//    public float intensity = 1f;
//    public float time = 1f;

//    public float attackDamage;

//    private void OnTriggerEnter(Collider collision)
//    {
//        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Ground")
//        {
//            StartCoroutine(Explode());
//        }
//    }

//    IEnumerator Explode()
//    {
//        yield return new WaitForSeconds(3.9f);

//        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
//        CinemachineShake.Instance.ShakeCamera(intensity, time);
//        Destroy(effect, 1);

//        GameObject tempObject;
//        var areaOfEffect = Instantiate(tempObject = new GameObject(), transform.position, Quaternion.identity);
//        Destroy(tempObject);
//        areaOfEffect.gameObject.AddComponent<SphereCollider>().radius = 5;
//        areaOfEffect.gameObject.GetComponent<SphereCollider>().isTrigger = true;
//        areaOfEffect.gameObject.AddComponent<AreaOfEffect>().attackDamage = attackDamage;
//        Destroy(areaOfEffect, 0.01f);
//    }
//}