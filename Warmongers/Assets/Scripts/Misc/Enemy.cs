using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    [ReadOnly] public float currentHealth;
    public GameObject impactEffect;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        GameObject effect = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(effect.gameObject, 1);
    }
}
