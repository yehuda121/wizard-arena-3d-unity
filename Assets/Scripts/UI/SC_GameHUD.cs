using UnityEngine;
using TMPro;

public class SC_GameHUD : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI boostText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI scoreText;

    private float elapsedTime = 0f;

    public void UpdateHealth(float percent)
    {
        int displayPercent = Mathf.RoundToInt(percent * 100f);
        healthText.text = "Health: " + displayPercent + "%";
    }

    public void UpdateBoostTime(float secondsLeft)
    {
        if (secondsLeft > 0f)
            boostText.text = "Boost: " + Mathf.CeilToInt(secondsLeft) + "s";
        else
            boostText.text = "Boost: 0";
    }
    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"Time: {minutes}:{seconds:00}";
    }


    public void UpdateStage(string stageName)
    {
        if (stageText != null)
            stageText.text = "Level: " + stageName;
    }
}
