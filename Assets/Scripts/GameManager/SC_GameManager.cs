using UnityEngine;

// This script manages game difficulty progression and triggers the boss stage
public class SC_GameManager : MonoBehaviour
{
    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

    public SC_EnemySpawner enemySpawner;       // Reference to the enemy spawner
    public GameObject bossEnemyPrefab;         // Boss prefab to spawn during Boss stage
    public Transform bossSpawnPoint;           // Where to spawn the boss
    public SC_GameHUD gameHUD;                 // Reference to HUD controller

    void Start()
    {
        // Set initial difficulty to Easy at game start
        SetDifficulty(DifficultyLevel.Easy);
    }

    void Update()
    {
        // calculate the current level
        PlayerShooting player = FindObjectOfType<PlayerShooting>();
        if (player == null) return;

        // move forward in difficulty
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                if (player.score >= 10)
                    SetDifficulty(DifficultyLevel.Medium);
                break;

            case DifficultyLevel.Medium:
                if (player.score >= 20)
                    SetDifficulty(DifficultyLevel.Hard);
                break;

            case DifficultyLevel.Hard:
                if (player.score >= 30)
                    SetDifficulty(DifficultyLevel.Boss);
                break;

            case DifficultyLevel.Boss:
                // Do nothing, boss already active
                break;
        }
    }


    // Apply difficulty changes and trigger UI/boss spawn
    public void SetDifficulty(DifficultyLevel level)
    {
        currentDifficulty = level;

        // Update stage text on HUD
        if (gameHUD != null)
        {
            gameHUD.UpdateStage(level.ToString());
        }

        // Notify spawner of difficulty change
        if (enemySpawner != null)
            enemySpawner.SetDifficulty(level);

        // Spawn the boss if we reached Boss stage
        if (level == DifficultyLevel.Boss)
            SpawnBoss();
    }

    // Spawn boss enemy at predefined location
    void SpawnBoss()
    {
        if (bossEnemyPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossEnemyPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        }
    }
}
 