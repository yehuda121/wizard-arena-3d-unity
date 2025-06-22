using UnityEngine;

public class SC_EnemySpawner : MonoBehaviour
{
    public SC_EnemyPool enemyPool;
    public Transform[] spawnPoints;

    private float timer;
    private int spawnedEnemiesCount = 0;
    private float spawnInterval = 7f;
    private SC_GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<SC_GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }
        ApplyDifficultySettings(gameManager.currentDifficulty);

        if (PlayerPrefs.HasKey("InitialSpawnedEnemies"))
        {
            spawnedEnemiesCount = PlayerPrefs.GetInt("InitialSpawnedEnemies");
            PlayerPrefs.DeleteKey("InitialSpawnedEnemies");// clean for next use
        }
    }

    void Update()
    {
        //Debug.Log(currentDifficulty.ToString());
        //Debug.Log(currentDifficulty == DifficultyLevel.Medium && spawnedEnemiesCount >= 10 && spawnedEnemiesCount < 20);

        DifficultyLevel currentDifficulty = gameManager.currentDifficulty;

        if (currentDifficulty == DifficultyLevel.Boss)
        {
            return;
        }
        if (currentDifficulty == DifficultyLevel.Easy && spawnedEnemiesCount < 10)
        {
            spownHelper();
        } else if (currentDifficulty == DifficultyLevel.Medium && spawnedEnemiesCount >= 10 && spawnedEnemiesCount < 20)
        {
            spownHelper();
        } else if (currentDifficulty == DifficultyLevel.Hard && spawnedEnemiesCount >= 20 && spawnedEnemiesCount < 30)
        {
            spownHelper();
        }
    }
    private void spownHelper()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            TrySpawn();
            timer = 0f;
        }
    }

    private void TrySpawn()
    {
        if (spawnPoints.Length == 0)
        {
            //Debug.Log("spawnPoints.Length = 0");
            return;
        }

        GameObject enemy = enemyPool.GetNextEnemy();
        if (enemy == null)
        {
            //Debug.Log("enemy == null");
            return;
        }

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

    private void ApplyDifficultySettings(DifficultyLevel level)
    {
        timer = 0f;

        switch (level)
        {
            case DifficultyLevel.Easy:
                spawnInterval = 7f;
                break;
            case DifficultyLevel.Medium:
                spawnInterval = 6f;
                break;
            case DifficultyLevel.Hard:
                spawnInterval = 5f;
                break;
            case DifficultyLevel.Boss:
                spawnInterval = 99f;
                break;
            default:
                spawnInterval = 7f;
                break;
        }
    }
}
