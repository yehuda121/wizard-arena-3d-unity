//using UnityEngine;

//public class SC_EnemyProjectile : MonoBehaviour
//{
//    public enum ShooterType { Player, Enemy }
//    public ShooterType shooter;

//    private void OnTriggerEnter(Collider other)
//    {
//        //Debug.Log("player projectile trigered");
//        //Player shot and hit an Enemy
//        if (shooter == ShooterType.Player && other.CompareTag("Enemy"))
//        {
//            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
//            if (enemyHealth != null)
//            {
//                float damage = 0.25f; // 25% damage
//                enemyHealth.TakeDamage(enemyHealth.maxHealth * damage);
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

//        // No action if projectile hits another projectile or its own shooter-type target
//    }
//}
using UnityEngine;

// Handles projectiles fired by regular enemies
public class SC_EnemyProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Enemy shot and hit the player
        if (other.CompareTag("Player"))
        {
            SC_PlayerHealthSystem playerHealth = other.GetComponent<SC_PlayerHealthSystem>();
            if (playerHealth != null)
            {
                float damageAmount = playerHealth.isBlocking ? 0f : playerHealth.maxHealth * 0.10f;
                playerHealth.TakeDamage(damageAmount);
            }

            gameObject.SetActive(false);
        }
    }
}

