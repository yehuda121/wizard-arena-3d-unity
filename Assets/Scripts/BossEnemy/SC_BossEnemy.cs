using UnityEngine;

// Boss enemy behavior - special shooting and stronger attacks
public class SC_BossEnemy : MonoBehaviour
{
    public float shootInterval = 4f; // Time between each boss shot
    private float shootTimer = 0f;

    public GameObject bossProjectilePrefab; // Prefab for boss projectiles
    public Transform BossShootPoint; // Where the projectile spawns

    private Transform player; // Reference to player
    private SC_GameManager gameManager; // To check pause state
    private SC_BossAnimator bossAnimator; // Reference to boss animator script

    private SC_BossEnemyHealthSystem bossHealthSystem;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        gameManager = FindObjectOfType<SC_GameManager>();
        bossAnimator = GetComponent<SC_BossAnimator>();
        bossHealthSystem = GetComponent<SC_BossEnemyHealthSystem>();
        if (bossHealthSystem == null)
        {
            Debug.Log("bossHealthSystem = null");
        }

        if (player == null)
            Debug.LogWarning("[BossEnemy] Player not found!");
        if (bossAnimator == null)
            Debug.LogWarning("[BossEnemy] Boss animator script not found!");
    }

    void Update()
    {
        // Do nothing if game is paused or player not found
        if (gameManager != null && gameManager.IsGamePaused()) return;
        if (player == null) return;

        //if (bossHealthSystem.currentHealth <= 0) return;
        if (bossHealthSystem != null && bossHealthSystem.isDead)
        {
            //Debug.Log("Boss is dead (health = " + bossHealthSystem.currentHealth + "), skipping Update");
            return;
        }


        // Rotate to face the player horizontally
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f; // Horizontal rotation only
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }

        // Shooting
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }
    void ShootAtPlayer()
    {

        // Play attack animation immediately
        if (bossAnimator != null)
            bossAnimator.PlayAttack();

        // Start coroutine to shoot after delay
        StartCoroutine(DelayedShoot());
    }

    private System.Collections.IEnumerator DelayedShoot()
    {
        yield return new WaitForSeconds(0.5f); // Wait before firing

        if (bossProjectilePrefab == null || BossShootPoint == null)
        {
            Debug.LogWarning("[BossEnemy] Missing projectile prefab or shoot point!");
            yield break;
        }

        GameObject proj = Instantiate(bossProjectilePrefab, BossShootPoint.position, BossShootPoint.rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = BossShootPoint.forward * 15f;
        }
        else
        {
            Debug.LogWarning("[BossEnemy] Projectile prefab is missing Rigidbody!");
        }

        //Debug.Log("[BossEnemy] Boss shot projectile.");
    }

}
