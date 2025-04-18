// This script provides a basic health system.
// It allows an object to take damage and be destroyed or deactivated when health reaches zero.

using UnityEngine;

public class SC_HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;       // Maximum health
    private float currentHealth;         // Current health

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("[HealthSystem] Health initialized to " + currentHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("[HealthSystem] Took damage: " + amount + " | Remaining health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("[HealthSystem] Enemy died.");

        // Clear velocity before deactivating (important for pooling)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        gameObject.SetActive(false);
    }

}