// This script periodically spawns enemies at random spawn points.
// It uses the EnemyPool to retrieve enemies instead of instantiating new ones.

using UnityEngine;

public class SC_EnemySpawner : MonoBehaviour
{
    public SC_EnemyPool enemyPool;         // Reference to enemy pool
    public float spawnInterval = 3f;       // Time between spawns
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
        if (spawnPoints.Length == 0) return;

        GameObject enemy = enemyPool.GetNextEnemy();
        if (enemy == null) return;

        // נבחר עד 5 ניסיונות לנקודה רנדומלית
        for (int attempt = 0; attempt < 5; attempt++)
        {
            int index = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[index];

            // נבדוק אם האזור פנוי (בדיקה סביבתית)
            float checkRadius = 1.5f; // רדיוס בדיקה (תלוי בגודל האויב)
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

                Debug.Log("[EnemySpawner] Spawned enemy at: " + spawnPoint.position);
                return;
            }
        }

        // אם לא מצא מקום אחרי 5 ניסיונות
        Debug.Log("[EnemySpawner] No available space to spawn enemy.");
    }

}