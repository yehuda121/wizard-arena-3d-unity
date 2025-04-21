using UnityEngine;

public class SC_HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;           // The maximum health value
    private float currentHealth;             // The current health value during gameplay

    private SC_HealthBar healthBar;          // Reference to the HealthBar script

    void Start()
    {
        // Set current health to full at the beginning
        currentHealth = maxHealth;

        // Find the anchor point (child object) on the enemy used to position the health bar
        Transform anchor = transform.Find("HealthBarAnchor");

        // Load the EnemyHealthBar prefab from the Resources folder
        GameObject hbPrefab = Resources.Load<GameObject>("EnemyHealthBar");

        // Instantiate the health bar as a child of the UI_Health canvas in the scene
        GameObject hbInstance = Instantiate(hbPrefab, GameObject.Find("UI_Health").transform);

        // Get the SC_HealthBar script from the instantiated health bar
        healthBar = hbInstance.GetComponent<SC_HealthBar>();

        // Set the target of the health bar to follow the enemy's anchor point
        healthBar.target = anchor;
    }

    // Call this method when the enemy takes damage
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Update the health bar display
        float percent = currentHealth / maxHealth;
        if (healthBar != null)
            healthBar.SetHealth(percent);

        // Check if health dropped to zero or below
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Called when the enemy "dies"
    void Die()
    {
        Debug.Log("[HealthSystem] Enemy died.");

        // Clear any velocity before deactivating (important for pooling)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Hide the enemy
        gameObject.SetActive(false);

        // Also remove the health bar from the screen
        if (healthBar != null)
            Destroy(healthBar.gameObject);
    }
}
