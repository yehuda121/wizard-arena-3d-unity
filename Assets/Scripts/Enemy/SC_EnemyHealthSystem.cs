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
        if (currentHealth <= 0)
        {
            return;
        }
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

        // Reset health bar or create if missing
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

        // Reset controller state (prevent shooting after respawn)
        SC_EnemyController controller = GetComponent<SC_EnemyController>();
        if (controller != null)
        {
            controller.ResetEnemy();
        }
    }

    //void Die()
    //{
    //    // Stop rigidbody movement
    //    Rigidbody rb = GetComponent<Rigidbody>();
    //    if (rb != null)
    //    {
    //        rb.velocity = Vector3.zero;
    //        rb.angularVelocity = Vector3.zero;
    //    }

    //    // Play death animation
    //    SC_EnemyAnimator animator = GetComponent<SC_EnemyAnimator>();
    //    if (animator != null)
    //    {
    //        animator.PlayDeath();
    //    }

    //    // Inform controller to stop movement and shooting after death
    //    SC_EnemyController controller = GetComponent<SC_EnemyController>();
    //    if (controller != null)
    //    {
    //        controller.Die();
    //    }

    //    // Destroy health bar object
    //    if (healthBar != null)
    //        Destroy(healthBar.gameObject);

    //    // Update player score and power-up system
    //    PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
    //    if (playerShooting != null)
    //    {
    //        playerShooting.score++;
    //        playerShooting.stageKillCount++;

    //        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
    //        if (hud != null)
    //            hud.UpdateScore(playerShooting.score);

    //        if (!playerShooting.poweredUp)
    //        {
    //            playerShooting.killCount++;
    //            if (playerShooting.killCount >= 4)
    //            {
    //                playerShooting.killCount = 0;
    //                playerShooting.ActivatePowerUp();
    //            }
    //        }
    //    }

    //    StartCoroutine(DisableAfterDelay(3f));
    //    //Debug.Log("[Enemy] Died: " + gameObject.name);
    //}

    void Die()
    {
        // Stop rigidbody movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Play death animation
        // Try BossAnimator first
        SC_BossAnimator bossAnimator = GetComponent<SC_BossAnimator>();
        if (bossAnimator != null)
        {
            bossAnimator.PlayDeath();
        }
        else
        {
            // If not boss, use regular enemy animator
            SC_EnemyAnimator enemyAnimator = GetComponent<SC_EnemyAnimator>();
            if (enemyAnimator != null)
            {
                enemyAnimator.PlayDeath();
            }
        }

        // Inform controller to stop movement and shooting after death
        SC_EnemyController controller = GetComponent<SC_EnemyController>();
        if (controller != null)
        {
            controller.Die();
        }

        // Destroy health bar object
        if (healthBar != null)
            Destroy(healthBar.gameObject);

        // Update player score and power-up system
        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting != null)
        {
            playerShooting.score++;
            playerShooting.stageKillCount++;

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

