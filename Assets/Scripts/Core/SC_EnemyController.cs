using UnityEngine;

public class SC_EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;             // Movement speed of the enemy
    public float stopDistance = 8f;          // Stop approaching the player when this close

    [Header("Avoidance Settings")]
    public float avoidRadius = 5f;           // How far to check for nearby enemies to avoid
    public LayerMask enemyLayer;             // Only consider other enemies for avoidance

    [Header("Shooting Settings")]
    public Transform shootPoint;             // The point the projectile spawns from
    private float fireTimer = 0f;            // Time passed since last shot
    private float nextFireTime = 0f;         // Time to wait before next shot

    private Transform player;                // Reference to the player’s transform
    private EnemyProjectilePool projectilePool;  // Reference to the projectile object pool

    void Start()
    {
        // Find the player in the scene by tag
        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("[EnemyController] Player not found in scene!");
        }

        // Get reference to the projectile pool in the scene
        projectilePool = FindObjectOfType<EnemyProjectilePool>();

        // Set initial fire delay between 5–15 seconds
        nextFireTime = Random.Range(5f, 15f);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Move toward the player if not too close
        if (distanceToPlayer > stopDistance)
        {
            Vector3 moveDirection = (player.position - transform.position).normalized;
            Vector3 avoidance = CalculateAvoidance();

            float avoidanceStrength = 1.2f;
            Vector3 finalDirection = (moveDirection + avoidance * avoidanceStrength).normalized;

            Vector3 nextPosition = transform.position + finalDirection * moveSpeed * Time.deltaTime;
            nextPosition.y = transform.position.y;

            transform.position = nextPosition;
        }

        // Shooting logic with randomized delay
        fireTimer += Time.deltaTime;

        if (fireTimer >= nextFireTime)
        {
            fireTimer = 0f;
            nextFireTime = Random.Range(5f, 15f); // Next delay
            Shoot();
        }
    }

    // Shoots a projectile using the pool
    //void Shoot() //enemy soot randomly and not on player
    //{
    //    if (projectilePool == null || shootPoint == null || player == null)
    //        return;

    //    // Get a projectile from the pool
    //    GameObject proj = projectilePool.GetNextProjectile();

    //    // Set position and rotation based on the shoot point
    //    proj.transform.position = shootPoint.position;
    //    proj.transform.rotation = shootPoint.rotation;

    //    // Activate the projectile
    //    proj.SetActive(true);

    //    // Launch forward in the direction the shoot point is facing
    //    Rigidbody rb = proj.GetComponent<Rigidbody>();
    //    rb.velocity = shootPoint.forward * 10f;
    //}
    void Shoot()
    {
        if (projectilePool == null || shootPoint == null || player == null)
            return;

        // Get projectile from pool
        GameObject proj = projectilePool.GetNextProjectile();

        // Set projectile position at the shoot point
        proj.transform.position = shootPoint.position;

        // Calculate horizontal direction toward player (ignoring height)
        Vector3 direction = player.position - shootPoint.position;
        direction.y = 0f; // Remove vertical difference
        direction = direction.normalized;

        // Apply velocity
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.velocity = direction * 10f;

        // Activate projectile
        proj.SetActive(true);
    }



    // Calculate avoidance vector to prevent overlapping with other enemies
    Vector3 CalculateAvoidance()
    {
        Vector3 avoidance = Vector3.zero;

        Collider[] hits = Physics.OverlapSphere(transform.position, avoidRadius, enemyLayer);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                Vector3 pushDir = transform.position - hit.transform.position;
                float distance = pushDir.magnitude;

                if (distance > 0)
                {
                    avoidance += pushDir.normalized / distance;
                }
            }
        }

        return avoidance;
    }
}
