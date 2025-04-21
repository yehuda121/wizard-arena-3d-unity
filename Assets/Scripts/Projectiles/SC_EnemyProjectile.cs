using UnityEngine;

public class SC_EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;       // Amount of damage dealt to the player
    public float lifeTime = 2.5f;    // Time before the projectile is automatically disabled

    void OnEnable()
    {
        // When activated, start a timer to disable this projectile after lifeTime seconds
        Invoke(nameof(Disable), lifeTime);
    }

    void OnDisable()
    {
        // Cancel any scheduled Disable call (in case it hit something first)
        CancelInvoke();
    }

    void Disable()
    {
        // Stop the projectile's movement before disabling it (important for pooling)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Deactivate the projectile (instead of destroying it, for reuse)
        gameObject.SetActive(false);
    }

    // Handle collision with other objects using a trigger collider
    void OnTriggerEnter(Collider other)
    {
        // If the projectile hits the player
        if (other.CompareTag("Player"))
        {
            // Get the player's health script
            SC_Player player = other.GetComponent<SC_Player>();
            if (player != null)
            {
                player.TakeDamage(damage); // Apply damage to the player
            }

            Disable(); // Disable the projectile after hitting the player
        }
        // If the projectile hits anything else (e.g. wall), but not another enemy
        else if (!other.CompareTag("Enemy"))
        {
            Disable(); // Disable the projectile on any other collision
        }
    }
}
