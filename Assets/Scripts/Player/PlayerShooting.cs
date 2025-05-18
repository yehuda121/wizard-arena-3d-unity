//using UnityEngine;

//// This script handles player shooting, power-up mechanics, and shield blocking
//public class PlayerShooting : MonoBehaviour
//{
//    [Header("Shooting Settings")]
//    public Transform shootPoint;         // Where the projectile spawns
//    public float shootForce = 15f;       // Speed of the projectile
//    public float cooldown = 0.2f;        // Delay between shots (lower = faster shooting)

//    [Header("Power-Up Settings")]
//    public int killCount = 0;            // Number of enemies killed (0-4 only for boost)
//    public bool poweredUp = false;       // Whether stronger projectiles are active
//    private float powerUpTimer = 0f;     // Timer for remaining boost time
//    public float powerUpDuration = 30f;  // Duration of the power-up in seconds

//    public int score = 0;                // Total number of enemies defeated (for score)

//    private float lastShotTime = -999f;  // Time of last shot
//    private PlayerProjectilePool projectilePool;      // Object pool for player projectiles
//    private SC_PlayerHealthSystem playerHealth;       // Reference to player health (for shield state)

//    void Start()
//    {
//        // Find the projectile pool and player health system in the scene
//        projectilePool = FindObjectOfType<PlayerProjectilePool>();
//        playerHealth = FindObjectOfType<SC_PlayerHealthSystem>();
//    }

//    //void Update()
//    //{
//    //    // Toggle blocking mode using 'S' key
//    //    if (Input.GetKeyDown(KeyCode.S))
//    //    {
//    //        playerHealth.isBlocking = true;
//    //    }
//    //    else if (Input.GetKeyUp(KeyCode.S))
//    //    {
//    //        playerHealth.isBlocking = false;
//    //    }

//    //    // Shoot if spacebar is held, enough time has passed, and the player is not blocking
//    //    if (!playerHealth.isBlocking && Input.GetKey(KeyCode.Space) && Time.time > lastShotTime + cooldown)
//    //    {
//    //        Shoot();
//    //        lastShotTime = Time.time;
//    //    }

//    //    // Update power-up timer
//    //    if (poweredUp)
//    //    {
//    //        powerUpTimer -= Time.deltaTime;

//    //        // Disable power-up when timer ends
//    //        if (powerUpTimer <= 0f)
//    //        {
//    //            poweredUp = false;
//    //        }
//    //    }

//    //    // Update HUD boost timer
//    //    SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//    //    if (hud != null)
//    //    {
//    //        hud.UpdateBoostTime(poweredUp ? powerUpTimer : 0f);
//    //    }
//    //}
//    void Update()
//    {
//        // Toggle blocking mode using 'S' key
//        if (Input.GetKeyDown(KeyCode.S))
//        {
//            playerHealth.isBlocking = true;

//            // Activate shield animation
//            Animator anim = GetComponentInChildren<Animator>();
//            if (anim != null)
//                anim.SetBool("isShielding", true);
//        }
//        else if (Input.GetKeyUp(KeyCode.S))
//        {
//            playerHealth.isBlocking = false;

//            // Deactivate shield animation
//            Animator anim = GetComponentInChildren<Animator>();
//            if (anim != null)
//                anim.SetBool("isShielding", false);
//        }

//        // Shoot if spacebar is held, cooldown passed, and not blocking
//        if (!playerHealth.isBlocking && Input.GetKey(KeyCode.Space) && Time.time > lastShotTime + cooldown)
//        {
//            Shoot();
//            lastShotTime = Time.time;
//        }

//        // Update power-up timer if active
//        if (poweredUp)
//        {
//            powerUpTimer -= Time.deltaTime;

//            // Disable power-up if timer has expired
//            if (powerUpTimer <= 0f)
//            {
//                poweredUp = false;
//            }
//        }

//        // Update HUD boost timer
//        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//        if (hud != null)
//        {
//            hud.UpdateBoostTime(poweredUp ? powerUpTimer : 0f);
//        }
//    }


//    // Called when the player reaches the kill threshold for power-up
//    public void ActivatePowerUp()
//    {
//        poweredUp = true;
//        powerUpTimer = powerUpDuration;

//        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
//        if (hud != null)
//        {
//            hud.UpdateBoostTime(powerUpTimer);
//        }
//    }

//    // Shoot a projectile using the pool
//    void Shoot()
//    {
//        // Get a projectile from the pool
//        GameObject projectile = projectilePool.GetNextProjectile();

//        // Set projectile position and rotation to match shoot point
//        projectile.transform.position = shootPoint.position;
//        projectile.transform.rotation = shootPoint.rotation;

//        // Activate the projectile in the scene
//        projectile.SetActive(true);

//        // Apply velocity to the projectile
//        Rigidbody rb = projectile.GetComponent<Rigidbody>();
//        rb.velocity = shootPoint.forward * shootForce;

//        // Add this line to trigger the animation:
//        Animator anim = GetComponentInChildren<Animator>();
//        if (anim != null)
//            anim.SetTrigger("attack");
//    }
//}
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

    public int score = 0;                // Total number of enemies defeated (for score)

    private float lastShotTime = -999f;  // Time of last shot
    private PlayerProjectilePool projectilePool;      // Object pool for player projectiles
    private SC_PlayerHealthSystem playerHealth;       // Reference to player health (for shield state)
    private SC_WizardAnimator animatorController;     // Reference to centralized animation controller

    void Start()
    {
        // Find required components in the scene
        projectilePool = FindObjectOfType<PlayerProjectilePool>();
        playerHealth = GetComponent<SC_PlayerHealthSystem>();
        animatorController = GetComponent<SC_WizardAnimator>();
    }

    void Update()
    {
        // Toggle blocking mode using 'S' key
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerHealth.isBlocking = true;
            animatorController?.SetShielding(true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            playerHealth.isBlocking = false;
            animatorController?.SetShielding(false);
        }

        // Shoot if spacebar is held, cooldown passed, and not blocking
        if (!playerHealth.isBlocking && Input.GetKey(KeyCode.Space) && Time.time > lastShotTime + cooldown)
        {
            Shoot();
            lastShotTime = Time.time;
            animatorController?.PlayAttack();
        }

        // Update power-up timer if active
        if (poweredUp)
        {
            powerUpTimer -= Time.deltaTime;

            // Disable power-up if timer has expired
            if (powerUpTimer <= 0f)
            {
                poweredUp = false;
            }
        }

        // Update HUD boost timer
        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
        {
            hud.UpdateBoostTime(poweredUp ? powerUpTimer : 0f);
        }
    }

    // Called when the player reaches the kill threshold for power-up
    public void ActivatePowerUp()
    {
        poweredUp = true;
        powerUpTimer = powerUpDuration;

        SC_GameHUD hud = FindObjectOfType<SC_GameHUD>();
        if (hud != null)
        {
            hud.UpdateBoostTime(powerUpTimer);
        }
    }

    // Shoot a projectile using the pool
    void Shoot()
    {
        // Get a projectile from the pool
        GameObject projectile = projectilePool.GetNextProjectile();

        // Set projectile position and rotation to match shoot point
        projectile.transform.position = shootPoint.position;
        projectile.transform.rotation = shootPoint.rotation;

        // Activate the projectile in the scene
        projectile.SetActive(true);

        // Apply velocity to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = shootPoint.forward * shootForce;
    }
}
