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
    public GameObject StatsContainer; // contain - Boost, Score ...'

    private float elapsedTime = 0f;
    private bool isVisible = true;

    private PlayerShooting playerShooting;
    private SC_PlayerHealthSystem playerHealth;

    void Start()
    {
        playerShooting = FindObjectOfType<PlayerShooting>();
        playerHealth = FindObjectOfType<SC_PlayerHealthSystem>();
    }

    void Update()
    {
        // Update timer every frame
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (TimerText != null)
            TimerText.text = $"Timer: {minutes}:{seconds:00}";

        // Update health from SC_PlayerHealthSystem
        if (playerHealth != null && HealthText != null)
        {
            float percent = Mathf.Clamp01(playerHealth.GetCurrentHealth() / playerHealth.maxHealth);
            int displayPercent = Mathf.RoundToInt(percent * 100f);
            HealthText.text = "Health: " + displayPercent + "%";
        }

        // Update boost time and score from PlayerShooting
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
    }

    public void OnDropdownLevelChanged(int index)
    {
        DifficultyLevel selected = DifficultyLevel.Easy;

        switch (index)
        {
            case 0:
                selected = DifficultyLevel.Easy;
                break;
            case 1:
                selected = DifficultyLevel.Medium;
                break;
            case 2:
                selected = DifficultyLevel.Hard;
                break;
            case 3:
                selected = DifficultyLevel.Boss;
                break;
            default:
                Debug.LogWarning("Invalid difficulty index from dropdown.");
                break;
        }

        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();
        if (gameManager != null)
        {
            gameManager.SetDifficulty(selected);
        }
    }

    public void UpdateLevel(string levelName)
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
