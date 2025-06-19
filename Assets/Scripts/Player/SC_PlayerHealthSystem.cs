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
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Try to find the health bar object in the scene if not already assigned
        if (healthBar == null)
            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

        // Initialize the health bar to full health if found
        if (healthBar != null)
            healthBar.SetHealth(1f); // 100% filled

        // Get the reference to the wizard's animation controller
        animatorController = GetComponent<SC_WizardAnimator>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();

        // If blocking and not in boss stage, ignore damage
        if (isBlocking)
        {
            if (gameManager != null && gameManager.currentDifficulty != DifficultyLevel.Boss)
                return;

            // If in boss stage while blocking, reduce damage
            if (gameManager != null && gameManager.currentDifficulty == DifficultyLevel.Boss)
                amount = maxHealth * 0.05f;
        }

        if (currentHealth > 0)
        {
            currentHealth -= amount;
        }
        float percent = Mathf.Clamp01(currentHealth / maxHealth);

        // Update health UI
        if (healthBar != null)
            healthBar.SetHealth(percent);

        //SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        //if (hud != null)
        //    hud.UpdateHealth(currentHealth / maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    public void ResetToFull()
    {
        currentHealth = maxHealth;

        // Reassign the health bar if it's missing (fallback)
        if (healthBar == null)
            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

        // Update the UI health bar
        if (healthBar != null)
            healthBar.SetHealth(1f); // full again

        // Update the HUD value (e.g. percent text or visual)
        //SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        //if (hud != null)
        //    hud.UpdateHealth(currentHealth / maxHealth);
    }


    private void Die()
    {
        if(isDead) return;

        // Trigger death animation through animator controller
        isDead = true;
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
