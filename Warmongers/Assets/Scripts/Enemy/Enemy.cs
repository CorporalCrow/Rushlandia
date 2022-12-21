using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject, IDamageable
{
    public AttackRadius attackRadius;
    public EnemyMovement movement;
    public NavMeshAgent agent;
    public EnemyScriptableObject enemyScriptableObject;
    public int health = 100;

    private Coroutine lookCoroutine;

    private void Awake()
    {
        attackRadius.onAttack += OnAttack;
    }

    private void OnAttack(IDamageable target)
    {
        if (lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt(target.GetTransform()));
    }

    private IEnumerator LookAt(Transform target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }

    public virtual void OnEnable()
    {
        SetupAgentConfiguration();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        agent.enabled = false;
    }

    public virtual void SetupAgentConfiguration()
    {
        agent.acceleration = enemyScriptableObject.acceleration;
        agent.angularSpeed = enemyScriptableObject.angularSpeed;
        agent.areaMask = enemyScriptableObject.areaMask;
        agent.avoidancePriority = enemyScriptableObject.avoidancePriority;
        agent.baseOffset = enemyScriptableObject.baseOffset;
        agent.height = enemyScriptableObject.height;
        agent.obstacleAvoidanceType = enemyScriptableObject.obstacleAvoidanceType;
        agent.radius = enemyScriptableObject.radius;
        agent.speed = enemyScriptableObject.speed;
        agent.stoppingDistance = enemyScriptableObject.stoppingDistance;

        movement.updateRate = enemyScriptableObject.aIUpdateInterval;

        health = enemyScriptableObject.health;

        (attackRadius.Collider == null ? attackRadius.GetComponent<SphereCollider>() : attackRadius.Collider).radius = enemyScriptableObject.attackRadius;
        attackRadius.attackDelay = enemyScriptableObject.attackDelay;
        attackRadius.damage = enemyScriptableObject.damage;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
