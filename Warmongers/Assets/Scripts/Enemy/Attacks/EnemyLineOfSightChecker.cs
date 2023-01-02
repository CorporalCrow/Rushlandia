using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider _collider;
    public float fieldOfView = 360f;
    public LayerMask lineOfSightLayers;

    public delegate void GainSightEvent(Player player);
    public GainSightEvent onGainSight;
    public delegate void LoseSightEvent(Player player);
    public LoseSightEvent onLoseSight;

    private Coroutine checkForLineOfSightCoroutine;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            if (!CheckLineOfSight(player))
            {
                checkForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;
        if (other.TryGetComponent<Player>(out player))
        {
            onLoseSight?.Invoke(player);
            if (checkForLineOfSightCoroutine != null)
            {
                StopCoroutine(checkForLineOfSightCoroutine);
            }
        }
    }

    private bool CheckLineOfSight(Player player)
    {
        Vector3 Direction = (player.transform.position - transform.position).normalized;
        float DotProduct = Vector3.Dot(transform.forward, Direction);
        if (DotProduct >= Mathf.Cos(fieldOfView))
        {
            RaycastHit Hit;

            if (Physics.Raycast(transform.position, Direction, out Hit, _collider.radius, lineOfSightLayers))
            {
                if (Hit.transform.GetComponent<Player>() != null)
                {
                    onGainSight?.Invoke(player);
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator CheckForLineOfSight(Player player)
    {
        WaitForSeconds Wait = new WaitForSeconds(0.1f);

        while (!CheckLineOfSight(player))
        {
            yield return Wait;
        }
    }
}