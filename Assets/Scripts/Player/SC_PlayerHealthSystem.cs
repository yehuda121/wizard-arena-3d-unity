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

    public void TakeDamage(float amount)
    {
        if (isBlocking)
        {
            // shild is on
            return;
        }
        //Debug.Log("[Player] Took damage: " + amount);
        currentHealth -= amount;

        float percent = Mathf.Clamp01(currentHealth / maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(percent);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("[Player] Died.");
        // TODO: Here you can trigger Game Over screen, restart level, etc.
    }
}
