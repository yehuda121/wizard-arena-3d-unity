
//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class SC_PlayerHealthSystem : MonoBehaviour
//{
//    public float maxHealth = 100f;               // Maximum player health
//    private float currentHealth;                 // Current player health
//    public bool isBlocking = false;              // Whether the player is in shield mode

//    private SC_PlayerHealthBar healthBar;        // Reference to the Player HealthBar

//    void Start()
//    {
//        currentHealth = maxHealth;

//        // Try to find the health bar if it's not already assigned
//        if (healthBar == null)
//            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

//        if (healthBar != null)
//            healthBar.SetHealth(1f); // Set full health at start
//        else
//            Debug.LogWarning("PlayerHealthBar not found in scene!");
//    }

//    public void TakeDamage(float amount)
//    {
//        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();

//        // If blocking and not in boss level → ignore damage
//        if (isBlocking)
//        {
//            if (gameManager != null && gameManager.currentDifficulty != DifficultyLevel.Boss)
//                return;

//            // If blocking in boss level → reduce damage to 5%
//            if (gameManager != null && gameManager.currentDifficulty == DifficultyLevel.Boss)
//                amount = 0.05f;
//        }

//        currentHealth -= amount;

//        // Clamp health between 0 and max
//        float percent = Mathf.Clamp01(currentHealth / maxHealth);

//        // Update health bar
//        if (healthBar != null)
//            healthBar.SetHealth(percent);

//        // Update HUD health text
//        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//        if (hud != null)
//            hud.UpdateHealth(currentHealth / maxHealth);

//        // If health drops to zero or below → trigger death
//        if (currentHealth <= 0f)
//            Die();
//    }

//    public void ResetToFull()
//    {
//        currentHealth = maxHealth;

//        if (healthBar != null)
//            healthBar.SetHealth(1f);

//        // Update HUD as well
//        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//        if (hud != null)
//            hud.UpdateHealth(currentHealth / maxHealth);
//    }

//    private void Die()
//    {
//        // Trigger death animation
//        Animator anim = GetComponentInChildren<Animator>();
//        if (anim != null)
//            anim.SetTrigger("die");

//        // Pause the game immediately so the player cannot move or shoot
//        Time.timeScale = 0f;

//        // Search all objects in the scene, including inactive ones, for "GameOverText"
//        Transform[] all = GameObject.FindObjectsOfType<Transform>(true);
//        foreach (Transform t in all)
//        {
//            if (t.name == "GameOverText")
//            {
//                t.gameObject.SetActive(true); // Show the Game Over text
//                break;
//            }
//        }

//        // Start coroutine to wait and then go to settings scene
//        StartCoroutine(HandleGameOver());
//    }

//    private System.Collections.IEnumerator HandleGameOver()
//    {
//        // Wait for 3 real-time seconds (ignores timeScale = 0)
//        yield return new WaitForSecondsRealtime(3f);

//        // Set flag to skip opening video on next scene load
//        PlayerPrefs.SetInt("SkipOpeningVideo", 1);

//        // Resume normal time scale before loading next scene
//        Time.timeScale = 1f;

//        // Load the opening/settings scene
//        SceneManager.LoadScene("OpeningScene");
//    }
//}
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// This script manages the player's health, damage, shield handling, and death sequence
public class SC_PlayerHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;           // Maximum player health
    private float currentHealth;             // Current player health
    public bool isBlocking = false;          // Whether the player is currently blocking

    private SC_PlayerHealthBar healthBar;    // UI health bar
    private SC_WizardAnimator animatorController;

    void Start()
    {
        currentHealth = maxHealth;

        // Try to find the health bar if not already assigned
        if (healthBar == null)
            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

        if (healthBar != null)
            healthBar.SetHealth(1f); // Full health
        else
            Debug.LogWarning("PlayerHealthBar not found in scene!");

        animatorController = GetComponent<SC_WizardAnimator>();
    }

    public void TakeDamage(float amount)
    {
        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();

        // If blocking and not in boss stage, ignore damage
        if (isBlocking)
        {
            if (gameManager != null && gameManager.currentDifficulty != DifficultyLevel.Boss)
                return;

            // If in boss stage while blocking, reduce damage
            if (gameManager != null && gameManager.currentDifficulty == DifficultyLevel.Boss)
                amount = 0.05f;
        }

        if (currentHealth > 0)
        {
            currentHealth -= amount;
        }
        float percent = Mathf.Clamp01(currentHealth / maxHealth);

        // Update health UI
        if (healthBar != null)
            healthBar.SetHealth(percent);

        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
            hud.UpdateHealth(currentHealth / maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    public void ResetToFull()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetHealth(1f);

        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
            hud.UpdateHealth(currentHealth / maxHealth);
    }

    private void Die()
    {
        // Trigger death animation through animator controller
        animatorController?.PlayDeath();

        // Pause the game
        //Time.timeScale = 0f;

        // Show "Game Over" text
        Transform[] all = GameObject.FindObjectsOfType<Transform>(true);
        foreach (Transform t in all)
        {
            if (t.name == "GameOverText")
            {
                t.gameObject.SetActive(true);
                break;
            }
        }

        StartCoroutine(HandleGameOver());
    }

    private System.Collections.IEnumerator HandleGameOver()
    {
        // Wait 3 real-time seconds before transitioning
        yield return new WaitForSecondsRealtime(3f);

        PlayerPrefs.SetInt("SkipOpeningVideo", 1);
        Time.timeScale = 1f;
        SceneManager.LoadScene("OpeningScene");
    }
}
