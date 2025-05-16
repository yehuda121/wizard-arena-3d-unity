using UnityEngine;

public class SC_PlayerHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;             // Maximum health
    private float currentHealth;               // Current health
    public bool isBlocking = false;            // Shild mode

    private SC_PlayerHealthBar healthBar;       // Reference to the Player HealthBar

    void Start()
    {
        currentHealth = maxHealth;

        // Try to find the health bar automatically if not assigned
        if (healthBar == null)
            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

        if (healthBar != null)
            healthBar.SetHealth(1f); // Full health
        else
            Debug.LogWarning("PlayerHealthBar not found in scene!");
    }

    //public void TakeDamage(float amount)
    //{
    //    // Check the current difficulty level from the GameManager
    //    SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();
    //    if (gameManager != null)
    //    {
    //        return;
    //    }
    //    if (isBlocking &&  gameManager.currentDifficulty != DifficultyLevel.Boss)
    //    {
    //        // If blocking and not in Boss level then take no damage
    //        return;
    //    }

    //    // Reduce health regardless of shield — the shield logic is handled externally
    //    currentHealth -= amount;

    //    float percent = Mathf.Clamp01(currentHealth / maxHealth);

    //    if (healthBar != null)
    //        healthBar.SetHealth(percent);

    //    // Update the text on the screen
    //    SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
    //    if (hud != null)
    //        hud.UpdateHealth(currentHealth / maxHealth);

    //    if (currentHealth <= 0f)
    //    {
    //        Die();
    //    }
    //}
    public void TakeDamage(float amount)
    {
        SC_GameManager gameManager = FindObjectOfType<SC_GameManager>();

        if (isBlocking)
        {
            if (gameManager != null && gameManager.currentDifficulty != DifficultyLevel.Boss)
            {
                return; // shield blocks all regular enemy damage
            }

            if (gameManager != null && gameManager.currentDifficulty == DifficultyLevel.Boss)
            {
                amount = 0.05f; // reduce boss damage from 35% to 5%
            }
        }

        currentHealth -= amount;

        float percent = Mathf.Clamp01(currentHealth / maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(percent);

        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
            hud.UpdateHealth(currentHealth / maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }


    public void ResetToFull()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetHealth(1f);

        // Update the text on the screen
        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
            hud.UpdateHealth(currentHealth / maxHealth);
    }


    private void Die()
    {
        Debug.Log("[Player] Died.");
        // TODO: Here you can trigger Game Over screen, restart level, etc.
    }
}
