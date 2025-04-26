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

    //void Shoot()
    //{
    //    if (projectilePool == null || shootPoint == null || player == null)
    //        return;

    //    // Get projectile from pool
    //    GameObject proj = projectilePool.GetNextProjectile();

    //    // Set projectile position at the shoot point
    //    proj.transform.position = shootPoint.position;

    //    // Calculate full 3D direction toward the player (including Y axis)
    //    //Vector3 direction = (player.position + Vector3.up * 0.5f) - shootPoint.position;
    //    //direction = direction.normalized;
    //    Vector3 direction = (player.position - shootPoint.position).normalized;

    //    // Apply velocity
    //    Rigidbody rb = proj.GetComponent<Rigidbody>();
    //    rb.velocity = direction * 10f;

    //    // Activate projectile
    //    proj.SetActive(true);
    //    Debug.DrawRay(shootPoint.position, direction * 10f, Color.red, 2f);
    //}
    //void Shoot()
    //{
    //    if (projectilePool == null || shootPoint == null || player == null)
    //        return;



    //    // Get projectile from pool
    //    GameObject proj = projectilePool.GetNextProjectile();
    //    proj.transform.position = shootPoint.position;

    //    Rigidbody rb = proj.GetComponent<Rigidbody>();
    //    rb.isKinematic = false;
    //    rb.detectCollisions = true;
    //    rb.velocity = Vector3.zero;
    //    rb.angularVelocity = Vector3.zero;
    //    rb.drag = 0f;
    //    rb.angularDrag = 0f;

    //    // Calculate full 3D direction toward the player (including Y axis)
    //    //Vector3 direction = (player.position - shootPoint.position).normalized;
    //    // סובב את נקודת הירי כך שתפנה לשחקן
    //    Vector3 direction = player.position;
    //    direction.y += 0.3f; // לכוון לגובה חזה ולא לרגליים או ראש (אפשר לשחק עם המספר)
    //    shootPoint.LookAt(direction);

    //    rb.velocity = shootPoint.forward * 10f;

    //    proj.SetActive(true);

    //    Debug.DrawRay(shootPoint.position, direction * 10f, Color.red, 2f);
    //}
    void Shoot()
    {
        if (projectilePool == null || shootPoint == null || player == null)
            return;

        // חישוב ידני של וקטור לכיוון השחקן
        Vector3 direction = (player.position - shootPoint.position).normalized;

        // קבל קליע מהפול
        GameObject proj = projectilePool.GetNextProjectile();
        proj.transform.position = shootPoint.position;
        proj.transform.rotation = Quaternion.LookRotation(direction); // לגרום לקליע להסתובב לכיוון הירי

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.drag = 0f;
        rb.angularDrag = 0f;

        // תן מהירות לפי הכיוון
        rb.velocity = direction * 10f;

        proj.SetActive(true);

        // נצייר קו לפי הכיוון האמיתי של הירי
        Debug.DrawRay(shootPoint.position, direction * 10f, Color.red, 2f);
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
