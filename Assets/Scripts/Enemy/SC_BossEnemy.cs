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

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        gameManager = FindObjectOfType<SC_GameManager>();

        if (player == null)
            Debug.LogWarning("[BossEnemy] Player not found!");
    }

    void Update()
    {
        // Do nothing if game is paused or player not found
        if (gameManager != null && gameManager.IsGamePaused()) return;
        if (player == null) return;

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
        if (bossProjectilePrefab == null || BossShootPoint == null)
        {
            if (!bossProjectilePrefab)
            {
                Debug.Log("bossProjectilePrefab == null");
            }
            else
            {
                Debug.Log("BossShootPoint == null");
            }
            return;
        }
        
        GameObject proj = Instantiate(bossProjectilePrefab, BossShootPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - BossShootPoint.position).normalized;

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * 15f;
        }
        Debug.Log("Boss is shooting");
    }
}
