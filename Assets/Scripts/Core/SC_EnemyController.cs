// This script handles basic enemy movement logic.
// The enemy moves toward the player until reaching a specified stop distance.
// Additionally, it avoids overlapping with other enemies using simple physics-based separation.

using UnityEngine;

public class SC_EnemyController : MonoBehaviour
{
    public float moveSpeed = 1f;             // Movement speed of the enemy
    public float stopDistance = 8f;          // How close the enemy should get to the player before stopping

    [Header("Avoidance Settings")]
    public float avoidRadius = 5f;           // Radius to check for nearby enemies to avoid overlapping
    public LayerMask enemyLayer;             // Layer mask to detect other enemies

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogWarning("[EnemyController] Player not found in scene!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Stop if too close to player
        if (distanceToPlayer <= stopDistance)
        {
            return;
        }

        // Move toward player + avoid others
        Vector3 moveDirection = (player.position - transform.position).normalized;
        Vector3 avoidance = CalculateAvoidance();

        // שליטה על עוצמת ההתרחקות מאויבים
        float avoidanceStrength = 1.2f;
        Vector3 finalDirection = (moveDirection + avoidance * avoidanceStrength).normalized;

        // שמירה על גובה קבוע
        Vector3 nextPosition = transform.position + finalDirection * moveSpeed * Time.deltaTime;
        nextPosition.y = transform.position.y;

        transform.position = nextPosition;
    }


    // This method calculates a separation vector from nearby enemies
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
