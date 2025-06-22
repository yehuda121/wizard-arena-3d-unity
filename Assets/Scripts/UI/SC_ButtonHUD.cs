using UnityEngine;
using TMPro;

public class SC_ButtonHUD : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI BoostText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ScoreText;

    [Header("Stats Panel")]
    public GameObject StatsContainer;

    private float elapsedTime = 0f;
    private bool isVisible = true;

    private PlayerShooting playerShooting;
    private SC_PlayerHealthSystem playerHealth;
    private SC_GameManager gameManager;

    private DifficultyLevel lastDifficulty;

    void Start()
    {
        playerShooting = FindObjectOfType<PlayerShooting>();
        playerHealth = FindObjectOfType<SC_PlayerHealthSystem>();
        gameManager = FindObjectOfType<SC_GameManager>();

        if (gameManager != null)
        {
            lastDifficulty = gameManager.currentDifficulty;
            UpdateLevelText(lastDifficulty.ToString());
        }
    }

    void Update()
    {
        // Update timer every frame
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (TimerText != null)
            TimerText.text = $"Timer: {minutes}:{seconds:00}";

        // Update health
        if (playerHealth != null && HealthText != null)
        {
            float percent = Mathf.Clamp01(playerHealth.GetCurrentHealth() / playerHealth.maxHealth);
            int displayPercent = Mathf.RoundToInt(percent * 100f);
            HealthText.text = "Health: " + displayPercent + "%";
        }

        // Update boost and score
        if (playerShooting != null)
        {
            if (BoostText != null)
            {
                if (playerShooting.poweredUp)
                    BoostText.text = "Boost: " + Mathf.CeilToInt(playerShooting.GetRemainingBoostTime()) + "s";
                else
                    BoostText.text = "Boost: 0";
            }

            if (ScoreText != null)
            {
                ScoreText.text = "Score: " + playerShooting.score;
            }
        }

        // Update level if changed
        if (gameManager != null && gameManager.currentDifficulty != lastDifficulty)
        {
            lastDifficulty = gameManager.currentDifficulty;
            UpdateLevelText(lastDifficulty.ToString());
        }
    }

    // Updates the level text in the HUD
    private void UpdateLevelText(string levelName)
    {
        if (LevelText != null)
            LevelText.text = "Level: " + levelName;
    }

    // Toggle visibility of stats panel
    public void ToggleStatsPanel()
    {
        if (StatsContainer != null)
        {
            isVisible = !isVisible;
            StatsContainer.SetActive(isVisible);
        }
    }
}
