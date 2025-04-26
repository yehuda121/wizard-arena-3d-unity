
//using UnityEngine;

//public class SC_MagicProjectile : MonoBehaviour
//{
//    public float damage = 25f;

//    private void OnTriggerEnter(Collider other)
//    {
//        // If the projectile hits an enemy
//        if (other.CompareTag("Enemy"))
//        {
//            // Try to get the enemy's health system component
//            SC_HealthSystem health = other.GetComponent<SC_HealthSystem>();
//            if (health != null)
//            {
//                // Deal damage to the enemy
//                health.TakeDamage(damage);
//            }

//            // Disable the projectile (don't destroy, because we are using object pooling)
//            gameObject.SetActive(false);
//        }
//        // If the projectile hits something else (but not the player)
//        else if (!other.CompareTag("Player"))
//        {
//            // Disable the projectile on any other collision like a wall
//            gameObject.SetActive(false);
//        }
//    }
//}
using UnityEngine;

public class SC_MagicProjectile : MonoBehaviour
{
    // This value is no longer used directly but can be kept for flexibility
    public float damage = 25f;

    public enum ShooterType { Player, Enemy }
    public ShooterType shooter;

    private void OnTriggerEnter(Collider other)
    {
        //Player shot and hit an Enemy
        if (shooter == ShooterType.Player && other.CompareTag("Enemy"))
        {
            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                float percent = 0.25f; // 25% damage
                enemyHealth.TakeDamage(enemyHealth.maxHealth * percent);
            }

            gameObject.SetActive(false); // always disable after valid hit
        }

        // Enemy shot and hit the Player
        else if (shooter == ShooterType.Enemy && other.CompareTag("Player"))
        {
            SC_Player player = other.GetComponent<SC_Player>();
            if (player != null)
            {
                float percent = 0.10f; // 10% damage
                player.TakeDamage(player.maxHealth * percent);
            }

            gameObject.SetActive(false); // always disable after valid hit
        }

        // Collision with anything else
        else if (!other.CompareTag("Projectile") && !other.CompareTag("Enemy") && !other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }

        // No action if projectile hits another projectile or its own shooter-type target
    }
}
