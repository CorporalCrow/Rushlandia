using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;

    public float spawnTimer;
    private float countdown;

    private int numberEnemy;

    void Start()
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
    }

    void Update()
    {
        if (countdown <= 0)
        {
            StartCoroutine(SpawnWave());
            countdown = spawnTimer;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < numberEnemy; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

        numberEnemy++;
        spawnTimer++;
    }   

    void SpawnEnemy()
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
    }
}
