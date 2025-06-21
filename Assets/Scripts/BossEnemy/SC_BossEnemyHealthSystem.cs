using UnityEngine;
using System.Collections;

public class SC_BossEnemyHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private SC_EnemyHealthBar healthBar;

    [Header("References")]
    public GameObject enemyHealthBarPrefab;
    public Transform healthBarAnchor;
    public Transform healthCanvas;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

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
            return;

        currentHealth -= amount;
        float percent = Mathf.Clamp01(currentHealth / maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(percent);

        if (currentHealth <= 0f)
            Die();
    }

    public void ResetEnemy()
    {
        currentHealth = maxHealth;

        if (healthBar == null)
        {
            if (enemyHealthBarPrefab == null || healthCanvas == null || healthBarAnchor == null)
            {
                Debug.LogWarning("[Boss ResetEnemy] Missing references!");
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

        if (animator != null)
        {
            animator.ResetTrigger("Die"); // optional
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

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting != null)
        {
            playerShooting.score++;
            playerShooting.stageKillCount++;

            //SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
            //if (hud != null)
            //    hud.UpdateScore(playerShooting.score);

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

        // Show Victory Text when boss is defeated
        GameObject victoryText = GameObject.Find("VictoryText");
        if (victoryText != null)
        {
            victoryText.SetActive(true);
        }

        StartCoroutine(DisableAfterDelay(7f));
    }

    private IEnumerator DisableAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        // Hide the boss 
        //gameObject.SetActive(false);

        // Set flag to skip the opening video
        PlayerPrefs.SetInt("SkipOpeningVideo", 1);
        PlayerPrefs.Save();

        // Load the Opening Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("OpeningScene");
    }

}
