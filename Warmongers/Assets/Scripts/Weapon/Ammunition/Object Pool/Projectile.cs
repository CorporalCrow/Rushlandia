using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : AutoDestroyPoolableObject
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public int attackDamage;

    public GameObject hitEffect;
    public float intensity = 1f;
    public float time = 1f;

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
