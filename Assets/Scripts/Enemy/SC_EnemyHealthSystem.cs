using UnityEngine;

public class SC_EnemyHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;               // The maximum health value
    private float currentHealth;                 // Tracks current health value during gameplay

    private SC_EnemyHealthBar healthBar;         // Reference to the attached enemy health bar

    void Start()
    {
        // Set starting health
        currentHealth = maxHealth;

        // Find the anchor point on the enemy used to position the health bar
        Transform anchor = transform.Find("HealthBarAnchor");

        // Load the EnemyHealthBar prefab from the Resources folder
        GameObject hbPrefab = Resources.Load<GameObject>("EnemyHealthBar");

        // Find the canvas where enemy health bars should appear
        GameObject healthCanvas = GameObject.Find("UI_HealthEnemy");

        // Validate references
        if (hbPrefab == null)
        {
            Debug.LogError("EnemyHealthBar prefab not found in Resources!");
            return;
        }

        if (healthCanvas == null)
        {
            Debug.LogError("UI_HealthEnemy canvas not found in scene!");
            return;
        }

        if (anchor == null)
        {
            Debug.LogError("HealthBarAnchor not found inside enemy prefab!");
            return;
        }

        // Instantiate health bar under the enemy health canvas
        GameObject hbInstance = Instantiate(hbPrefab, healthCanvas.transform);

        // Get the SC_EnemyHealthBar component from the prefab
        healthBar = hbInstance.GetComponent<SC_EnemyHealthBar>();

        // Link the bar to the enemy's anchor
        if (healthBar != null)
        {
            healthBar.target = anchor;
            healthBar.SetHealth(1f); // full health
        }
    }

    // Call this method to apply damage to the enemy
    public void TakeDamage(float amount)
    {
        //Debug.Log("Enemy took damage: " + amount);
        currentHealth -= amount;

        // Update the health bar fill
        float percent = Mathf.Clamp01(currentHealth / maxHealth);
        if (healthBar != null)
            healthBar.SetHealth(percent);

        // Kill the enemy if health is depleted
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Called when the enemy dies
    void Die()
    {
        //Debug.Log("[EnemyHealthSystem] Enemy died.");

        // Stop all motion
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Remove the health bar from UI
        if (healthBar != null)
            Destroy(healthBar.gameObject);

        // Hide the enemy
        gameObject.SetActive(false);
    }
}

