
using UnityEngine;

public class SC_MagicProjectile : MonoBehaviour
{
    // This value is no longer used directly but can be kept for flexibility
    public float damage = 25f;

    public enum ShooterType { Player, Enemy }
    public ShooterType shooter;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("player projectile trigered");
        //Player shot and hit an Enemy
        if(shooter == ShooterType.Player && other.CompareTag("Enemy"))
{
            SC_EnemyHealthSystem enemyHealth = other.GetComponent<SC_EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                float damagePercent = damage / enemyHealth.maxHealth;
                enemyHealth.TakeDamage(enemyHealth.maxHealth * damagePercent);

                // קטע חדש: ספירת הריגות
                PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
                if (playerShooting != null && !playerShooting.poweredUp)
                {
                    playerShooting.killCount++;
                    if (playerShooting.killCount >= 4)
                    {
                        playerShooting.killCount = 0;
                        playerShooting.ActivatePowerUp();
                    }
                }
            }

            gameObject.SetActive(false); // always disable after valid hit
        }


        // Enemy shot and hit the Player
        else if (shooter == ShooterType.Enemy && other.CompareTag("Player"))
        {
            SC_PlayerHealthSystem playerHealth = other.GetComponent<SC_PlayerHealthSystem>();
            if (playerHealth != null)
            {
                float damageAmount = 10f; 
                playerHealth.TakeDamage(damageAmount);
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
