using UnityEngine;

public class SC_EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float stopDistance = 12f;

    [Header("Avoidance Settings")]
    public float avoidRadius = 10f;
    public LayerMask enemyLayer;

    [Header("Shooting Settings")]
    public Transform enemyShootPoint;
    public AudioClip enemyShootSound;
    private AudioSource audioSource;

    private float fireTimer = 0f;
    private float nextFireTime = 0f;

    private Transform player;
    private EnemyProjectilePool projectilePool;
    private SC_GameManager gameManager;
    private SC_EnemyAnimator enemyAnimator;

    private bool isDead = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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
            Debug.LogError($"[{name}] Missing enemyShootPoint! Please check prefab structure.");

        SetInitialFireDelay();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool shouldWalk = distanceToPlayer > stopDistance;

        if (enemyAnimator != null)
            enemyAnimator.SetWalking(shouldWalk);

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
                    nextFireTime = Random.Range(7f, 15f);
                    break;
                case DifficultyLevel.Medium:
                    nextFireTime = Random.Range(5f, 10f);
                    break;
                case DifficultyLevel.Hard:
                    nextFireTime = Random.Range(4f, 8f);
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

        if (audioSource != null && enemyShootSound != null)
        {
            audioSource.PlayOneShot(enemyShootSound, 0.5f);
        }
        else
        {
            Debug.Log("audioSource = null || enemyShootSound != null");
        }
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

    public void Die()
    {
        isDead = true;
    }

    public void ResetEnemy()
    {
        isDead = false;
        fireTimer = 0f;
        SetInitialFireDelay();
    }
}
