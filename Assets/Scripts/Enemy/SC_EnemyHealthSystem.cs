//using UnityEngine;
//using System.Collections;

//public class SC_EnemyHealthSystem : MonoBehaviour
//{
//    public float maxHealth = 100f;               // The maximum health value

//    private float currentHealth;                 // Tracks current health during gameplay
//    private SC_EnemyHealthBar healthBar;         // Reference to the instantiated health bar

//    [Header("References")]
//    public GameObject enemyHealthBarPrefab;      // Prefab for the health bar
//    public Transform healthBarAnchor;            // The point on the enemy where the health bar should follow
//    public Transform healthCanvas;               // The world-space canvas where the health bar will be attached

//    //void Start()
//    //{
//    //    currentHealth = maxHealth;

//    //    // Fallback: Try to find canvas by name if not assigned
//    //    if (healthCanvas == null)
//    //    {
//    //        GameObject canvasObj = GameObject.Find("UI_HealthEnemy");
//    //        if (canvasObj != null)
//    //        {
//    //            healthCanvas = canvasObj.transform;
//    //        }
//    //    }


//    //    Canvas canvas = GetComponentInChildren<Canvas>();
//    //    if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
//    //    {
//    //        canvas.worldCamera = Camera.main;
//    //    }

//    //    // Validate references
//    //    if (enemyHealthBarPrefab == null)
//    //        Debug.LogWarning("[ResetEnemy] enemyHealthBarPrefab is NULL!");
//    //    if (healthCanvas == null)
//    //        Debug.LogWarning("[ResetEnemy] healthCanvas is NULL!");
//    //    if (healthBarAnchor == null)
//    //        Debug.LogWarning("[ResetEnemy] HealthBarAnchor is NULL!");


//    //    // Instantiate the health bar under the canvas
//    //    GameObject hbInstance = Instantiate(enemyHealthBarPrefab, healthCanvas);

//    //    // Get the health bar script component
//    //    healthBar = hbInstance.GetComponent<SC_EnemyHealthBar>();

//    //    // Attach the health bar to the enemy's anchor point
//    //    if (healthBar != null)
//    //    {
//    //        healthBar.target = healthBarAnchor;  
//    //        healthBar.SetHealth(1f); // Full health on start
//    //    }
//    //}

//    void Start()
//    {
//        currentHealth = maxHealth;

//        healthBar = GetComponentInChildren<SC_EnemyHealthBar>();

//        if (healthBar != null)
//        {
//            healthBar.target = healthBarAnchor;
//            healthBar.SetHealth(1f);
//        }
//    }


//    // Method to apply damage
//    public void TakeDamage(float amount)
//    {
//        currentHealth -= amount;
//        //Debug.Log($"Enemy took {amount} damage. Current: {currentHealth}");

//        // Update health bar fill
//        float percent = Mathf.Clamp01(currentHealth / maxHealth);
//        if (healthBar != null)
//            healthBar.SetHealth(percent);

//        // Check for death
//        if (currentHealth <= 0f)
//        {
//            Die();
//        }
//    }

//    // Method to reset enemy (e.g. from object pool)
//    public void ResetEnemy()
//    {
//        currentHealth = maxHealth;

//        // If health bar was destroyed, re-create it
//        if (healthBar == null)
//        {
//            if (enemyHealthBarPrefab == null || healthCanvas == null || healthBarAnchor == null)
//            {
//                Debug.LogWarning("[ResetEnemy] Missing references!");
//                return;
//            }

//            GameObject hbInstance = Instantiate(enemyHealthBarPrefab, healthCanvas);
//            healthBar = hbInstance.GetComponent<SC_EnemyHealthBar>();

//            if (healthBar != null)
//            {
//                healthBar.target = healthBarAnchor;
//                healthBar.SetHealth(1f);
//            }
//        }
//        else
//        {
//            // Reset fill to full health
//            healthBar.SetHealth(1f);
//        }
//    }

//    // Called when the enemy dies
//    void Die()
//    {
//        // Stop physics movement
//        Rigidbody rb = GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            rb.velocity = Vector3.zero;
//            rb.angularVelocity = Vector3.zero;
//        }

//        // Play death animation
//        SC_EnemyAnimator animator = GetComponent<SC_EnemyAnimator>();
//        if (animator != null)
//        {
//            animator.PlayDeath();
//        }

//        // Destroy health bar UI
//        if (healthBar != null)
//            Destroy(healthBar.gameObject);

//        // Update score and power-up system
//        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
//        if (playerShooting != null)
//        {
//            playerShooting.score++;

//            SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//            if (hud != null)
//                hud.UpdateScore(playerShooting.score);

//            if (!playerShooting.poweredUp)
//            {
//                playerShooting.killCount++;
//                if (playerShooting.killCount >= 4)
//                {
//                    playerShooting.killCount = 0;
//                    playerShooting.ActivatePowerUp();
//                }
//            }
//        }

//        // Start coroutine to disable object after short delay
//        StartCoroutine(DisableAfterDelay(3f));
//    }

//    // Coroutine to delay object deactivation
//    private IEnumerator DisableAfterDelay(float delaySeconds)
//    {
//        yield return new WaitForSeconds(delaySeconds);
//        gameObject.SetActive(false);
//    }
//}


using UnityEngine;
using System.Collections;

public class SC_EnemyHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;

    private float currentHealth;
    private SC_EnemyHealthBar healthBar;

    [Header("References")]
    public GameObject enemyHealthBarPrefab;
    public Transform healthBarAnchor;
    public Transform healthCanvas;

    void Start()
    {
        currentHealth = maxHealth;

        healthBar = GetComponentInChildren<SC_EnemyHealthBar>();
        if (healthBar != null)
        {
            healthBar.target = healthBarAnchor;
            healthBar.SetHealth(1f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        float percent = Mathf.Clamp01(currentHealth / maxHealth);
        if (healthBar != null)
            healthBar.SetHealth(percent);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void ResetEnemy()
    {
        currentHealth = maxHealth;

        if (healthBar == null)
        {
            if (enemyHealthBarPrefab == null || healthCanvas == null || healthBarAnchor == null)
            {
                Debug.LogWarning("[ResetEnemy] Missing references!");
                return;
            }

            GameObject hbInstance = Instantiate(enemyHealthBarPrefab, healthCanvas);
            healthBar = hbInstance.GetComponent<SC_EnemyHealthBar>();

            if (healthBar != null)
            {
                healthBar.target = healthBarAnchor;
                healthBar.SetHealth(1f);
            }
        }
        else
        {
            healthBar.SetHealth(1f);
        }
    }

    void Die()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        SC_EnemyAnimator animator = GetComponent<SC_EnemyAnimator>();
        if (animator != null)
        {
            animator.PlayDeath();
        }

        // Inform controller to stop movement and shooting after death
        SC_EnemyController controller = GetComponent<SC_EnemyController>();
        if (controller != null)
        {
            controller.Die();
        }

        if (healthBar != null)
            Destroy(healthBar.gameObject);

        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting != null)
        {
            playerShooting.score++;

            SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
            if (hud != null)
                hud.UpdateScore(playerShooting.score);

            if (!playerShooting.poweredUp)
            {
                playerShooting.killCount++;
                if (playerShooting.killCount >= 4)
                {
                    playerShooting.killCount = 0;
                    playerShooting.ActivatePowerUp();
                }
            }
        }

        StartCoroutine(DisableAfterDelay(3f));
    }

    private IEnumerator DisableAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        gameObject.SetActive(false);
    }
}

