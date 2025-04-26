// This script manages a pool of enemy objects to avoid frequent instantiation.
// It returns an inactive enemy GameObject when needed and reuses it.

using System.Collections.Generic;
using UnityEngine;

public class SC_EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;       // The enemy prefab to pool
    public int poolSize = 10;            // Number of enemies to prepare

    private List<GameObject> enemies;    // Internal list of pooled enemies

    void Start()
    {
        enemies = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.SetParent(this.transform);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }

        //Debug.Log("[EnemyPool] Initialized with " + poolSize + " enemies.");
    }

    public GameObject GetNextEnemy()
    {
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy)
            {
                //Debug.Log("[EnemyPool] Returning available enemy.");
                return enemy;
            }
        }

        //Debug.LogWarning("[EnemyPool] No available enemy in pool.");
        return null;
    }
}

