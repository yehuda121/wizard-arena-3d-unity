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

    [Header("Gameplay References")]
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private SC_PlayerHealthSystem playerHealth;
    [SerializeField] private SC_GameManager gameManager;

    private float elapsedTime = 0f;
    private bool isVisible = true;
    private DifficultyLevel lastDifficulty;

    void Start()
    {
        if (playerShooting == null)
            playerShooting = FindObjectOfType<PlayerShooting>();

        if (playerHealth == null)
            playerHealth = FindObjectOfType<SC_PlayerHealthSystem>();

        if (gameManager == null)
            gameManager = FindObjectOfType<SC_GameManager>();

        if (gameManager != null)
        {
            lastDifficulty = gameManager.currentDifficulty;
            UpdateLevelText(lastDifficulty.ToString());
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (TimerText != null)
            TimerText.text = $"Timer: {minutes}:{seconds:00}";

        if (playerHealth != null && HealthText != null)
        {
            float percent = Mathf.Clamp01(playerHealth.GetCurrentHealth() / playerHealth.maxHealth);
            int displayPercent = Mathf.RoundToInt(percent * 100f);
            HealthText.text = "Health: " + displayPercent + "%";
        }

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
                ScoreText.text = "Score: " + playerShooting.score;
        }

        if (gameManager != null && gameManager.currentDifficulty != lastDifficulty)
        {
            lastDifficulty = gameManager.currentDifficulty;
            UpdateLevelText(lastDifficulty.ToString());
        }
    }

    private void UpdateLevelText(string levelName)
    {
        if (LevelText != null)
            LevelText.text = "Level: " + levelName;
    }

    public void ToggleStatsPanel()
    {
        if (StatsContainer != null)
        {
            isVisible = !isVisible;
            StatsContainer.SetActive(isVisible);
        }
    }
}
