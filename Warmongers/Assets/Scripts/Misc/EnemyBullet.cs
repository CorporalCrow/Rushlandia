using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject impactEffect;
    public int attackDamage;
    public float attackLifespan;

    private void FixedUpdate()
    {
        Destroy(gameObject, attackLifespan);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
            Impact();

        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().TakeDamage(attackDamage);
            Impact();
        }
    }

    public void Impact()
    {
        Destroy(gameObject);
        GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(effect.gameObject, 1);
    }
}
