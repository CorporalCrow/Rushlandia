using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public int numberOfEnemiesToSpawn = 5;
    public float spawnDelay = 1f;
    public List<EnemyScriptableObject> enemies = new List<EnemyScriptableObject>();
    public ScalingScriptableObject scaling;
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;
    public bool continuousSpawning;
    [Space]
    [Header("Read At Runtime")]
    [SerializeField]
    private int level = 0;
    [SerializeField]
    private List<EnemyScriptableObject> scaledEnemies = new List<EnemyScriptableObject>();

    private int enemiesAlive = 0;
    private int spawnedEnemies = 0;
    private int initialEnemiesToSpawn;
    private float initialSpawnDelay;

    private NavMeshTriangulation triangulation;
    private Dictionary<int, ObjectPool> enemyObjectPools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyObjectPools.Add(i, ObjectPool.CreateInstance(enemies[i].prefab, numberOfEnemiesToSpawn));
        }

        initialEnemiesToSpawn = numberOfEnemiesToSpawn;
        initialSpawnDelay = spawnDelay;
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();

        for (int i = 0; i < enemies.Count; i++)
        {
            scaledEnemies.Add(enemies[i].ScaleUpForLevel(scaling, 0));
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        level++;
        spawnedEnemies = 0;
        enemiesAlive = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            scaledEnemies[i] = enemies[i].ScaleUpForLevel(scaling, level);
        }

        WaitForSeconds Wait = new WaitForSeconds(spawnDelay);

        while (spawnedEnemies < numberOfEnemiesToSpawn)
        {
            if (enemySpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(spawnedEnemies);
            }
            else if (enemySpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }

            spawnedEnemies++;

            yield return Wait;
        }

        if (continuousSpawning)
        {
            ScaleUpSpawns();
            StartCoroutine(SpawnEnemies());
        }
    }

    private void SpawnRoundRobinEnemy(int SpawnedEnemies)
    {
        int SpawnIndex = SpawnedEnemies % enemies.Count;

        DoSpawnEnemy(SpawnIndex, ChooseRandomPositionOnNavMesh());
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, enemies.Count), ChooseRandomPositionOnNavMesh());
    }

    private Vector3 ChooseRandomPositionOnNavMesh()
    {
        int VertexIndex = Random.Range(0, triangulation.vertices.Length);
        return triangulation.vertices[VertexIndex]; 
    }

    public void DoSpawnEnemy(int SpawnIndex, Vector3 spawnPosition)
    {
        PoolableObject poolableObject = enemyObjectPools[SpawnIndex].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();
            scaledEnemies[SpawnIndex].SetUpEnemy(enemy);

            NavMeshHit Hit;
            if (NavMesh.SamplePosition(spawnPosition, out Hit, 2f, -1))
            {
                enemy.agent.Warp(Hit.position);
                // enemy needs to get enabled and start chasing now.
                enemy.movement.player = player;
                enemy.movement.triangulation = triangulation;
                enemy.agent.enabled = true;
                enemy.movement.Spawn();
                enemy.onDie += HandleEnemyDeath;

                enemiesAlive++;
            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgent on NavMesh. Tried to use {spawnPosition}");
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch enemy of type {SpawnIndex} from object pool. Out of objects?");
        }
    }

    private void ScaleUpSpawns()
    {
        numberOfEnemiesToSpawn = Mathf.FloorToInt(initialEnemiesToSpawn * scaling.spawnCountCurve.Evaluate(level + 1));
        spawnDelay = initialSpawnDelay * scaling.spawnRateCurve.Evaluate(level + 1);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemiesAlive--;

        if (enemiesAlive == 0 && spawnedEnemies == numberOfEnemiesToSpawn)
        {
            ScaleUpSpawns();
            StartCoroutine(SpawnEnemies());
        }
    }

    public enum SpawnMethod
    {
        RoundRobin,
        Random
        // Other spawn methods can be added here
    }
}