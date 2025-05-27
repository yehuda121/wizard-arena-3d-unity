using UnityEngine;

public class SC_EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;               // Movement speed of the enemy
    public float stopDistance = 8f;            // Minimum distance to stop moving toward the player

    [Header("Avoidance Settings")]
    public float avoidRadius = 8f;             // Radius to detect nearby enemies to avoid overlapping
    public LayerMask enemyLayer;               // Layer used to detect other enemies

    [Header("Shooting Settings")]
    public Transform shootPoint;               // The point where projectiles are spawned
    private float fireTimer = 0f;              // Time counter between shots
    private float nextFireTime = 0f;           // Time to wait before the next shot

    private Transform player;                  // Reference to the player's position
    private EnemyProjectilePool projectilePool; // Reference to projectile object pool
    private SC_GameManager gameManager;        // Reference to the GameManager
    private SC_EnemyAnimator enemyAnimator;    // Reference to the enemy animator script

    void Start()
    {
        // Get references
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("[EnemyController] Player not found in scene!");
        }

        projectilePool = FindObjectOfType<EnemyProjectilePool>();
        gameManager = FindObjectOfType<SC_GameManager>();
        enemyAnimator = GetComponent<SC_EnemyAnimator>();

        SetInitialFireDelay();
    }

    void Update()
    {
        if (player == null) return;

        // === DISTANCE TO PLAYER ===
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Determine whether the enemy should walk or stop
        bool shouldWalk = distanceToPlayer > stopDistance;
        //Debug.Log("Speed = " + (shouldWalk ? "WALKING" : "IDLE"));

        // Set walking animation (true if moving, false if idle)
        if (enemyAnimator != null)
        {
            enemyAnimator.SetWalking(shouldWalk);
        }

        // === MOVEMENT LOGIC ===
        if (shouldWalk)
        {
            // Calculate direction toward the player
            Vector3 moveDirection = (player.position - transform.position).normalized;

            // Calculate avoidance from nearby enemies
            Vector3 avoidance = CalculateAvoidance();
            float avoidanceStrength = 3f;

            // Combine movement and avoidance
            Vector3 finalDirection = (moveDirection + avoidance * avoidanceStrength).normalized;

            // Smoothly rotate to face movement direction
            if (finalDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // Move the enemy forward
            Vector3 moveDelta = finalDirection * moveSpeed * Time.deltaTime;
            transform.position += moveDelta;
        }

        // === SHOOTING LOGIC ===
        fireTimer += Time.deltaTime;

        // Fire projectile when delay has passed
        if (fireTimer >= nextFireTime)
        {
            fireTimer = 0f;
            Shoot();
            SetNextFireDelay();
        }
        
        // Only clamp Y position if enemy is alive and on ground
        if (isActiveAndEnabled) // Avoids clamping dead/falling enemies
        {
            Vector3 pos = transform.position;
            pos.y = -0.5f; // depending on your floor
            transform.position = pos;
        }
    }


    // Sets the initial fire delay based on current difficulty
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

    // Used after each shot to determine delay until next shot
    void SetNextFireDelay()
    {
        SetInitialFireDelay();
    }

    // Fires a projectile toward the player
    void Shoot()
    {
        if (projectilePool == null || shootPoint == null || player == null)
            return;

        // Play attack animation
        if (enemyAnimator != null) enemyAnimator.PlayAttack();

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

        //Debug.DrawRay(shootPoint.position, direction * 10f, Color.red, 2f);

        // Restart walking if needed
        if (enemyAnimator != null && Vector3.Distance(transform.position, player.position) > stopDistance)
        {
            enemyAnimator.SetWalking(true);
        }
    }

    // Calculates a movement vector that avoids overlapping other enemies
    //Vector3 CalculateAvoidance()
    //{
    //    Vector3 avoidance = Vector3.zero;
    //    Collider[] hits = Physics.OverlapSphere(transform.position, avoidRadius, enemyLayer);

    //    foreach (Collider hit in hits)
    //    {
    //        if (hit.gameObject != gameObject)
    //        {
    //            Vector3 pushDir = transform.position - hit.transform.position;
    //            float distance = pushDir.magnitude;

    //            if (distance > 0)
    //            {
    //                avoidance += pushDir.normalized / distance;
    //            }
    //        }
    //    }

    //    return avoidance;
    //}
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
                    // דחיפה אחורית + דחיפה לצד (ב־90 מעלות)
                    Vector3 sideStep = Vector3.Cross(offset.normalized, Vector3.up);
                    avoidance += (offset.normalized + sideStep * 0.5f) / distance;
                }
            }
        }

        return avoidance;
    }

}
