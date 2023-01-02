using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [ReadOnly] public Transform player;
    public EnemyLineOfSightChecker lineOfSightChecker;
    public NavMeshTriangulation triangulation;
    [ReadOnly] public float updateRate = 0.1f;
    private NavMeshAgent agent;

    [ReadOnly] public EnemyState defaultState;
    private EnemyState _state;
    public EnemyState state
    {
        get
        {
            return _state;
        }
        set
        {
            onStateChange?.Invoke(_state, value);
            _state = value;
        }
    }

    public delegate void StateChangeEvent(EnemyState oldState, EnemyState newState);
    public StateChangeEvent onStateChange;
    [ReadOnly] public float idleLocationRadius = 4f;
    [ReadOnly] public float idleMovespeedMultiplier = 0.5f;
    [ReadOnly] public Vector3[] waypoints = new Vector3[4];
    private int waypointIndex = 0;

    [HideInInspector] public float defaultSpeed;

    private Coroutine followCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        lineOfSightChecker.onGainSight += HandleGainSight;
        lineOfSightChecker.onLoseSight += HandleLoseSight;
         

        onStateChange += HandleStateChange;
    }

    private void HandleGainSight(Player player)
    {
        state = EnemyState.Chase;
    }

    private void HandleLoseSight(Player player)
    {
        state = defaultState;
    }

    private void OnDisable()
    {
        _state = defaultState;
    }

    public void Spawn()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(triangulation.vertices[Random.Range(0,triangulation.vertices.Length)], out hit, 2f, agent.areaMask))
            {
                waypoints[i] = hit.position;
            }
            else
            {
                Debug.LogError("Unable to find position for navmesh near Triangulation vertex!");
            }
        }
        onStateChange?.Invoke(EnemyState.Spawn, defaultState);
    }

    private void HandleStateChange(EnemyState oldState, EnemyState newState)
    {
        if (oldState != newState)
        {
            if (followCoroutine != null)
            {
                StopCoroutine(followCoroutine);
            }
            
            if (oldState == EnemyState.Idle)
            {
                agent.speed /= idleMovespeedMultiplier;
            }

            switch (newState)
            {
                case EnemyState.Idle:
                    followCoroutine = StartCoroutine(DoIdleMotion());
                    break;
                case EnemyState.Patrol:
                    followCoroutine = StartCoroutine(DoPatrolMotion());
                    break;
                case EnemyState.Chase:
                    followCoroutine = StartCoroutine(FollowTarget());
                    break;
            }
        }

        oldState = newState;
    }

    private IEnumerator DoIdleMotion()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        agent.speed *= idleMovespeedMultiplier;

        while (true)
        {
            if (!agent.enabled || !agent.isOnNavMesh)
            {
                yield return wait;
            }
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector2 point = Random.insideUnitCircle * idleLocationRadius;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(agent.transform.position + new Vector3(point.x, 0, point.y), out hit, 2f, agent.areaMask))
                {
                    agent.SetDestination(hit.position);
                }
            }

            yield return wait;
        }
    }

    private IEnumerator DoPatrolMotion()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        yield return new WaitUntil(() => agent.enabled && agent.isOnNavMesh);
        agent.SetDestination(waypoints[waypointIndex]);

        while (true)
        {
            if (agent.isOnNavMesh && agent.enabled && agent.remainingDistance <= agent.stoppingDistance)
            {
                waypointIndex++;

                if (waypointIndex >= waypoints.Length)
                {
                    waypointIndex = 0;
                }

                agent.SetDestination(waypoints[waypointIndex]);
            }

            yield return wait;
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (enabled)
        {
            if (gameObject.GetComponent<NavMeshAgent>().isActiveAndEnabled == true)
                agent.SetDestination(player.transform.position);

            yield return wait;
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawWireSphere(waypoints[i], 0.25f);
            if (i + 1 < waypoints.Length)
            {
                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(waypoints[i], waypoints[0]);
            }
        }
    }
}
