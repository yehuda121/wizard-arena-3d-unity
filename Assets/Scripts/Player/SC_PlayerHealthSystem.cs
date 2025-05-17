//using UnityEngine;

//public class SC_PlayerHealthSystem : MonoBehaviour
//{
//    public float maxHealth = 100f;             // Maximum health
//    private float currentHealth;               // Current health
//    public bool isBlocking = false;            // Shild mode

//    private SC_PlayerHealthBar healthBar;       // Reference to the Player HealthBar

//    void Start()
//    {
//        currentHealth = maxHealth;

//        // Try to find the health bar automatically if not assigned
//        if (healthBar == null)
//            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

//        if (healthBar != null)
//            healthBar.SetHealth(1f); // Full health
//        else
//            Debug.LogWarning("PlayerHealthBar not found in scene!");
//    }

//    //public void TakeDamage(float amount)
//    //{
//    //    // Check the current difficulty level from the GameManager
//    //    SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();
//    //    if (gameManager != null)
//    //    {
//    //        return;
//    //    }
//    //    if (isBlocking &&  gameManager.currentDifficulty != DifficultyLevel.Boss)
//    //    {
//    //        // If blocking and not in Boss level then take no damage
//    //        return;
//    //    }

//    //    // Reduce health regardless of shield — the shield logic is handled externally
//    //    currentHealth -= amount;

//    //    float percent = Mathf.Clamp01(currentHealth / maxHealth);

//    //    if (healthBar != null)
//    //        healthBar.SetHealth(percent);

//    //    // Update the text on the screen
//    //    SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//    //    if (hud != null)
//    //        hud.UpdateHealth(currentHealth / maxHealth);

//    //    if (currentHealth <= 0f)
//    //    {
//    //        Die();
//    //    }
//    //}
//    public void TakeDamage(float amount)
//    {
//        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();

//        if (isBlocking)
//        {
//            if (gameManager != null && gameManager.currentDifficulty != DifficultyLevel.Boss)
//            {
//                return; // shield blocks all regular enemy damage
//            }

//            if (gameManager != null && gameManager.currentDifficulty == DifficultyLevel.Boss)
//            {
//                amount = 0.05f; // reduce boss damage from 35% to 5%
//            }
//        }

//        currentHealth -= amount;

//        float percent = Mathf.Clamp01(currentHealth / maxHealth);

//        if (healthBar != null)
//            healthBar.SetHealth(percent);

//        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//        if (hud != null)
//            hud.UpdateHealth(currentHealth / maxHealth);

//        if (currentHealth <= 0f)
//        {
//            Die();
//        }
//    }


//    public void ResetToFull()
//    {
//        currentHealth = maxHealth;
//        if (healthBar != null)
//            healthBar.SetHealth(1f);

//        // Update the text on the screen
//        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//        if (hud != null)
//            hud.UpdateHealth(currentHealth / maxHealth);
//    }


//    private void Die()
//    {
//        Debug.Log("[Player] Died.");

//        // הצגת טקסט Game Over
//        GameObject goText = GameObject.Find("GameOverText");
//        if (goText != null)
//        {
//            Debug.Log("GameOverText FOUND");
//            goText.SetActive(true);
//        }

//        // התחלת Coroutine שמעביר לסצנת ההגדרות אחרי 3 שניות
//        StartCoroutine(HandleGameOver());
//    }

//    private System.Collections.IEnumerator HandleGameOver()
//    {
//        yield return new WaitForSeconds(3f);

//        // כדי לא להפעיל שוב את הווידאו נטעין את Scene ההגדרות אבל נדלג על הסרטון
//        PlayerPrefs.SetInt("SkipOpeningVideo", 1); // נשמור דגל

//        UnityEngine.SceneManagement.SceneManager.LoadScene("OpeningScene");
//    }

//}
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SC_PlayerHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;               // Maximum player health
    private float currentHealth;                 // Current player health
    public bool isBlocking = false;              // Whether the player is in shield mode

    private SC_PlayerHealthBar healthBar;        // Reference to the Player HealthBar

    void Start()
    {
        currentHealth = maxHealth;

        // Try to find the health bar if it's not already assigned
        if (healthBar == null)
            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

        if (healthBar != null)
            healthBar.SetHealth(1f); // Set full health at start
        else
            Debug.LogWarning("PlayerHealthBar not found in scene!");
    }

    public void TakeDamage(float amount)
    {
        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();

        // If blocking and not in boss level → ignore damage
        if (isBlocking)
        {
            if (gameManager != null && gameManager.currentDifficulty != DifficultyLevel.Boss)
                return;

            // If blocking in boss level → reduce damage to 5%
            if (gameManager != null && gameManager.currentDifficulty == DifficultyLevel.Boss)
                amount = 0.05f;
        }

        currentHealth -= amount;

        // Clamp health between 0 and max
        float percent = Mathf.Clamp01(currentHealth / maxHealth);

        // Update health bar
        if (healthBar != null)
            healthBar.SetHealth(percent);

        // Update HUD health text
        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
            hud.UpdateHealth(currentHealth / maxHealth);

        // If health drops to zero or below → trigger death
        if (currentHealth <= 0f)
            Die();
    }

    public void ResetToFull()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetHealth(1f);

        // Update HUD as well
        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
            hud.UpdateHealth(currentHealth / maxHealth);
    }

    private void Die()
    {
        // Pause the game immediately so the player cannot move or shoot
        Time.timeScale = 0f;

        // Search all objects in the scene, including inactive ones, for "GameOverText"
        Transform[] all = GameObject.FindObjectsOfType<Transform>(true);
        foreach (Transform t in all)
        {
            if (t.name == "GameOverText")
            {
                t.gameObject.SetActive(true); // Show the Game Over text
                break;
            }
        }

        // Start coroutine to wait and then go to settings scene
        StartCoroutine(HandleGameOver());
    }

    private System.Collections.IEnumerator HandleGameOver()
    {
        // Wait for 3 real-time seconds (ignores timeScale = 0)
        yield return new WaitForSecondsRealtime(3f);

        // Set flag to skip opening video on next scene load
        PlayerPrefs.SetInt("SkipOpeningVideo", 1);

        // Resume normal time scale before loading next scene
        Time.timeScale = 1f;

        // Load the opening/settings scene
        SceneManager.LoadScene("OpeningScene");
    }
}
