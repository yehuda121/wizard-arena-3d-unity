using System.Collections.Generic;
using UnityEngine;

public class SC_EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;       // The enemy prefab to pool
    public int poolSize = 30;            // Number of enemies to prepare

    private List<GameObject> enemies;    // Internal list of pooled enemies
    public List<GameObject> Enemies => enemies;

    void Start()
    {
        enemies = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.name = "PooledEnemy_" + i;
            enemy.transform.SetParent(this.transform);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }

        //Debug.Log("[EnemyPool] Initialized with " + poolSize + " enemies.");
    }

    public GameObject GetNextEnemy()
    {
        //Debug.Log("[EnemyPool] Searching for inactive enemy in pool...");
        int index = 0;
        foreach (GameObject enemy in enemies)
        {
            //Debug.Log($"[EnemyPool] Enemy {index}: active = {enemy.activeInHierarchy}");
            if (!enemy.activeInHierarchy)
            {
                //Debug.Log($"[EnemyPool] Returning enemy at index {index}: {enemy.name}");
                return enemy;
            }
            index++;
        }

        Debug.LogWarning("[EnemyPool] No available enemy in pool!");
        return null;
    }
}
