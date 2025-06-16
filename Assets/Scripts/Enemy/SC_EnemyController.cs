//using System.Linq;
//using UnityEngine;

//public class SC_EnemyController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float moveSpeed = 1f;               // Movement speed of the enemy
//    public float stopDistance = 8f;            // Minimum distance to stop moving toward the player

//    [Header("Avoidance Settings")]
//    public float avoidRadius = 8f;             // Radius to detect nearby enemies to avoid overlapping
//    public LayerMask enemyLayer;               // Layer used to detect other enemies

//    [Header("Shooting Settings")]
//    public Transform enemyShootPoint;               // The point where projectiles are spawned
//    private float fireTimer = 0f;              // Time counter between shots
//    private float nextFireTime = 0f;           // Time to wait before the next shot

//    private Transform player;                  // Reference to the player's position
//    private EnemyProjectilePool projectilePool; // Reference to projectile object pool
//    private SC_GameManager gameManager;        // Reference to the GameManager
//    private SC_EnemyAnimator enemyAnimator;    // Reference to the enemy animator script

//    private float debugInterval = 3f;
//    private float debugTimer = 0f;

//    void Start()
//    {
//        // Get references
//        player = GameObject.FindWithTag("Player")?.transform;
//        if (player == null)
//        {
//            Debug.LogWarning("[EnemyController] Player not found in scene!");
//        }

//        projectilePool = FindObjectOfType<EnemyProjectilePool>();
//        gameManager = FindObjectOfType<SC_GameManager>();
//        enemyAnimator = GetComponent<SC_EnemyAnimator>();

//        //enemyShootPoint = transform.parent.Find("enemyShootPoint");
//        enemyShootPoint = transform.Find("enemyShootPoint");

//        if (enemyShootPoint == null)
//            Debug.LogWarning($"[{name}] enemyShootPoint NOT FOUND");
//        else
//            Debug.Log($"[{name}] enemyShootPoint FOUND at {enemyShootPoint.position}");

//        //foreach (Transform t in GetComponentsInChildren<Transform>(true))
//        //{
//        //    Debug.Log($"[{name}] CHILD: {t.name}");
//        //}

//        SetInitialFireDelay();
//    }

//    void Update()
//    {
//        debugTimer += Time.deltaTime;
//        if (debugTimer >= debugInterval)
//        {
//            debugTimer = 0f;

//            Vector3 enemyPos = transform.position;
//            Vector3 shootPointPos = enemyShootPoint != null ? enemyShootPoint.position : Vector3.zero;

//            //Debug.Log($"[{name}] Enemy position: {enemyPos}, enemyShootPoint: {shootPointPos}");
//        }

//        if (player == null) return;

//        // DISTANCE TO PLAYER
//        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//        // Determine whether the enemy should walk or stop
//        bool shouldWalk = distanceToPlayer > stopDistance;
//        //Debug.Log("Speed = " + (shouldWalk ? "WALKING" : "IDLE"));

//        // Set walking animation (true if moving, false if idle)
//        if (enemyAnimator != null)
//        {
//            enemyAnimator.SetWalking(shouldWalk);
//        }

//        // === MOVEMENT LOGIC ===
//        if (shouldWalk)
//        {
//            // Calculate direction toward the player
//            Vector3 moveDirection = (player.position - transform.position).normalized;

//            // Calculate avoidance from nearby enemies
//            Vector3 avoidance = CalculateAvoidance();
//            float avoidanceStrength = 3f;

//            // Combine movement and avoidance
//            Vector3 finalDirection = (moveDirection + avoidance * avoidanceStrength).normalized;

//            // Smoothly rotate to face movement direction
//            if (finalDirection != Vector3.zero)
//            {
//                Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
//                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
//            }

//            // Move the enemy forward
//            Vector3 moveDelta = finalDirection * moveSpeed * Time.deltaTime;
//            transform.position += moveDelta;
//        }

//        // === SHOOTING LOGIC ===
//        fireTimer += Time.deltaTime;

//        // Fire projectile when delay has passed
//        if (fireTimer >= nextFireTime)
//        {
//            fireTimer = 0f;
//            Shoot();
//            SetNextFireDelay();
//        }

//        // Only clamp Y position if enemy is alive and on ground
//        if (isActiveAndEnabled) // Avoids clamping dead/falling enemies
//        {
//            Vector3 pos = transform.position;
//            pos.y = -0.5f; // depending on your floor
//            transform.position = pos;
//        }
//    }


//    // Sets the initial fire delay based on current difficulty
//    void SetInitialFireDelay()
//    {
//        if (gameManager != null)
//        {
//            switch (gameManager.currentDifficulty)
//            {
//                case DifficultyLevel.Easy:
//                    nextFireTime = Random.Range(5f, 10f);
//                    break;
//                case DifficultyLevel.Medium:
//                    nextFireTime = Random.Range(3f, 8f);
//                    break;
//                case DifficultyLevel.Hard:
//                    nextFireTime = Random.Range(2f, 5f);
//                    break;
//                default:
//                    nextFireTime = Random.Range(5f, 10f);
//                    break;
//            }
//        }
//        else
//        {
//            nextFireTime = Random.Range(5f, 10f);
//        }
//    }

//    // Used after each shot to determine delay until next shot
//    void SetNextFireDelay()
//    {
//        SetInitialFireDelay();
//    }

//    // Fires a projectile toward the player
//    //void Shoot()
//    //{
//    //    if (projectilePool == null || enemyShootPoint == null || player == null)
//    //        return;

//    //    // Play attack animation
//    //    if (enemyAnimator != null) enemyAnimator.PlayAttack();

//    //    Vector3 targetPosition = player.position;
//    //    Vector3 direction = (targetPosition - enemyShootPoint.position).normalized;

//    //    GameObject proj = projectilePool.GetNextProjectile();
//    //    Rigidbody rb = proj.GetComponent<Rigidbody>();

//    //    Debug.Log($"[{name}] Shooting from: {enemyShootPoint.position}");

//    //    rb.isKinematic = false;
//    //    rb.detectCollisions = true;
//    //    rb.velocity = Vector3.zero;
//    //    rb.angularVelocity = Vector3.zero;
//    //    rb.drag = 0f;
//    //    rb.angularDrag = 0f;

//    //    proj.transform.position = enemyShootPoint.position;
//    //    proj.transform.rotation = Quaternion.LookRotation(direction);
//    //    rb.velocity = direction * 20f;

//    //    proj.SetActive(true);

//    //    //Debug.DrawRay(enemyShootPoint.position, direction * 10f, Color.red, 2f);

//    //    // Restart walking if needed
//    //    if (enemyAnimator != null && Vector3.Distance(transform.position, player.position) > stopDistance)
//    //    {
//    //        enemyAnimator.SetWalking(true);
//    //    }
//    //}

//    void Shoot()
//    {
//        if (projectilePool == null || player == null)
//            return;

//        if (enemyShootPoint == null)
//        {
//            Debug.LogWarning($"[{name}] enemyShootPoint not assigned");
//            return;
//        }

//        if (enemyAnimator != null)
//            enemyAnimator.PlayAttack();

//        Vector3 shootFrom = enemyShootPoint.position;
//        Vector3 target = player.position;
//        Vector3 direction = (target - shootFrom).normalized;

//        GameObject proj = projectilePool.GetNextProjectile();
//        proj.transform.SetPositionAndRotation(shootFrom, Quaternion.LookRotation(direction));

//        Rigidbody rb = proj.GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            rb.velocity = direction * 20f;
//        }
//        //Debug.Log($"{name} shooting from {enemyShootPoint.position}");
//        proj.SetActive(true); 
//    }


//    Vector3 CalculateAvoidance()
//    {
//        Vector3 avoidance = Vector3.zero;
//        Collider[] hits = Physics.OverlapSphere(transform.position, avoidRadius, enemyLayer);

//        foreach (Collider hit in hits)
//        {
//            if (hit.gameObject != gameObject)
//            {
//                Vector3 offset = transform.position - hit.transform.position;
//                float distance = offset.magnitude;

//                if (distance > 0f)
//                {
//                    Vector3 sideStep = Vector3.Cross(offset.normalized, Vector3.up);
//                    avoidance += (offset.normalized + sideStep * 0.5f) / distance;
//                }
//            }
//        }

//        return avoidance;
//    }

//}

using System.Linq;
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
    public Transform enemyShootPoint;
    private float fireTimer = 0f;
    private float nextFireTime = 0f;

    private Transform player;
    private EnemyProjectilePool projectilePool;
    private SC_GameManager gameManager;
    private SC_EnemyAnimator enemyAnimator;

    private float debugInterval = 3f;
    private float debugTimer = 0f;

    private bool isDead = false;  // New: prevent shooting and movement after death

    void Start()
    {
        // Find references
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("[EnemyController] Player not found in scene!");
        }

        projectilePool = FindObjectOfType<EnemyProjectilePool>();
        gameManager = FindObjectOfType<SC_GameManager>();
        enemyAnimator = GetComponent<SC_EnemyAnimator>();

        enemyShootPoint = transform.Find("enemyShootPoint");

        if (enemyShootPoint == null)
            Debug.LogWarning($"[{name}] enemyShootPoint NOT FOUND");
        else
            Debug.Log($"[{name}] enemyShootPoint FOUND at {enemyShootPoint.position}");

        SetInitialFireDelay();
    }

    void Update()
    {
        if (isDead)
            return;

        debugTimer += Time.deltaTime;
        if (debugTimer >= debugInterval)
        {
            debugTimer = 0f;
            Vector3 enemyPos = transform.position;
            Vector3 shootPointPos = enemyShootPoint != null ? enemyShootPoint.position : Vector3.zero;
            // Debug.Log($"[{name}] Enemy position: {enemyPos}, enemyShootPoint: {shootPointPos}");
        }

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool shouldWalk = distanceToPlayer > stopDistance;

        if (enemyAnimator != null)
        {
            enemyAnimator.SetWalking(shouldWalk);
        }

        if (shouldWalk)
        {
            Vector3 moveDirection = (player.position - transform.position).normalized;
            Vector3 avoidance = CalculateAvoidance();
            float avoidanceStrength = 3f;
            Vector3 finalDirection = (moveDirection + avoidance * avoidanceStrength).normalized;

            if (finalDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            Vector3 moveDelta = finalDirection * moveSpeed * Time.deltaTime;
            transform.position += moveDelta;
        }

        fireTimer += Time.deltaTime;
        if (fireTimer >= nextFireTime)
        {
            fireTimer = 0f;
            Shoot();
            SetNextFireDelay();
        }

        if (isActiveAndEnabled)
        {
            Vector3 pos = transform.position;
            pos.y = -0.5f;
            transform.position = pos;
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
        SetInitialFireDelay();
    }

    void Shoot()
    {
        if (projectilePool == null || player == null)
            return;

        if (enemyShootPoint == null)
        {
            Debug.LogWarning($"[{name}] enemyShootPoint not assigned");
            return;
        }

        if (enemyAnimator != null)
            enemyAnimator.PlayAttack();

        Vector3 shootFrom = enemyShootPoint.position;
        Vector3 target = player.position;
        Vector3 direction = (target - shootFrom).normalized;

        GameObject proj = projectilePool.GetNextProjectile();
        proj.transform.SetPositionAndRotation(shootFrom, Quaternion.LookRotation(direction));

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * 20f;
        }

        proj.SetActive(true);
    }

    Vector3 CalculateAvoidance()
    {
        Vector3 avoidance = Vector3.zero;
        Collider[] hits = Physics.OverlapSphere(transform.position, avoidRadius, enemyLayer);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                Vector3 offset = transform.position - hit.transform.position;
                float distance = offset.magnitude;

                if (distance > 0f)
                {
                    Vector3 sideStep = Vector3.Cross(offset.normalized, Vector3.up);
                    avoidance += (offset.normalized + sideStep * 0.5f) / distance;
                }
            }
        }

        return avoidance;
    }

    // Called externally when enemy dies
    public void Die()
    {
        isDead = true;
    }
}

