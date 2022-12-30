using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyBurstSpawnArea : MonoBehaviour
{
    [SerializeField]
    private Collider spawnCollider;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private List<EnemyScriptableObject> enemies = new List<EnemyScriptableObject>();
    [SerializeField]
    private EnemySpawner.SpawnMethod spawnMethod = EnemySpawner.SpawnMethod.Random;
    [SerializeField]
    private int spawnCount = 10;
    [SerializeField]
    private float spawnDelay = 0.5f;

    private Coroutine spawnEnemiesCoroutine;
    private Bounds bounds;

    private void Awake()
    {
        if (spawnCollider == null)
        {
            spawnCollider = GetComponent<Collider>();
        }

        bounds = spawnCollider.bounds;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (spawnEnemiesCoroutine == null)
        {
            spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    private Vector3 GetRandomPositionInBounds()
    {
        return new Vector3(Random.Range(bounds.min.x, bounds.max.x), bounds.min.y, Random.Range(bounds.min.z, bounds.max.z));
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(spawnDelay);

        for (int i = 0; i < spawnCount; i++)
        {
            if (spawnMethod == EnemySpawner.SpawnMethod.RoundRobin)
            {
                enemySpawner.DoSpawnEnemy(enemySpawner.enemies.FindIndex((enemy) => enemy.Equals(enemies[i % enemies.Count])), GetRandomPositionInBounds()); ;
            }
            else if (spawnMethod == EnemySpawner.SpawnMethod.Random)
            {
                int index = Random.Range(0, enemies.Count);
                enemySpawner.DoSpawnEnemy(enemySpawner.enemies.FindIndex((enemy) => enemy.Equals(enemies[index])), GetRandomPositionInBounds());
            }

            yield return Wait;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (spawnCollider != null)
        {
            Gizmos.DrawWireCube(spawnCollider.bounds.center, spawnCollider.bounds.size);
        }
        else
        {
            Collider collider = GetComponent<Collider>();
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }
}
