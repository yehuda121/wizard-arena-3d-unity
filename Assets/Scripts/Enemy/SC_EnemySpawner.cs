//// This script periodically spawns enemies at random spawn points.
//// It uses the EnemyPool to retrieve enemies instead of instantiating new ones.

//using UnityEngine;
//// Define the available difficulty levels

//public class SC_EnemySpawner : MonoBehaviour
//{
//    // Current difficulty level for the spawner
//    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

//    public SC_EnemyPool enemyPool;         // Reference to enemy pool
//    public Transform[] spawnPoints;        // Possible spawn locations

//    private float timer;

//    // Get spawn interval based on current difficulty
//    private float GetSpawnInterval()
//    {
//        switch (currentDifficulty)
//        {
//            case DifficultyLevel.Easy: return 8f;
//            case DifficultyLevel.Medium: return 5f;
//            case DifficultyLevel.Hard: return 3f;
//            case DifficultyLevel.Boss: return 99f; // No regular enemies in boss mode
//            default: return 8f;
//        }
//    }

//    void Update()
//    {
//        // Only start spawning if enough enemies are alive
//        if (CountAliveEnemies() < 4)
//        {
//            timer += Time.deltaTime;
//            if (timer >= GetSpawnInterval())
//            {
//                SpawnEnemy();
//                timer = 0f;
//            }
//        }
//    }

//    // Count how many enemies are currently active in the scene
//    int CountAliveEnemies()
//    {
//        return GameObject.FindGameObjectsWithTag("Enemy").Length;
//    }


//    void SpawnEnemy()
//    {
//        if (spawnPoints.Length == 0) return; // No spawn points available

//        GameObject enemy = enemyPool.GetNextEnemy();
//        if (enemy == null) return; // No available enemy in the pool

//        // Try up to 5 times to find a valid spawn point
//        for (int attempt = 0; attempt < 5; attempt++)
//        {
//            int index = Random.Range(0, spawnPoints.Length);
//            Transform spawnPoint = spawnPoints[index];

//            // Check if the spawn area is clear
//            float checkRadius = 1.5f; // Check radius (adjust based on enemy size)
//            Collider[] hitColliders = Physics.OverlapSphere(spawnPoint.position, checkRadius);

//            bool spaceIsClear = true;

//            foreach (var hit in hitColliders)
//            {
//                if (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Player"))
//                {
//                    spaceIsClear = false;
//                    break;
//                }
//            }

//            if (spaceIsClear)
//            {
//                // Set enemy position and activate
//                enemy.transform.position = spawnPoint.position;
//                enemy.transform.rotation = spawnPoint.rotation;
//                enemy.SetActive(true);

//                // reset the enemy
//                SC_EnemyHealthSystem enemyHealth = enemy.GetComponent<SC_EnemyHealthSystem>();
//                if (enemyHealth != null)
//                {
//                    enemyHealth.ResetEnemy();
//                }

//                // Debug.Log("[EnemySpawner] Spawned enemy at: " + spawnPoint.position);
//                return;
//            }
//        }
//        // not founded after 5 attemts
//        //Debug.Log("[EnemySpawner] No available space to spawn enemy.");
//    }
//    public void SetDifficulty(DifficultyLevel level)
//    {
//        currentDifficulty = level;
//    }


//}

using UnityEngine;

public class SC_EnemySpawner : MonoBehaviour
{
    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

    public SC_EnemyPool enemyPool;
    public Transform[] spawnPoints;

    private float timer;
    private int spawnedEnemiesCount = 0;
    private int maxEnemiesPerStage = 10;
    private float spawnInterval = 5f;

    void Start()
    {
        ApplyDifficultySettings();
    }

    void Update()
    {
        if (spawnedEnemiesCount < maxEnemiesPerStage)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                TrySpawn();
                timer = 0f;
            }
        }
    }

    private void TrySpawn()
    {
        if (spawnPoints.Length == 0)
            return;

        GameObject enemy = enemyPool.GetNextEnemy();
        if (enemy == null)
            return;

        for (int attempt = 0; attempt < 5; attempt++)
        {
            int index = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[index];

            float checkRadius = 1.5f;
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
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;
                enemy.SetActive(true);

                SC_EnemyHealthSystem enemyHealth = enemy.GetComponent<SC_EnemyHealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.ResetEnemy();
                }

                spawnedEnemiesCount++;
                return;
            }
        }
    }

    public void SetDifficulty(DifficultyLevel level)
    {
        currentDifficulty = level;
        ApplyDifficultySettings();
    }

    private void ApplyDifficultySettings()
    {
        timer = 0f;
        spawnedEnemiesCount = 0;

        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                maxEnemiesPerStage = 10;
                spawnInterval = 5f;
                break;
            case DifficultyLevel.Medium:
                maxEnemiesPerStage = 10;
                spawnInterval = 4f;
                break;
            case DifficultyLevel.Hard:
                maxEnemiesPerStage = 10;
                spawnInterval = 3f;
                break;
            case DifficultyLevel.Boss:
                maxEnemiesPerStage = 0;
                spawnInterval = 99f;
                break;
            default:
                maxEnemiesPerStage = 10;
                spawnInterval = 5f;
                break;
        }
    }

    public bool IsAllEnemiesDead()
    {
        int aliveEnemies = 0;
        foreach (var enemy in enemyPool.Enemies)
        {
            if (enemy.activeInHierarchy)
                aliveEnemies++;
        }
        return (spawnedEnemiesCount >= maxEnemiesPerStage && aliveEnemies == 0);
    }
}
