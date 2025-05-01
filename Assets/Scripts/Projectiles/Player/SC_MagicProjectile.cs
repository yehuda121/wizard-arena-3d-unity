
//using UnityEngine;

//public class SC_MagicProjectile : MonoBehaviour
//{
//    // This value is no longer used directly but can be kept for flexibility
//    public float damage = 25f;

//    public enum ShooterType { Player, Enemy }
//    public ShooterType shooter;

//    private void OnTriggerEnter(Collider other)
//    {
//        //Debug.Log("player projectile trigered");
//        //Player shot and hit an Enemy
//        if(shooter == ShooterType.Player && (other.CompareTag("Enemy") || other.CompareTag("BossEnemy")))
//{
//            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
//            if (enemyHealth != null)
//            {
//                float damagePercent = damage / enemyHealth.maxHealth;
//                enemyHealth.TakeDamage(enemyHealth.maxHealth * damagePercent);
//            }

//            gameObject.SetActive(false); // always disable after valid hit
//        }


//        // Enemy shot and hit the Player
//        else if (shooter == ShooterType.Enemy && other.CompareTag("Player"))
//        {
//            SC_PlayerHealthSystem playerHealth = other.GetComponent<SC_PlayerHealthSystem>();
//            if (playerHealth != null)
//            {
//                float damageAmount = 10f; 
//                playerHealth.TakeDamage(damageAmount);
//            }
//            gameObject.SetActive(false); // always disable after valid hit
//        }


//        // Collision with anything else
//        else if (!other.CompareTag("Enemy") && !other.CompareTag("Player"))
//        {
//            gameObject.SetActive(false);
//        }

//        // No action if projectile hits another projectile or its own shooter-type target
//    }
//}
using UnityEngine;

// Handles projectiles shot by the player
public class SC_MagicProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Collision with anything else (not a valid targets)
        if (!other.CompareTag("Enemy") && !other.CompareTag("BossEnemy"))
        {
            gameObject.SetActive(false);
            return;
        }
        // Player shot and hit an enemy
        SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
        if (enemyHealth != null)
        {
            float percentDamage = 0f;

            // Determine damage based on enemy type
            if (other.CompareTag("BossEnemy"))
            {
                percentDamage = 0.10f; // 10% damage to boss
            }
            else if (other.CompareTag("Enemy"))
            {
                percentDamage = 0.25f; // 25% damage to regular enemy
            }

            if (percentDamage > 0f)
            {
                enemyHealth.TakeDamage(enemyHealth.maxHealth * percentDamage);
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(false);
        }
    }
}
