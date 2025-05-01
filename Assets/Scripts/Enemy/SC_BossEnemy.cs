using UnityEngine;

// Boss enemy behavior - special shooting and stronger attacks
public class SC_BossEnemy : MonoBehaviour
{
    public float shootInterval = 4f; // Time between each boss shot
    private float shootTimer = 0f;

    public GameObject bossProjectilePrefab; // Prefab for boss projectiles
    public Transform shootPoint; // Where the projectile spawns

    private Transform player; // Reference to player

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
            Debug.LogWarning("[BossEnemy] Player not found!");
    }

    void Update()
    {
        if (player == null) return;

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void ShootAtPlayer()
    {
        if (bossProjectilePrefab == null || shootPoint == null)
            return;

        // Spawn the projectile
        GameObject proj = Instantiate(bossProjectilePrefab, shootPoint.position, Quaternion.identity);

        // Calculate direction toward player
        Vector3 direction = (player.position - shootPoint.position).normalized;

        // Apply force
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * 15f;
        }
    }
}
