using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_GameManager : MonoBehaviour
{
    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

    public SC_EnemySpawner enemySpawner;
    public GameObject bossEnemyPrefab;
    public Transform bossSpawnPoint;
    public SC_ButtonHUD ButtonHUD;

    public GameObject pauseButton;
    public GameObject startOverButton;

    private static bool isPaused = false;

    private bool bossSpawned = false;
    private PlayerShooting player;

    void Start()
    {
        // Find and assign references
        player = FindObjectOfType<PlayerShooting>();
        enemySpawner = FindObjectOfType<SC_EnemySpawner>();

        if (PlayerPrefs.HasKey("SelectedDifficulty"))
        {
            int saved = PlayerPrefs.GetInt("SelectedDifficulty", 0);
            DifficultyLevel chosenDifficulty = (DifficultyLevel)saved;
            SetDifficulty(chosenDifficulty);
        }
        else
        {
            SetDifficulty(DifficultyLevel.Easy);
        }

        InitializeStageState(player, enemySpawner);

        ResumeGame();
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    void Update()
    {
        if (isPaused || player == null)
            return;

        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                if (player.stageKillCount >= 10)
                    SetDifficulty(DifficultyLevel.Medium);
                break;
            case DifficultyLevel.Medium:
                if (player.stageKillCount >= 20)
                    SetDifficulty(DifficultyLevel.Hard);
                break;
            case DifficultyLevel.Hard:
                if (player.stageKillCount >= 30)
                    SetDifficulty(DifficultyLevel.Boss);
                break;
            case DifficultyLevel.Boss:
                break;
        }
    }

    public bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }

    public void InitializeStageState(PlayerShooting player, SC_EnemySpawner spawner)
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                player.stageKillCount = 0;
                spawner.SetSpawnedEnemies(0);
                break;
            case DifficultyLevel.Medium:
                player.stageKillCount = 10;
                spawner.SetSpawnedEnemies(10);
                break;
            case DifficultyLevel.Hard:
                player.stageKillCount = 20;
                spawner.SetSpawnedEnemies(20);
                break;
            case DifficultyLevel.Boss:
                player.stageKillCount = 30;
                spawner.SetSpawnedEnemies(30);
                break;
        }
    }


    public void SetDifficulty(DifficultyLevel level)
    {
        if(currentDifficulty == level) return;

        // set the new level
        currentDifficulty = level; 

        // if its boss level create the boss
        if (level == DifficultyLevel.Boss)
            SpawnBoss();

        PlayerPrefs.SetInt("SelectedDifficulty", (int)currentDifficulty);
        PlayerPrefs.Save();
    }


    void SpawnBoss()
    {
        if (bossSpawned) return;

        if (bossEnemyPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossEnemyPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossSpawned = true;
        }
        else
        {
            Debug.Log("bossEnemyPrefab = null or bossSpawnPoint = null");
        }
    }

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

    public void ReturnToMainMenu()
    {
        PlayerPrefs.SetInt("SkipOpeningVideo", 1);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene("OpeningScene");
    }
}

