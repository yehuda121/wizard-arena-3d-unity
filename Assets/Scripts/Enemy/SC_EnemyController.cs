//using UnityEngine;

//public class SC_EnemyController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float moveSpeed = 1f;             // Movement speed of the enemy
//    public float stopDistance = 8f;          // Stop approaching the player when this close

//    [Header("Avoidance Settings")]
//    public float avoidRadius = 8f;           // How far to check for nearby enemies to avoid
//    public LayerMask enemyLayer;             // Only consider other enemies for avoidance

//    [Header("Shooting Settings")]
//    public Transform shootPoint;             // The point the projectile spawns from
//    private float fireTimer = 0f;            // Time passed since last shot
//    private float nextFireTime = 0f;         // Time to wait before next shot

//    private Transform player;                // Reference to the player’s transform
//    private EnemyProjectilePool projectilePool;  // Reference to the projectile object pool

//    void Start()
//    {
//        // Find the player in the scene by tag
//        player = GameObject.FindWithTag("Player")?.transform;

//        if (player == null)
//        {
//            Debug.LogWarning("[EnemyController] Player not found in scene!");
//        }

//        // Get reference to the projectile pool in the scene
//        projectilePool = FindObjectOfType<EnemyProjectilePool>();

//        // Set initial fire delay between 5–15 seconds
//        nextFireTime = Random.Range(5f, 15f);
//    }

//    void Update()
//    {
//        if (player == null) return;

//        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//        // Move toward the player if not too close
//        if (distanceToPlayer > stopDistance)
//        {
//            Vector3 moveDirection = (player.position - transform.position).normalized;
//            Vector3 avoidance = CalculateAvoidance();

//            float avoidanceStrength = 1.2f;
//            Vector3 finalDirection = (moveDirection + avoidance * avoidanceStrength).normalized;

//            Vector3 nextPosition = transform.position + finalDirection * moveSpeed * Time.deltaTime;
//            nextPosition.y = transform.position.y;

//            transform.position = nextPosition;
//        }

//        // Shooting logic with randomized delay
//        fireTimer += Time.deltaTime;

//        if (fireTimer >= nextFireTime)
//        {
//            fireTimer = 0f;
//            nextFireTime = Random.Range(5f, 15f); // Next delay
//            Shoot();
//        }
//    }

//    void Shoot()
//    {
//        if (projectilePool == null || shootPoint == null || player == null)
//            return;

//        // Calculate the direction of the player to shoot
//        Vector3 targetPosition = player.position;
//        //targetPosition.y -= 1f; // Adjust target position slightly lower
//        Vector3 direction = (targetPosition - shootPoint.position).normalized;

//        // Get a projectile from the pool
//        GameObject proj = projectilePool.GetNextProjectile();
//        // Making shur that there is no valocity from previous shooting
//        Rigidbody rb = proj.GetComponent<Rigidbody>();
//        rb.isKinematic = false;
//        rb.detectCollisions = true;
//        rb.velocity = Vector3.zero;
//        rb.angularVelocity = Vector3.zero;
//        rb.drag = 0f;
//        rb.angularDrag = 0f;

//        proj.transform.position = shootPoint.position;
//        proj.transform.rotation = Quaternion.LookRotation(direction); // Rotate the projectile turds the player

//        // Gives speed to the projectile
//        rb.velocity = direction * 10f;

//        proj.SetActive(true);

//        // Drow a red line to see if the projectile hit the player for debaging
//        Debug.DrawRay(shootPoint.position, direction * 10f, Color.red, 2f);
//    }

//    // Calculate avoidance vector to prevent overlapping with other enemies
//    Vector3 CalculateAvoidance()
//    {
//        // Initialize the avoidance vector
//        Vector3 avoidance = Vector3.zero;
//        // Find all enemies within the avoidRadius
//        Collider[] hits = Physics.OverlapSphere(transform.position, avoidRadius, enemyLayer);

//        foreach (Collider hit in hits)
//        {
//            if (hit.gameObject != gameObject)// Ignore self
//            {
//                // Direction away from the other enemy
//                Vector3 pushDir = transform.position - hit.transform.position;
//                // Distance to the other enemy
//                float distance = pushDir.magnitude;

//                if (distance > 0)
//                {
//                    // Weighted push based on distance
//                    avoidance += pushDir.normalized / distance;
//                }
//            }
//        }

//        return avoidance;
//    }
//}
using UnityEngine;

public class SC_EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float stopDistance = 8f;

    [Header("Avoidance Settings")]
    public float avoidRadius = 8f;
    public LayerMask enemyLayer;

    [Header("Shooting Settings")]
    public Transform shootPoint;
    private float fireTimer = 0f;
    private float nextFireTime = 0f;

    private Transform player;
    private EnemyProjectilePool projectilePool;
    private SC_GameManager gameManager;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("[EnemyController] Player not found in scene!");
        }

        projectilePool = FindObjectOfType<EnemyProjectilePool>();
        gameManager = FindObjectOfType<SC_GameManager>();

        SetInitialFireDelay();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Movement
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

        // Shooting
        fireTimer += Time.deltaTime;
        if (fireTimer >= nextFireTime)
        {
            fireTimer = 0f;
            Shoot();
            SetNextFireDelay();
        }
    }

    void SetInitialFireDelay()
    {
        if (gameManager != null)
        {
            switch (gameManager.currentDifficulty)
            {
                case DifficultyLevel.Easy:
                    nextFireTime = Random.Range(5f, 10f); 
                    break;
                case DifficultyLevel.Medium:
                    nextFireTime = Random.Range(3f, 8f);
                    break;
                case DifficultyLevel.Hard:
                    nextFireTime = Random.Range(2f, 5f);
                    break;
                default:
                    nextFireTime = Random.Range(5f, 10f);
                    break;
            }
        }
        else
        {
            nextFireTime = Random.Range(5f, 10f);
        }
    }

    void SetNextFireDelay()
    {
        // Same as SetInitialFireDelay — use this after each shot
        SetInitialFireDelay();
    }

    void Shoot()
    {
        if (projectilePool == null || shootPoint == null || player == null)
            return;

        Vector3 targetPosition = player.position;
        Vector3 direction = (targetPosition - shootPoint.position).normalized;

        GameObject proj = projectilePool.GetNextProjectile();

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.drag = 0f;
        rb.angularDrag = 0f;

        proj.transform.position = shootPoint.position;
        proj.transform.rotation = Quaternion.LookRotation(direction);
        rb.velocity = direction * 10f;

        proj.SetActive(true);

        Debug.DrawRay(shootPoint.position, direction * 10f, Color.red, 2f);
    }

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

