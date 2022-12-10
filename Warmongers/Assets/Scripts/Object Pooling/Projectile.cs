using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : AutoDestroyPoolableObject
{
    public Rigidbody rb;

    public GameObject hitEffect;
    public float intensity = 1f;
    public float time = 1f;

    [HideInInspector] public float attackDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        rb.velocity = Vector3.zero;
    }
}
