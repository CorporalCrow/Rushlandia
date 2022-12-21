using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float updateRate = 0.1f;
    private NavMeshAgent agent;

    private Coroutine followCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartChasing()
    {
        if (followCoroutine == null)
        {
            followCoroutine = StartCoroutine(FollowTargets());
        }
        else
        {
            Debug.LogWarning("Called StartChasing on Enemy that is already chasing! This is likely a bug in some calling class");
        }
    }

    private IEnumerator FollowTargets()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (enabled)
        {
            if (gameObject.GetComponent<NavMeshAgent>().isActiveAndEnabled == true)
                agent.SetDestination(player.transform.position);

            yield return wait;
        }
    }
}
