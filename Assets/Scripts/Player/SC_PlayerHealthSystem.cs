using UnityEngine;

// This script manages the player's health, damage, shield handling, and death sequence
public class SC_PlayerHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isBlocking = false;
    public bool isDead = false;

    [Header("References")]
    [SerializeField] private SC_PlayerHealthBar healthBar;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private SC_GameManager gameManager;

    private SC_WizardAnimator animatorController;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar == null)
            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

        if (gameManager == null)
            gameManager = FindObjectOfType<SC_GameManager>();

        if (healthBar != null)
            healthBar.SetHealth(1f);

        animatorController = GetComponent<SC_WizardAnimator>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        if (gameManager == null)
            gameManager = FindObjectOfType<SC_GameManager>();

        if (isBlocking)
        {
            if (gameManager != null && gameManager.currentDifficulty != DifficultyLevel.Boss)
                return;

            if (gameManager != null && gameManager.currentDifficulty == DifficultyLevel.Boss)
                amount = maxHealth * 0.05f;
        }

        if (currentHealth > 0)
            currentHealth -= amount;

        float percent = Mathf.Clamp01(currentHealth / maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(percent);
            if (amount > 0f)
                healthBar.FlashDamage();
        }

        if (amount > 0f)
            SC_CombatFeedback.Instance?.PlayPlayerHurt();

        if (currentHealth <= 0f)
            Die();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ResetToFull()
    {
        currentHealth = maxHealth;

        if (healthBar == null)
            healthBar = FindObjectOfType<SC_PlayerHealthBar>();

        if (healthBar != null)
            healthBar.SetHealth(1f);
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animatorController?.PlayDeath();

        if (SC_EndScreenController.Instance != null)
            SC_EndScreenController.Instance.ShowGameOver();
        else
            Debug.LogWarning("[PlayerHealth] SC_EndScreenController not found.");
    }
}
