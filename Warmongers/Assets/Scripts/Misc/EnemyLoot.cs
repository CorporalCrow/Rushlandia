using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public EnemyLootTable[] enemyLootTable;
    private void OnDestroy()
    {
        for (int i = 0; i < enemyLootTable.Length; i++)
        {
            if (enemyLootTable[i] != null)
            {
                if (Random.Range(0f, 1f) <= enemyLootTable[i].dropChance)
                {
                    for (int j = 0; j < enemyLootTable[i].amount; j++)
                    {
                        Instantiate(enemyLootTable[i].item, transform.position, Quaternion.identity);
                    }
                }
                    
            }
        }
    }
}

[System.Serializable]
public class EnemyLootTable
{
    public GameObject item;
    public int amount;
    public float dropChance;
}
