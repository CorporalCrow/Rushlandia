using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Player player;
    public int numberOfEnemiesToSpawn = 5;
    public float spawnDelay = 1f;
    public List<WeightedSpawnScriptableObject> weightedEnemies = new List<WeightedSpawnScriptableObject>();
    public ScalingScriptableObject scaling;
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;
    public bool continuousSpawning;
    [Space]
    [Header("Read At Runtime")]
    [SerializeField]
    private int level = 0;
    [SerializeField]
    private List<EnemyScriptableObject> scaledEnemies = new List<EnemyScriptableObject>();
    [SerializeField]
    [ReadOnly] private float[] weights;

    private int enemiesAlive = 0;
    private int spawnedEnemies = 0;
    private int initialEnemiesToSpawn;
    private float initialSpawnDelay;

    private NavMeshTriangulation triangulation;
    private Dictionary<int, ObjectPool> enemyObjectPools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        for (int i = 0; i < weightedEnemies.Count; i++)
        {
            enemyObjectPools.Add(i, ObjectPool.CreateInstance(weightedEnemies[i].enemy.prefab, numberOfEnemiesToSpawn));
        }

        weights = new float[weightedEnemies.Count];
        initialEnemiesToSpawn = numberOfEnemiesToSpawn;
        initialSpawnDelay = spawnDelay;
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();

        for (int i = 0; i < weightedEnemies.Count; i++)
        {
            scaledEnemies.Add(weightedEnemies[i].enemy.ScaleUpForLevel(scaling, 0));
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        level++;
        spawnedEnemies = 0;
        enemiesAlive = 0;
        for (int i = 0; i < weightedEnemies.Count; i++)
        {
            scaledEnemies[i] = weightedEnemies[i].enemy.ScaleUpForLevel(scaling, level);
        }

        ResetSpawnWeights();

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
            else if (enemySpawnMethod == SpawnMethod.WeightedRandom)
            {
                SpawnWeightedRandomEnemy();
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

    private void ResetSpawnWeights()
    {
        float totalWeight = 0;

        for (int i = 0; i < weightedEnemies.Count; i++)
        {
            weights[i] = weightedEnemies[i].GetWeight();
            totalWeight += weights[i];
        }

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = weights[i] / totalWeight;
        }
    }

    private void SpawnRoundRobinEnemy(int SpawnedEnemies)
    {
        int SpawnIndex = SpawnedEnemies % weightedEnemies.Count;

        DoSpawnEnemy(SpawnIndex, ChooseRandomPositionOnNavMesh());
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, weightedEnemies.Count), ChooseRandomPositionOnNavMesh());
    }

    private void SpawnWeightedRandomEnemy()
    {
        float value = Random.value;

        for (int i = 0; i < weights.Length; i++)
        {
            if (value < weights[i])
            {
                DoSpawnEnemy(i, ChooseRandomPositionOnNavMesh());
                return;
            }

            value -= weights[i];
        }

        Debug.LogError("Invalid configuration! Could not spawn a Weighted Random Enemy. Did you forget to call ResetSpawnWeights()?");
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
                enemy.movement.player = player.transform;
                enemy.movement.triangulation = triangulation;
                enemy.agent.enabled = true;
                enemy.movement.Spawn();
                enemy.onDie += HandleEnemyDeath;
                enemy.level = level;
                enemy.skills = scaledEnemies[SpawnIndex].skills;
                enemy.player = player;

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
        Random,
        WeightedRandom
        // Other spawn methods can be added here
    }
}