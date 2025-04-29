// This script periodically spawns enemies at random spawn points.
// It uses the EnemyPool to retrieve enemies instead of instantiating new ones.

using UnityEngine;

public class SC_EnemySpawner : MonoBehaviour
{
    public SC_EnemyPool enemyPool;         // Reference to enemy pool
    public float spawnInterval = 5f;       // Time between spawns
    public Transform[] spawnPoints;        // Possible spawn locations

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return; // No spawn points available

        GameObject enemy = enemyPool.GetNextEnemy();
        if (enemy == null) return; // No available enemy in the pool

        // Try up to 5 times to find a valid spawn point
        for (int attempt = 0; attempt < 5; attempt++)
        {
            int index = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[index];

            // Check if the spawn area is clear
            float checkRadius = 1.5f; // Check radius (adjust based on enemy size)
            Collider[] hitColliders = Physics.OverlapSphere(spawnPoint.position, checkRadius);

            bool spaceIsClear = true;

            foreach (var hit in hitColliders)
            {
                if (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Player"))
                {
                    spaceIsClear = false;
                    break;
                }
            }

            if (spaceIsClear)
            {
                // Set enemy position and activate
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;
                enemy.SetActive(true);

                // reset the enemy
                SC_EnemyHealthSystem enemyHealth = enemy.GetComponent<SC_EnemyHealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.ResetEnemy();
                }

                // Debug.Log("[EnemySpawner] Spawned enemy at: " + spawnPoint.position);
                return;
            }
        }
        // not founded after 5 attemts
        //Debug.Log("[EnemySpawner] No available space to spawn enemy.");
    }


}