using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_GameManager : MonoBehaviour
{
    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

    public SC_EnemySpawner enemySpawner;
    public GameObject bossEnemyPrefab;
    public Transform bossSpawnPoint;
    public SC_GameHUD gameHUD;

    public GameObject pauseButton;       // נגרור מהיררכיה
    public GameObject startOverButton;   // נגרור מהיררכיה

    private static bool isPaused = false;

    void Start()
    {
        SetDifficulty(DifficultyLevel.Easy);
        ResumeGame(); // מבטיח שהמשחק יתחיל בריצה
    }

    void Update()
    {
        if (isPaused) return; // לא נעדכן קושי כשמושהה

        PlayerShooting player = FindObjectOfType<PlayerShooting>();
        if (player == null) return;

        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                if (player.score >= 10) SetDifficulty(DifficultyLevel.Medium);
                break;
            case DifficultyLevel.Medium:
                if (player.score >= 20) SetDifficulty(DifficultyLevel.Hard);
                break;
            case DifficultyLevel.Hard:
                if (player.score >= 30) SetDifficulty(DifficultyLevel.Boss);
                break;
            case DifficultyLevel.Boss:
                break;
        }
    }

    public void SetDifficulty(DifficultyLevel level)
    {
        currentDifficulty = level;

        if (gameHUD != null)
            gameHUD.UpdateStage(level.ToString());

        if (enemySpawner != null)
            enemySpawner.SetDifficulty(level);

        if (level == DifficultyLevel.Boss)
            SpawnBoss();
    }

    void SpawnBoss()
    {
        if (bossEnemyPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossEnemyPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        }
        else
        {
            Debug.Log("bossEnemyPrefab = null or bossSpawnPoint = null");
        }
    }

    // 🟡 פונקציות Pause / Resume / Restart
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
