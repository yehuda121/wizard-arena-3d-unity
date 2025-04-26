using UnityEngine;

public class SC_Player : MonoBehaviour
{
    public float maxHealth = 100f;                   // Maximum health value
    private float currentHealth;                     // Current health value during gameplay

    public SC_PlayerHealthBar healthBar;             // Reference to the UI health bar (assign in Inspector)

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize the health bar to full
        if (healthBar != null)
            healthBar.SetHealth(1f);
        else
            Debug.LogWarning("Player health bar is not assigned in the Inspector.");
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        float percent = Mathf.Clamp01(currentHealth / maxHealth); // Clamp to 0-1 range
        if (healthBar != null)
            healthBar.SetHealth(percent);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // TODO: Trigger Game Over logic or restart
    }
}
