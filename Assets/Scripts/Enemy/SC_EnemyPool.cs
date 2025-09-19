using System.Collections.Generic;
using UnityEngine;

public class SC_EnemyPool : MonoBehaviour
{
    public static SC_EnemyPool Instance { get; private set; } // Singleton

    [Header("Pool Settings")]
    public GameObject enemyPrefab;   // Enemy prefab
    public int poolSize = 10;        // Initial pool size
    public int expandSize = 5;       // How many to add when pool is empty

    private Queue<GameObject> availableEnemies;  // Free enemies ready for use
    private List<GameObject> activeEnemies;      // Enemies currently in use

    void Awake()
    {
        // Singleton check
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[EnemyPool] Another instance detected, destroying...");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Initialize collections
        availableEnemies = new Queue<GameObject>();
        activeEnemies = new List<GameObject>();

        // Pre-fill pool
        AddEnemies(poolSize);
    }

    // Add new enemies to the pool
    private void AddEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.name = "PooledEnemy_" + (availableEnemies.Count + activeEnemies.Count);
            enemy.transform.SetParent(this.transform);
            enemy.SetActive(false);
            availableEnemies.Enqueue(enemy);
        }
        Debug.Log("[EnemyPool] Expanded by " + count + " enemies. Total = "
                  + (availableEnemies.Count + activeEnemies.Count));
    }

    // Get enemy from pool
    public GameObject GetNextEnemy()
    {
        if (availableEnemies.Count == 0)
        {
            Debug.LogWarning("[EnemyPool] Pool empty, expanding...");
            AddEnemies(expandSize);
        }

        GameObject enemy = availableEnemies.Dequeue();
        enemy.SetActive(true);
        activeEnemies.Add(enemy);
        return enemy;
    }

    // Return enemy to pool (call this when enemy dies or is disabled)
    public void ReturnEnemy(GameObject enemy)
    {
        //Debug.Log("[EnemyPool] Returning enemy: " + enemy.name);
        if (activeEnemies.Remove(enemy))
        {
            //Debug.Log("[EnemyPool] SUCCESS: removed from active and deactivating.");
            enemy.SetActive(false);
            availableEnemies.Enqueue(enemy);
        }
        else
        {
            Debug.LogWarning("[EnemyPool] Tried to return enemy not in active list!");
        }
    }

}
