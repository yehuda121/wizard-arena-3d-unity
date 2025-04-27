using UnityEngine;

public class SC_Player : MonoBehaviour
{
    public float maxHealth = 100f;       // Maximum health value
    private float currentHealth;         // Current health value during gameplay

    public SC_PlayerHealthBar healthBar; // Reference to the UI health bar (assign in Inspector)

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize the health bar to full
        if (healthBar != null)
            healthBar.SetHealth(1f);
        else
            Debug.LogWarning("Player health bar is not assigned in the Inspector.");
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // TODO: Trigger Game Over logic or restart
    }
}
