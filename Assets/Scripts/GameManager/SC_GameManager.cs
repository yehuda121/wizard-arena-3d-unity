//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class SC_GameManager : MonoBehaviour
//{
//    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

//    public SC_EnemySpawner enemySpawner;
//    public GameObject bossEnemyPrefab;
//    public Transform bossSpawnPoint;
//    public SC_GameHUD gameHUD;

//    public GameObject pauseButton;       
//    public GameObject startOverButton;   

//    private static bool isPaused = false;

//    void Start()
//    {
//        if (PlayerPrefs.HasKey("SelectedDifficulty"))
//        {
//            int saved = PlayerPrefs.GetInt("SelectedDifficulty", 0);
//            DifficultyLevel chosenDifficulty = (DifficultyLevel)saved;
//            SetDifficulty(chosenDifficulty);
//        }
//        else
//        {
//            SetDifficulty(DifficultyLevel.Easy);
//        }

//        //SetDifficulty(DifficultyLevel.Easy);
//        ResumeGame(); 
//    }

//    //public void ReturnToMainMenu()
//    //{
//    //    Time.timeScale = 1f;

//    //    SceneManager.LoadScene("OpeningScene");
//    //}
//    public void ReturnToMainMenu()
//    {
//        // Set PlayerPrefs to skip video
//        PlayerPrefs.SetInt("SkipOpeningVideo", 1);
//        PlayerPrefs.Save();

//        // Load opening scene
//        Time.timeScale = 1f;
//        SceneManager.LoadScene("OpeningScene");
//    }

//    void Update()
//    {
//        if (isPaused) return; 

//        PlayerShooting player = FindObjectOfType<PlayerShooting>();
//        if (player == null) return;

//        switch (currentDifficulty)
//        {
//            case DifficultyLevel.Easy:
//                if (player.score >= 10) SetDifficulty(DifficultyLevel.Medium);
//                break;
//            case DifficultyLevel.Medium:
//                if (player.score >= 20) SetDifficulty(DifficultyLevel.Hard);
//                break;
//            case DifficultyLevel.Hard:
//                if (player.score >= 30) SetDifficulty(DifficultyLevel.Boss);
//                break;
//            case DifficultyLevel.Boss:
//                break;
//        }
//    }
//    public bool IsGamePaused()
//    {
//        return Time.timeScale == 0f;
//    }

//    public void SetDifficulty(DifficultyLevel level)
//    {
//        currentDifficulty = level;

//        if (gameHUD != null)
//            gameHUD.UpdateStage(level.ToString());

//        if (enemySpawner != null)
//            enemySpawner.SetDifficulty(level);

//        if (level == DifficultyLevel.Boss)
//            SpawnBoss();
//    }

//    void SpawnBoss()
//    {
//        if (bossEnemyPrefab != null && bossSpawnPoint != null)
//        {
//            Instantiate(bossEnemyPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
//        }
//        else
//        {
//            Debug.Log("bossEnemyPrefab = null or bossSpawnPoint = null");
//        }
//    }

//    public void TogglePause()
//    {
//        if (isPaused)
//            ResumeGame();
//        else
//            PauseGame();
//    }

//    public void PauseGame()
//    {
//        Time.timeScale = 0f;
//        isPaused = true;
//    }

//    public void ResumeGame()
//    {
//        Time.timeScale = 1f;
//        isPaused = false;
//    }

//    public void RestartGame()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }
//}


using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_GameManager : MonoBehaviour
{
    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

    public SC_EnemySpawner enemySpawner;
    public GameObject bossEnemyPrefab;
    public Transform bossSpawnPoint;
    public SC_GameHUD gameHUD;

    public GameObject pauseButton;
    public GameObject startOverButton;

    private static bool isPaused = false;

    void Start()
    {
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

        ResumeGame();
    }

    void Update()
    {
        if (isPaused)
            return;

        PlayerShooting player = FindObjectOfType<PlayerShooting>();
        if (player == null)
            return;

        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                if (enemySpawner.IsAllEnemiesDead())
                    SetDifficulty(DifficultyLevel.Medium);
                break;
            case DifficultyLevel.Medium:
                if (enemySpawner.IsAllEnemiesDead())
                    SetDifficulty(DifficultyLevel.Hard);
                break;
            case DifficultyLevel.Hard:
                if (enemySpawner.IsAllEnemiesDead())
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

    public void SetDifficulty(DifficultyLevel level)
    {
        currentDifficulty = level;

        if (gameHUD != null)
            gameHUD.UpdateStage(level.ToString());

        if (enemySpawner != null)
            enemySpawner.SetDifficulty(level);

        if (level == DifficultyLevel.Boss)
            SpawnBoss();

        UpdatePlayerScore(level);
    }

    private void UpdatePlayerScore(DifficultyLevel level)
    {
        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting == null)
            return;

        switch (level)
        {
            case DifficultyLevel.Easy:
                playerShooting.score = 0;
                break;
            case DifficultyLevel.Medium:
                playerShooting.score = 10;
                break;
            case DifficultyLevel.Hard:
                playerShooting.score = 20;
                break;
            case DifficultyLevel.Boss:
                playerShooting.score = 30;
                break;
        }

        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
            hud.UpdateScore(playerShooting.score);
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
