using UnityEngine;
using System.Collections;

public class SC_BossEnemyHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private SC_EnemyHealthBar healthBar;

    [Header("References")]
    public GameObject enemyHealthBarPrefab;
    public Transform healthBarAnchor;
    public Transform healthCanvas;

    private GameObject victoryCanvas;
    private GameObject victoryTextObject;


    [HideInInspector]
    public bool isDead = false;

    void Start()
    {
        victoryCanvas = GameObject.Find("VictoryCanvas");
        if (victoryCanvas != null)
        {
            victoryTextObject = victoryCanvas.transform.Find("VictoryText")?.gameObject;
            if (victoryTextObject != null)
                victoryTextObject.SetActive(false);
        }

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
        if (isDead || currentHealth <= 0)
            return;

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
        isDead = false;
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
        isDead = true;

        SC_BossAnimator bossAnimator = GetComponent<SC_BossAnimator>();
        if (bossAnimator != null)
        {
            bossAnimator.PlayDeath();
        }

        if (healthBar != null)
            Destroy(healthBar.gameObject);

        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting != null)
        {
            playerShooting.score++;
            playerShooting.stageKillCount++;

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

        if (victoryTextObject != null)
            victoryTextObject.SetActive(true);
        else
        {
            Debug.Log("victoryTextObject = null");
        }

            StartCoroutine(DisableAfterDelay(5f));
    }

    private IEnumerator DisableAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        SC_GameManager gm = FindObjectOfType<SC_GameManager>();
        if (gm != null)
        {
            gm.ReturnToMainMenu();
        }
        else
        {
            Debug.LogWarning("[Boss] GameManager not found.");
        }
    }
}
