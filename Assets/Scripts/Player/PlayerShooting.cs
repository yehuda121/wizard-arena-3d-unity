using UnityEngine;

// This script handles player shooting, power-up mechanics, and shield blocking
public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Transform shootPoint;         // Where the projectile spawns
    public float shootForce = 15f;       // Speed of the projectile
    public float cooldown = 0.2f;        // Delay between shots (lower = faster shooting)

    [Header("Power-Up Settings")]
    public int killCount = 0;            // Number of enemies killed (0-4 only for boost)
    public bool poweredUp = false;       // Whether stronger projectiles are active
    private float powerUpTimer = 0f;     // Timer for remaining boost time
    public float powerUpDuration = 30f;  // Duration of the power-up in seconds
    public float powerUpDamageMultiplier = 2f;

    public int score = 0;                // Total number of enemies defeated (for score)
    public int stageKillCount = 0;       // For tracking kills during each stage

    private float lastShotTime = -999f;  // Time of last shot
    private PlayerProjectilePool projectilePool;
    private SC_PlayerHealthSystem playerHealth;
    private SC_WizardAnimator animatorController;

    public AudioClip spellShootClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("audioSource = null");
        }

        projectilePool = FindObjectOfType<PlayerProjectilePool>();
        playerHealth = GetComponent<SC_PlayerHealthSystem>();
        animatorController = GetComponent<SC_WizardAnimator>();

        // Initialize stageKillCount based on selected difficulty
        if (PlayerPrefs.HasKey("SelectedDifficulty"))
        {
            int saved = PlayerPrefs.GetInt("SelectedDifficulty", 0);
            switch (saved)
            {
                case 0: stageKillCount = 0; break;  // Easy
                case 1: stageKillCount = 10; break; // Medium
                case 2: stageKillCount = 20; break; // Hard
                case 3: stageKillCount = 30; break; // Boss
            }
        }

        // Reset global flags
        PlayerState.isShooting = false;
        PlayerState.isBlocking = false;
    }

    void Update()
    {
        if (playerHealth.isDead)
            return;

        PlayerState.isDead = false;

        SC_MobileInputController mobileInput = SC_MobileInputController.Instance;
        bool shieldActive = Input.GetKey(KeyCode.S) ||
            (mobileInput != null && mobileInput.ShieldPressed);

        if (shieldActive != playerHealth.isBlocking)
        {
            playerHealth.isBlocking = shieldActive;
            PlayerState.isBlocking = shieldActive;
            animatorController?.SetShielding(shieldActive);
        }

        bool shootInput = Input.GetKey(KeyCode.Space) ||
            (mobileInput != null && mobileInput.ShootPressed);

        // Shoot if spacebar or mobile shoot is held, cooldown passed, and not blocking
        if (!playerHealth.isBlocking && shootInput && Time.time > lastShotTime + cooldown)
        {
            PlayerState.isShooting = true;
            Shoot();
            lastShotTime = Time.time;
            animatorController?.PlayAttack();
        }
        else
        {
            PlayerState.isShooting = false;
        }

        // Update power-up timer if active
        if (poweredUp)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                poweredUp = false;
            }
        }

    }

    // Called when the player reaches the kill threshold for power-up
    public void ActivatePowerUp()
    {
        poweredUp = true;
        powerUpTimer = powerUpDuration;
    }

    public float GetRemainingBoostTime()
    {
        return powerUpTimer;
    }


    // Shoot a projectile using the pool
    void Shoot()
    {
        GameObject projectile = projectilePool.GetNextProjectile();

        projectile.transform.position = shootPoint.position;
        projectile.transform.rotation = shootPoint.rotation;

        projectile.SetActive(true);

        SC_MagicProjectile magicProjectile = projectile.GetComponent<SC_MagicProjectile>();
        if (magicProjectile != null)
            magicProjectile.damageMultiplier = poweredUp ? powerUpDamageMultiplier : 1f;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = shootPoint.forward * shootForce;

        audioSource.PlayOneShot(spellShootClip);
    }
}
